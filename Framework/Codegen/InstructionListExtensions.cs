using System.Reflection;
using System.Reflection.Emit;

namespace StardewUI.Framework.Codegen;

internal static class InstructionListExtensions
{
    private static readonly ConstructorInfo decimalConstructor = typeof(decimal).GetConstructor(
        [typeof(int), typeof(int), typeof(int), typeof(bool), typeof(byte)]
    )!;

    public static void EmitAccessor(this IInstructionList il, MemberInfo member)
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

    public static void EmitCastOrUnbox(this IInstructionList il, Type type)
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

    public static void EmitLdarg(this IInstructionList il, Type type, int index)
    {
        if (type.IsValueType)
        {
            il.Emit(index >= 0 && index <= 255 ? OpCodes.Ldarga_S : OpCodes.Ldarga, index);
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

    public static void EmitLdc(this IInstructionList il, int value)
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

    public static void EmitLdefault(this IInstructionList il, Type type, object? defaultValue, int localIndex)
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
            byte scale = (byte)(parts[3] >> 16 & 0x7F);
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

    public static void EmitLdloc(this IInstructionList il, int index)
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

    public static void EmitStloc(this IInstructionList il, int index)
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
