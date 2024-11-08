using System.Reflection;
using System.Reflection.Emit;

namespace StardewUI.Framework.Codegen;

internal class ILGeneratorWrapper(ILGenerator il) : IInstructionList
{
    public IReadOnlyList<(OpCode, object?)> Instructions => instructions;

    private readonly List<(OpCode, object?)> instructions = [];

    public LocalBuilder DeclareLocal(Type localType)
    {
        return il.DeclareLocal(localType);
    }

    public void Emit(OpCode opcode, Type cls)
    {
        instructions.Add((opcode, cls));
        il.Emit(opcode, cls);
    }

    public void Emit(OpCode opcode, string str)
    {
        instructions.Add((opcode, str));
        il.Emit(opcode, str);
    }

    public void Emit(OpCode opcode, float arg)
    {
        instructions.Add((opcode, arg));
        il.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, sbyte arg)
    {
        instructions.Add((opcode, arg));
        il.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, MethodInfo meth)
    {
        instructions.Add((opcode, meth));
        il.Emit(opcode, meth);
    }

    public void Emit(OpCode opcode, FieldInfo field)
    {
        instructions.Add((opcode, field));
        il.Emit(opcode, field);
    }

    public void Emit(OpCode opcode, Label[] labels)
    {
        instructions.Add((opcode, labels));
        il.Emit(opcode, labels);
    }

    public void Emit(OpCode opcode, SignatureHelper signature)
    {
        instructions.Add((opcode, signature));
        il.Emit(opcode, signature);
    }

    public void Emit(OpCode opcode, LocalBuilder local)
    {
        instructions.Add((opcode, local));
        il.Emit(opcode, local);
    }

    public void Emit(OpCode opcode, ConstructorInfo con)
    {
        instructions.Add((opcode, con));
        il.Emit(opcode, con);
    }

    public void Emit(OpCode opcode, long arg)
    {
        instructions.Add((opcode, arg));
        il.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, int arg)
    {
        instructions.Add((opcode, arg));
        il.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, short arg)
    {
        instructions.Add((opcode, arg));
        il.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, double arg)
    {
        instructions.Add((opcode, arg));
        il.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, byte arg)
    {
        instructions.Add((opcode, arg));
        il.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode)
    {
        instructions.Add((opcode, null));
        il.Emit(opcode);
    }

    public void Emit(OpCode opcode, Label label)
    {
        instructions.Add((opcode, label));
        il.Emit(opcode, label);
    }
}
