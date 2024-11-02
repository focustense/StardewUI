using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Factory that creates duck-typing converters for <c>class</c> and <c>struct</c> types.
/// </summary>
/// <remarks>
/// For the conversion to be allowed:
/// <list type="number">
/// <item>The <c>TDestination</c> type must be annotated with <see cref="DuckTypeAttribute"/>.</item>
/// <item>The destination type must have either a default constructor, or a constructor that can be completely satisfied
/// by properties/fields of the <c>TSource</c> type.</item>
/// <item>If the best or only constructor match is the default/parameterless constructor, at least one writable property
/// on the target type must be satisfied by a property/field on the source type.</item>
/// </list>
/// Additionally, source types may use fields or properties, but only constructor arguments and properties will be
/// considered on the destination type.
/// </remarks>
/// <param name="innerFactory">The converter factory to handle conversion of individual properties/arguments.</param>
public class DuckTypeClassConverterFactory(IValueConverterFactory innerFactory) : IValueConverterFactory
{
    private static readonly MethodInfo valueConverterConvertMethod = typeof(IValueConverter).GetMethod("Convert")!;

    [ThreadStatic]
    private static Stack<(Type, Type)> activeRequests = [];

    record SourceMember(MemberInfo Member, Type ValueType);

    /// <inheritdoc />
    public bool TryGetConverter<TSource, TDestination>(
        [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
    )
    {
        converter = null;
        var requestKey = (typeof(TSource), typeof(TDestination));
        if (activeRequests.Contains(requestKey))
        {
            Logger.Log(
                $"Cycle detected in duck-type conversion from {typeof(TSource).FullName} to "
                    + $"{typeof(TDestination).FullName}. Conversion between these types is disabled.",
                LogLevel.Warn
            );
            return false;
        }
        if (
            !IsClassOrStruct(typeof(TSource))
            || !IsClassOrStruct(typeof(TDestination))
            || !HasDuckTypeAttribute(typeof(TDestination))
        )
        {
            return false;
        }
        activeRequests.Push(requestKey);
        try
        {
            var sourceMembers = (
                from member in typeof(TSource).GetMembers(BindingFlags.Instance | BindingFlags.Public)
                let valueType = GetMemberValueType(member)
                where valueType is not null
                let convertedNames = GetConvertedNames(member, typeof(TDestination))
                from convertedName in convertedNames
                select (member, valueType, convertedName)
            ).ToDictionary(
                x => x.convertedName,
                x => new SourceMember(x.member, x.valueType),
                StringComparer.OrdinalIgnoreCase
            );
            // Always start with the most specific constructor we can. If some arguments don't match/can't be obtained, then
            // we can widen the search.
            var destinationConstructors = typeof(TDestination)
                .GetConstructors()
                .Select(ctor => MatchesAllParameters(ctor, sourceMembers, out var count) ? (ctor, count) : default)
                .Where(x => x.ctor is not null)
                .OrderByDescending(x => x.count)
                .Select(x => x.ctor)
                .ToArray();
            foreach (var ctor in destinationConstructors)
            {
                converter = TryCreateConverter<TSource, TDestination>(ctor, sourceMembers);
                if (converter is not null)
                {
                    return true;
                }
            }
            return false;
        }
        finally
        {
            activeRequests.Pop();
        }
    }

    private static IEnumerable<string> GetConvertedNames(MemberInfo member, Type destinationType)
    {
        var attributes = member.GetCustomAttributes<DuckPropertyAttribute>().ToArray();
        if (attributes.Length == 0)
        {
            return [member.Name];
        }
        return attributes
            .Where(a => string.IsNullOrEmpty(a.TargetTypeName) || a.TargetTypeName == destinationType.Name)
            .Select(a => a.TargetPropertyName);
    }

    private static Type? GetMemberValueType(MemberInfo member)
    {
        return member switch
        {
            FieldInfo field => field.FieldType,
            PropertyInfo property => property.CanRead && property.GetIndexParameters().Length == 0
                ? property.PropertyType
                : null,
            _ => null,
        };
    }

    private static bool HasDuckTypeAttribute(Type type)
    {
        return type.GetCustomAttributes().Any(a => a.GetType()?.Name == nameof(DuckTypeAttribute));
    }

    private static bool IsClassOrStruct(Type type)
    {
        return type.IsClass || (type.IsValueType && !type.IsPrimitive && !type.IsEnum);
    }

    private bool MatchesAllParameters(
        ConstructorInfo ctor,
        IReadOnlyDictionary<string, SourceMember> sourceMembers,
        out int matchedParameterCount
    )
    {
        matchedParameterCount = 0;
        foreach (var parameter in ctor.GetParameters())
        {
            if (
                sourceMembers.TryGetValue(parameter.Name ?? "", out var sourceMember)
                && innerFactory.TryGetConverter(sourceMember.ValueType, parameter.ParameterType, out _)
            )
            {
                matchedParameterCount++;
            }
            else if (!parameter.HasDefaultValue)
            {
                return false;
            }
        }
        return true;
    }

    private IValueConverter<TSource, TDestination>? TryCreateConverter<TSource, TDestination>(
        ConstructorInfo ctor,
        IReadOnlyDictionary<string, SourceMember> sourceMembers
    )
    {
        var ctorParameters = ctor.GetParameters();
        var ctorSourceMembers = new SourceMember[ctorParameters.Length];
        var ctorConverters = new IValueConverter[ctorParameters.Length];
        for (int i = 0; i < ctorParameters.Length; i++)
        {
            var parameter = ctorParameters[i];
            if (
                sourceMembers.TryGetValue(parameter.Name ?? "", out var sourceMember)
                && innerFactory.TryGetConverter(sourceMember.ValueType, parameter.ParameterType, out var converter)
            )
            {
                ctorSourceMembers[i] = sourceMember;
                ctorConverters[i] = converter;
            }
            else if (!parameter.HasDefaultValue)
            {
                return null;
            }
        }

        var writableProperties = typeof(TDestination)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p =>
                p.SetMethod?.IsPublic == true
                // Constructor argument lists shouldn't have dozens of items, so it's likely to be slower here to hash
                // all the names than it is to just do linear searches.
                && !ctorParameters.Any(cp => cp.Name?.Equals(p.Name, StringComparison.OrdinalIgnoreCase) == true)
            )
            .Select(p =>
                sourceMembers.TryGetValue(p.Name ?? "", out var sourceMember)
                && innerFactory.TryGetConverter(sourceMember.ValueType, p.PropertyType, out var converter)
                    ? (property: p, sourceMember, converter)
                    : default
            )
            .Where(x => x.property is not null)
            .ToArray();

        if (ctorParameters.Length == 0 && writableProperties.Length == 0)
        {
            return null;
        }

        var combinedConverters = new IValueConverter[ctorParameters.Length + writableProperties.Length];
        Array.Copy(ctorConverters, combinedConverters, ctorConverters.Length);

        var convertMethod = new DynamicMethod(
            $"Convert_{typeof(TSource).Name}_{typeof(TDestination).Name}",
            typeof(TDestination),
            [typeof(TSource), typeof(IValueConverter[])]
        );
        var il = convertMethod.GetILGenerator();
        for (int i = 0; i < ctorParameters.Length; i++)
        {
            var parameter = ctorParameters[i];
            il.DeclareLocal(parameter.ParameterType);
            var sourceMember = ctorSourceMembers[i];
            if (sourceMember is null)
            {
                // Only get here if the parameter has a default value AND doesn't have a source.
                continue;
            }
            il.Emit(OpCodes.Ldarg_1);
            il.EmitLdc(i);
            il.Emit(OpCodes.Ldelem_Ref);
            il.EmitLdarg(typeof(TSource), 0);
            il.EmitAccessor(sourceMember.Member);
            if (sourceMember.ValueType.IsValueType)
            {
                il.Emit(OpCodes.Box, sourceMember.ValueType);
            }
            il.Emit(OpCodes.Callvirt, valueConverterConvertMethod);
            il.EmitCastOrUnbox(parameter.ParameterType);
            il.EmitStloc(i);
        }
        for (int i = 0; i < ctorParameters.Length; i++)
        {
            if (ctorSourceMembers[i] is not null)
            {
                il.EmitLdloc(i);
            }
            else
            {
                il.EmitLdefault(ctorParameters[i].ParameterType, ctorParameters[i].DefaultValue, i);
            }
        }
        il.Emit(OpCodes.Newobj, ctor);
        il.DeclareLocal(typeof(TDestination));
        int resultLocalIndex = ctorParameters.Length;
        il.EmitStloc(resultLocalIndex);
        for (int i = 0; i < writableProperties.Length; i++)
        {
            var (property, sourceMember, converter) = writableProperties[i];
            combinedConverters[ctorConverters.Length + i] = converter;
            int localIndex = resultLocalIndex + i;
            il.EmitLdloc(resultLocalIndex);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitLdc(localIndex);
            il.Emit(OpCodes.Ldelem_Ref);
            il.EmitLdarg(typeof(TSource), 0);
            il.EmitAccessor(sourceMember.Member);
            if (sourceMember.ValueType.IsValueType)
            {
                il.Emit(OpCodes.Box, sourceMember.ValueType);
            }
            il.Emit(OpCodes.Callvirt, valueConverterConvertMethod);
            il.EmitCastOrUnbox(property.PropertyType);
            var setOpCode = property.SetMethod!.IsVirtual ? OpCodes.Callvirt : OpCodes.Call;
            il.Emit(setOpCode, property.SetMethod);
        }
        il.EmitLdloc(resultLocalIndex);
        il.Emit(OpCodes.Ret);

        var convertDelegate = convertMethod.CreateDelegate<Func<TSource, IValueConverter[], TDestination>>();
        return new ValueConverter<TSource, TDestination>(combinedConverters, convertDelegate);
    }

    class ValueConverter<TSource, TDestination>(
        IValueConverter[] converters,
        Func<TSource, IValueConverter[], TDestination> convert
    ) : IValueConverter<TSource, TDestination>
    {
        public TDestination Convert(TSource source)
        {
            return convert(source, converters);
        }
    }
}

file static class ILGeneratorExtensions
{
    private static readonly ConstructorInfo decimalConstructor = typeof(decimal).GetConstructor(
        [typeof(int), typeof(int), typeof(int), typeof(bool), typeof(byte)]
    )!;

    public static void EmitAccessor(this ILGenerator il, MemberInfo member)
    {
        if (member.MemberType == MemberTypes.Field)
        {
            il.Emit(OpCodes.Ldfld, (FieldInfo)member);
        }
        else
        {
            var getMethod = ((PropertyInfo)member).GetMethod!;
            var opCode = getMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call;
            il.Emit(opCode, getMethod);
        }
    }

    public static void EmitCastOrUnbox(this ILGenerator il, Type type)
    {
        if (type.IsValueType)
        {
            il.Emit(OpCodes.Unbox_Any, type);
        }
        else
        {
            il.Emit(OpCodes.Castclass, type);
        }
    }

    public static void EmitLdarg(this ILGenerator il, Type type, int index)
    {
        if (type.IsValueType)
        {
            il.Emit((index >= 0 && index <= 255) ? OpCodes.Ldarga_S : OpCodes.Ldarga, index);
        }
        else if (index >= 0 && index < 4)
        {
            il.Emit(
                index switch
                {
                    0 => OpCodes.Ldarg_0,
                    1 => OpCodes.Ldarg_1,
                    2 => OpCodes.Ldarg_2,
                    3 => OpCodes.Ldarg_3,
                    _ => throw new InvalidOperationException("Invalid argument index for Ldarg"),
                }
            );
        }
        else if (index >= 0 && index <= 255)
        {
            il.Emit(OpCodes.Ldarg_S, index);
        }
        else
        {
            il.Emit(OpCodes.Ldarg, index);
        }
    }

    public static void EmitLdc(this ILGenerator il, int value)
    {
        if (value >= -1 && value < 9)
        {
            il.Emit(
                value switch
                {
                    -1 => OpCodes.Ldc_I4_M1,
                    0 => OpCodes.Ldc_I4_0,
                    1 => OpCodes.Ldc_I4_1,
                    2 => OpCodes.Ldc_I4_2,
                    3 => OpCodes.Ldc_I4_3,
                    4 => OpCodes.Ldc_I4_4,
                    5 => OpCodes.Ldc_I4_5,
                    6 => OpCodes.Ldc_I4_6,
                    7 => OpCodes.Ldc_I4_7,
                    8 => OpCodes.Ldc_I4_8,
                    _ => throw new InvalidOperationException("Invalid argument index for Ldc"),
                }
            );
        }
        else if (value >= 0 && value <= 255)
        {
            il.Emit(OpCodes.Ldc_I4_S, value);
        }
        else
        {
            il.Emit(OpCodes.Ldc_I4, value);
        }
    }

    public static void EmitLdefault(this ILGenerator il, Type type, object? defaultValue, int localIndex)
    {
        if (defaultValue is null)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Ldloca_S, localIndex);
                il.Emit(OpCodes.Initobj, type);
                il.EmitLdloc(localIndex);
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }
        }
        else if (defaultValue is bool b)
        {
            il.Emit(b ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
        }
        else if (
            defaultValue is sbyte
            || defaultValue is byte
            || defaultValue is short
            || defaultValue is ushort
            || defaultValue is int
            || defaultValue is uint
            || defaultValue is Enum
        )
        {
            il.EmitLdc((int)defaultValue);
        }
        else if (defaultValue is long || defaultValue is ulong)
        {
            il.Emit(OpCodes.Ldc_I8, (long)defaultValue);
        }
        else if (defaultValue is float f)
        {
            il.Emit(OpCodes.Ldc_R4, f);
        }
        else if (defaultValue is double db)
        {
            il.Emit(OpCodes.Ldc_R8, db);
        }
        else if (defaultValue is decimal dc)
        {
            var parts = decimal.GetBits(dc);
            bool sign = (parts[3] & 0x80000000) != 0;
            byte scale = (byte)((parts[3] >> 16) & 0x7F);
            il.EmitLdc(parts[0]);
            il.EmitLdc(parts[1]);
            il.EmitLdc(parts[2]);
            il.Emit(sign ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
            il.EmitLdc(scale);
            il.Emit(OpCodes.Newobj, decimalConstructor);
        }
        else if (defaultValue is string s)
        {
            il.Emit(OpCodes.Ldstr, s);
        }
        else
        {
            throw new ArgumentException(
                $"Unable to determine instructions for default value {defaultValue}, type {defaultValue?.GetType().FullName}",
                nameof(defaultValue)
            );
        }
    }

    public static void EmitLdloc(this ILGenerator il, int index)
    {
        if (index >= 0 && index < 4)
        {
            il.Emit(
                index switch
                {
                    0 => OpCodes.Ldloc_0,
                    1 => OpCodes.Ldloc_1,
                    2 => OpCodes.Ldloc_2,
                    3 => OpCodes.Ldloc_3,
                    _ => throw new InvalidOperationException("Invalid argument index for Ldloc"),
                }
            );
        }
        else
        {
            il.Emit(OpCodes.Ldloc_S, index);
        }
    }

    public static void EmitStloc(this ILGenerator il, int index)
    {
        if (index >= 0 && index < 4)
        {
            il.Emit(
                index switch
                {
                    0 => OpCodes.Stloc_0,
                    1 => OpCodes.Stloc_1,
                    2 => OpCodes.Stloc_2,
                    3 => OpCodes.Stloc_3,
                    _ => throw new InvalidOperationException("Invalid argument index for Stloc"),
                }
            );
        }
        else
        {
            il.Emit(OpCodes.Stloc_S, index);
        }
    }
}
