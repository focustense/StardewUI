using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using StardewUI.Framework.Codegen;

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
    /// <summary>
    /// Whether or not to print MSIL output for generated conversion methods.
    /// </summary>
    /// <remarks>
    /// Use for troubleshooting misbehaving converters or AVE crashes.
    /// </remarks>
    public bool EnableDebugOutput { get; set; }

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

        var allMembers = typeof(TDestination).GetMembers(BindingFlags.Instance | BindingFlags.Public);
        var writableFieldsAndProperties = allMembers
            .Where(m => m.MemberType == MemberTypes.Field || (m is PropertyInfo p && p.SetMethod?.IsPublic == true))
            .Where(m =>
                // Constructor argument lists shouldn't have dozens of items, so it's likely to be slower here to hash
                // all the names than it is to just do linear searches.
                !ctorParameters.Any(cp => cp.Name?.Equals(m.Name, StringComparison.OrdinalIgnoreCase) == true)
            )
            .Select(m =>
                (
                    member: m,
                    type: m switch
                    {
                        FieldInfo f => f.FieldType,
                        PropertyInfo p => p.PropertyType,
                        _ => throw new InvalidOperationException(
                            $"Member '{m.Name}' has invalid member type {m.MemberType}."
                        ),
                    }
                )
            )
            .Select(m =>
                sourceMembers.TryGetValue(m.member.Name ?? "", out var sourceMember)
                && innerFactory.TryGetConverter(sourceMember.ValueType, m.type, out var converter)
                    ? (m.member, m.type, sourceMember, converter)
                    : default
            )
            .Where(x => x.member is not null)
            .ToArray();

        if (ctorParameters.Length == 0 && writableFieldsAndProperties.Length == 0)
        {
            return null;
        }

        var combinedConverters = new IValueConverter[ctorParameters.Length + writableFieldsAndProperties.Length];
        Array.Copy(ctorConverters, combinedConverters, ctorConverters.Length);

        var convertMethod = new DynamicMethod(
            $"Convert_{typeof(TSource).Name}_{typeof(TDestination).Name}",
            typeof(TDestination),
            [typeof(TSource), typeof(IValueConverter[])]
        );
        var il = new ILGeneratorWrapper(convertMethod.GetILGenerator());
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
        il.DeclareLocal(typeof(TDestination));
        int resultLocalIndex = ctorParameters.Length;
        if (typeof(TDestination).IsValueType)
        {
            il.Emit(OpCodes.Ldloca_S, resultLocalIndex);
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
        if (typeof(TDestination).IsValueType)
        {
            il.Emit(OpCodes.Call, ctor);
        }
        else
        {
            il.Emit(OpCodes.Newobj, ctor);
            il.EmitStloc(resultLocalIndex);
        }
        for (int i = 0; i < writableFieldsAndProperties.Length; i++)
        {
            var (member, writableType, sourceMember, converter) = writableFieldsAndProperties[i];
            combinedConverters[ctorConverters.Length + i] = converter;
            int localIndex = resultLocalIndex + i;
            if (typeof(TDestination).IsValueType)
            {
                il.Emit(OpCodes.Ldloca_S, resultLocalIndex);
            }
            else
            {
                il.EmitLdloc(resultLocalIndex);
            }
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
            il.EmitCastOrUnbox(writableType);
            if (member is PropertyInfo property)
            {
                var setOpCode = property.SetMethod!.IsVirtual ? OpCodes.Callvirt : OpCodes.Call;
                il.Emit(setOpCode, property.SetMethod);
            }
            else
            {
                il.Emit(OpCodes.Stfld, (FieldInfo)member);
            }
        }
        il.EmitLdloc(resultLocalIndex);
        il.Emit(OpCodes.Ret);

        if (EnableDebugOutput)
        {
            LogDebugOutput(typeof(TSource), typeof(TDestination), il.Instructions);
        }

        var convertDelegate = convertMethod.CreateDelegate<Func<TSource, IValueConverter[], TDestination>>();
        return new ValueConverter<TSource, TDestination>(combinedConverters, convertDelegate);
    }

    private static void LogDebugOutput(
        Type sourceType,
        Type destinationType,
        IReadOnlyList<(OpCode, object?)> instructions
    )
    {
        var sb = new StringBuilder();
        sb.AppendLine("Wrote instructions for duck-type converter: ");
        sb.Append("  Source: ");
        sb.AppendLine(sourceType.FullName ?? sourceType.Name);
        sb.Append("  Destination: ");
        sb.AppendLine(destinationType.FullName ?? destinationType.Name);
        sb.AppendLine();
        foreach (var (opcode, arg) in instructions)
        {
            sb.Append("  ");
            sb.Append(opcode.ToString()!.PadRight(12));
            sb.Append(arg);
            sb.AppendLine();
        }
        Logger.Log(sb.ToString(), LogLevel.Debug);
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
