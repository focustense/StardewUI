using System.Reflection;
using System.Reflection.Emit;

namespace StardewUI.Framework.Codegen;

/// <summary>
/// Abstract representation of an instruction list, e.g. for emitting instructions to an <see cref="ILGenerator"/>.
/// </summary>
internal interface IInstructionList
{
    /// <inheritdoc cref="ILGenerator.DeclareLocal(Type)" />
    LocalBuilder DeclareLocal(Type localType);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, Type)" />
    void Emit(OpCode opcode, Type cls);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, string)" />
    void Emit(OpCode opcode, string str);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, float)" />
    void Emit(OpCode opcode, float arg);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, sbyte)" />
    void Emit(OpCode opcode, sbyte arg);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, MethodInfo)" />
    void Emit(OpCode opcode, MethodInfo meth);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, FieldInfo)" />
    void Emit(OpCode opcode, FieldInfo field);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, Label[])" />
    void Emit(OpCode opcode, Label[] labels);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, SignatureHelper)" />
    void Emit(OpCode opcode, SignatureHelper signature);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, LocalBuilder)" />
    void Emit(OpCode opcode, LocalBuilder local);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, ConstructorInfo)" />
    void Emit(OpCode opcode, ConstructorInfo con);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, long)" />
    void Emit(OpCode opcode, long arg);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, int)" />
    void Emit(OpCode opcode, int arg);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, short)" />
    void Emit(OpCode opcode, short arg);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, double)" />
    void Emit(OpCode opcode, double arg);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, byte)" />
    void Emit(OpCode opcode, byte arg);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode)" />
    void Emit(OpCode opcode);

    /// <inheritdoc cref="ILGenerator.Emit(OpCode, Label)" />
    void Emit(OpCode opcode, Label label);
}
