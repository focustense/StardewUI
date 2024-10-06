using System.Reflection;
using System.Reflection.Metadata;
using static StardewValley.Minigames.TargetGame;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Helper for creating <see cref="IMethodDescriptor"/> instances using reflection.
/// </summary>
public static class ReflectionMethodDescriptor
{
    delegate IMethodDescriptor DescriptorFactory(MethodInfo method);

    private static readonly MethodInfo createDescriptorFactoryMethod = typeof(ReflectionMethodDescriptor).GetMethod(
        nameof(CreateDescriptorFactory),
        BindingFlags.Static | BindingFlags.NonPublic
    )!;
    private static readonly Dictionary<MethodInfo, IMethodDescriptor> descriptorCache = [];
    private static readonly Dictionary<Type, DescriptorFactory> factoryCache = [];

    /// <summary>
    /// Creates or retrieves a descriptor for a given method.
    /// </summary>
    /// <param name="method">The method info.</param>
    /// <returns>The descriptor for the specified <paramref name="method"/>.</returns>
    public static IMethodDescriptor FromMethodInfo(MethodInfo method)
    {
        if (!descriptorCache.TryGetValue(method, out var descriptor))
        {
            var descriptorFactory = GetDescriptorFactory(method.ReturnType);
            descriptor = descriptorFactory(method);
            descriptorCache.Add(method, descriptor);
        }
        return descriptor;
    }

    private static IMethodDescriptor CreateDescriptorFactory<TResult>(MethodInfo method)
    {
        var argumentAndResultTypes = GetArgumentAndResultTypes(method, out int optionalArgumentCount);
        return new ReflectionMethodDescriptor<TResult>(method, argumentAndResultTypes, optionalArgumentCount);
    }

    private static Type[] GetArgumentAndResultTypes(MethodInfo method, out int optionalArgumentCount)
    {
        optionalArgumentCount = 0;
        var declaringType =
            method.DeclaringType
            ?? throw new ArgumentException(
                $"Method '{method.Name}' has no declaring type. Anonymous methods are not supported in view bindings.",
                nameof(method)
            );
        var parameters = method.GetParameters();
        int argCount = method.IsStatic ? parameters.Length + 1 : parameters.Length + 2;
        var argTypes = new Type[argCount];
        int argIndex = 0;
        if (!method.IsStatic)
        {
            argTypes[0] = declaringType;
            argIndex++;
        }
        foreach (var param in parameters)
        {
            if (param.IsIn || param.IsOut)
            {
                throw new ArgumentException(
                    $"Method '{method.Name}' has invalid parameter '{param.Name}' "
                        + "(in, out and ref parameters are not supported).",
                    nameof(method)
                );
            }
            argTypes[argIndex++] = param.ParameterType;
            if (param.IsOptional)
            {
                optionalArgumentCount++;
            }
        }
        argTypes[argIndex] = method.ReturnType != typeof(void) ? method.ReturnType : typeof(object);
        return argTypes;
    }

    private static DescriptorFactory GetDescriptorFactory(Type returnType)
    {
        if (returnType == typeof(void))
        {
            returnType = typeof(object);
        }
        if (!factoryCache.TryGetValue(returnType, out var factory))
        {
            factory = createDescriptorFactoryMethod.MakeGenericMethod(returnType).CreateDelegate<DescriptorFactory>();
            factoryCache.Add(returnType, factory);
        }
        return factory;
    }
}

/// <summary>
/// Reflection-based implementation of a method descriptor.
/// </summary>
/// <typeparam name="TResult">The method's return type.</typeparam>
internal class ReflectionMethodDescriptor<TResult> : IMethodDescriptor<TResult>
{
    public ReadOnlySpan<Type> ArgumentTypes =>
        method.IsStatic ? argumentAndResultTypes : argumentAndResultTypes.AsSpan()[1..^1];

    public Type DeclaringType => method.DeclaringType!; // Validated to be non-null in GetArgumentTypes.

    public string Name => method.Name;

    public int OptionalArgumentCount { get; }

    public Type ReturnType => method.ReturnType;

    private readonly Type[] argumentAndResultTypes;
    private readonly IInvoker<TResult> invoker;
    private readonly MethodInfo method;

    /// <summary>
    /// Initializes a new instance of <see cref="ReflectionMethodDescriptor{TResult}"/>.
    /// </summary>
    /// <param name="method">The method to be described/invoked.</param>
    /// <param name="argumentAndResultTypes">An array whose elements are the method's declaring type (only if the method
    /// is not <c>static</c>), followed by all the normal argument types, and ending with the method's return type
    /// or the <see cref="Object"/> type if the method has void return.</param>
    /// <param name="optionalArgumentCount">Number of optional arguments at the end of the argument list.</param>
    public ReflectionMethodDescriptor(MethodInfo method, Type[] argumentAndResultTypes, int optionalArgumentCount)
    {
        if (method.IsGenericMethod || method.IsGenericMethodDefinition)
        {
            throw new ArgumentException(
                $"Generic method '{method.DeclaringType}.{method.Name}' cannot be used in a view binding.",
                nameof(method)
            );
        }
        this.method = method;
        this.argumentAndResultTypes = argumentAndResultTypes;
        OptionalArgumentCount = optionalArgumentCount;
        invoker = CreateInvoker();
    }

    public override bool Equals(object? obj)
    {
        return obj is ReflectionMethodDescriptor<TResult> other && other.method.Equals(method);
    }

    public override int GetHashCode()
    {
        return method.GetHashCode();
    }

    public virtual TResult Invoke(object? target, object?[] args)
    {
        return invoker.Invoke(target, args);
    }

    private IInvoker<TResult> CreateInvoker()
    {
        var invokerType = argumentAndResultTypes.Length switch
        {
            1 => typeof(Invoker<>).MakeGenericType(argumentAndResultTypes),
            2 => typeof(Invoker<,>).MakeGenericType(argumentAndResultTypes),
            3 => typeof(Invoker<,,>).MakeGenericType(argumentAndResultTypes),
            4 => typeof(Invoker<,,,>).MakeGenericType(argumentAndResultTypes),
            5 => typeof(Invoker<,,,,>).MakeGenericType(argumentAndResultTypes),
            6 => typeof(Invoker<,,,,,>).MakeGenericType(argumentAndResultTypes),
            7 => typeof(Invoker<,,,,,,>).MakeGenericType(argumentAndResultTypes),
            8 => typeof(Invoker<,,,,,,,>).MakeGenericType(argumentAndResultTypes),
            9 => typeof(Invoker<,,,,,,,,>).MakeGenericType(argumentAndResultTypes),
            _ => null,
        };
        return invokerType is not null
            ? (IInvoker<TResult>)invokerType.GetConstructor([typeof(MethodInfo)])!.Invoke([method])
            : new DefaultInvoker<TResult>(method);
    }
}

/// <summary>
/// Delegate used by <see cref="ReflectionMethodDescriptor{TResult}"/> to perform the method invocation.
/// </summary>
/// <typeparam name="TResult">The method's return type.</typeparam>
internal interface IInvoker<TResult>
{
    TResult Invoke(object? target, object?[] args);
}

file class DefaultInvoker<T>(MethodInfo method) : IInvoker<T>
{
    public T Invoke(object? target, object?[] args)
    {
        return (T)method.Invoke(target, args)!;
    }
}

// Generic invokers are all bespoke implementations for generics up to 8 params. This is mostly boilerplate that's easy
// to spit out of a code generator, but not easy to make any more concise without some built-in ability to use macros
// or meta-generics, which we don't have.
//
// If the number of arguments exceeds the number of available generics, then it doesn't break, it just reverts to the
// slower DefaultInvoker implementation using reflection instead of delegates.
file class Invoker<TResult>(MethodInfo method) : IInvoker<TResult>
{
    private readonly Action? invokeAction = method.ReturnType == typeof(void) ? method.CreateDelegate<Action>() : null;
    private readonly Func<TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.CreateDelegate<Func<TResult>>() : null;

    public TResult Invoke(object? target, object?[] args)
    {
        if (invokeFunc is not null)
        {
            return invokeFunc();
        }
        invokeAction!();
        return (TResult)(object)null!;
    }
}

file class Invoker<T0, TResult>(MethodInfo method) : IInvoker<TResult>
{
    private readonly Action<T0>? invokeAction =
        method.ReturnType == typeof(void) ? method.CreateDelegate<Action<T0>>() : null;
    private readonly Func<T0, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.CreateDelegate<Func<T0, TResult>>() : null;

    public TResult Invoke(object? target, object?[] args)
    {
        var arg0 = args.Get<T0>(target, 0);
        if (invokeFunc is not null)
        {
            return invokeFunc(arg0);
        }
        invokeAction!(arg0);
        return (TResult)(object)null!;
    }
}

file class Invoker<T0, T1, TResult>(MethodInfo method) : IInvoker<TResult>
{
    private readonly Action<T0, T1>? invokeAction =
        method.ReturnType == typeof(void) ? method.CreateDelegate<Action<T0, T1>>() : null;
    private readonly Func<T0, T1, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.CreateDelegate<Func<T0, T1, TResult>>() : null;

    public TResult Invoke(object? target, object?[] args)
    {
        var arg0 = args.Get<T0>(target, 0);
        var arg1 = args.Get<T1>(target, 1);
        if (invokeFunc is not null)
        {
            return invokeFunc(arg0, arg1);
        }
        invokeAction!(arg0, arg1);
        return (TResult)(object)null!;
    }
}

file class Invoker<T0, T1, T2, TResult>(MethodInfo method) : IInvoker<TResult>
{
    private readonly Action<T0, T1, T2>? invokeAction =
        method.ReturnType == typeof(void) ? method.CreateDelegate<Action<T0, T1, T2>>() : null;
    private readonly Func<T0, T1, T2, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.CreateDelegate<Func<T0, T1, T2, TResult>>() : null;

    public TResult Invoke(object? target, object?[] args)
    {
        var arg0 = args.Get<T0>(target, 0);
        var arg1 = args.Get<T1>(target, 1);
        var arg2 = args.Get<T2>(target, 2);
        if (invokeFunc is not null)
        {
            return invokeFunc(arg0, arg1, arg2);
        }
        invokeAction!(arg0, arg1, arg2);
        return (TResult)(object)null!;
    }
}

file class Invoker<T0, T1, T2, T3, TResult>(MethodInfo method) : IInvoker<TResult>
{
    private readonly Action<T0, T1, T2, T3>? invokeAction =
        method.ReturnType == typeof(void) ? method.CreateDelegate<Action<T0, T1, T2, T3>>() : null;
    private readonly Func<T0, T1, T2, T3, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.CreateDelegate<Func<T0, T1, T2, T3, TResult>>() : null;

    public TResult Invoke(object? target, object?[] args)
    {
        var arg0 = args.Get<T0>(target, 0);
        var arg1 = args.Get<T1>(target, 1);
        var arg2 = args.Get<T2>(target, 2);
        var arg3 = args.Get<T3>(target, 3);
        if (invokeFunc is not null)
        {
            return invokeFunc(arg0, arg1, arg2, arg3);
        }
        invokeAction!(arg0, arg1, arg2, arg3);
        return (TResult)(object)null!;
    }
}

file class Invoker<T0, T1, T2, T3, T4, TResult>(MethodInfo method) : IInvoker<TResult>
{
    private readonly Action<T0, T1, T2, T3, T4>? invokeAction =
        method.ReturnType == typeof(void) ? method.CreateDelegate<Action<T0, T1, T2, T3, T4>>() : null;
    private readonly Func<T0, T1, T2, T3, T4, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.CreateDelegate<Func<T0, T1, T2, T3, T4, TResult>>() : null;

    public TResult Invoke(object? target, object?[] args)
    {
        var arg0 = args.Get<T0>(target, 0);
        var arg1 = args.Get<T1>(target, 1);
        var arg2 = args.Get<T2>(target, 2);
        var arg3 = args.Get<T3>(target, 3);
        var arg4 = args.Get<T4>(target, 4);
        if (invokeFunc is not null)
        {
            return invokeFunc(arg0, arg1, arg2, arg3, arg4);
        }
        invokeAction!(arg0, arg1, arg2, arg3, arg4);
        return (TResult)(object)null!;
    }
}

file class Invoker<T0, T1, T2, T3, T4, T5, TResult>(MethodInfo method) : IInvoker<TResult>
{
    private readonly Action<T0, T1, T2, T3, T4, T5>? invokeAction =
        method.ReturnType == typeof(void) ? method.CreateDelegate<Action<T0, T1, T2, T3, T4, T5>>() : null;
    private readonly Func<T0, T1, T2, T3, T4, T5, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.CreateDelegate<Func<T0, T1, T2, T3, T4, T5, TResult>>() : null;

    public TResult Invoke(object? target, object?[] args)
    {
        var arg0 = args.Get<T0>(target, 0);
        var arg1 = args.Get<T1>(target, 1);
        var arg2 = args.Get<T2>(target, 2);
        var arg3 = args.Get<T3>(target, 3);
        var arg4 = args.Get<T4>(target, 4);
        var arg5 = args.Get<T5>(target, 5);
        if (invokeFunc is not null)
        {
            return invokeFunc(arg0, arg1, arg2, arg3, arg4, arg5);
        }
        invokeAction!(arg0, arg1, arg2, arg3, arg4, arg5);
        return (TResult)(object)null!;
    }
}

file class Invoker<T0, T1, T2, T3, T4, T5, T6, TResult>(MethodInfo method) : IInvoker<TResult>
{
    private readonly Action<T0, T1, T2, T3, T4, T5, T6>? invokeAction =
        method.ReturnType == typeof(void) ? method.CreateDelegate<Action<T0, T1, T2, T3, T4, T5, T6>>() : null;
    private readonly Func<T0, T1, T2, T3, T4, T5, T6, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.CreateDelegate<Func<T0, T1, T2, T3, T4, T5, T6, TResult>>() : null;

    public TResult Invoke(object? target, object?[] args)
    {
        var arg0 = args.Get<T0>(target, 0);
        var arg1 = args.Get<T1>(target, 1);
        var arg2 = args.Get<T2>(target, 2);
        var arg3 = args.Get<T3>(target, 3);
        var arg4 = args.Get<T4>(target, 4);
        var arg5 = args.Get<T5>(target, 5);
        var arg6 = args.Get<T6>(target, 6);
        if (invokeFunc is not null)
        {
            return invokeFunc(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        invokeAction!(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
        return (TResult)(object)null!;
    }
}

file class Invoker<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(MethodInfo method) : IInvoker<TResult>
{
    private readonly Action<T0, T1, T2, T3, T4, T5, T6, T7>? invokeAction =
        method.ReturnType == typeof(void) ? method.CreateDelegate<Action<T0, T1, T2, T3, T4, T5, T6, T7>>() : null;
    private readonly Func<T0, T1, T2, T3, T4, T5, T6, T7, TResult>? invokeFunc =
        method.ReturnType != typeof(void)
            ? method.CreateDelegate<Func<T0, T1, T2, T3, T4, T5, T6, T7, TResult>>()
            : null;

    public TResult Invoke(object? target, object?[] args)
    {
        var arg0 = args.Get<T0>(target, 0);
        var arg1 = args.Get<T1>(target, 1);
        var arg2 = args.Get<T2>(target, 2);
        var arg3 = args.Get<T3>(target, 3);
        var arg4 = args.Get<T4>(target, 4);
        var arg5 = args.Get<T5>(target, 5);
        var arg6 = args.Get<T6>(target, 6);
        var arg7 = args.Get<T7>(target, 7);
        if (invokeFunc is not null)
        {
            return invokeFunc(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        invokeAction!(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        return (TResult)(object)null!;
    }
}

file static class ArgumentExtensions
{
    public static T Get<T>(this object?[] args, object? target, int argIndex)
    {
        var argObject = target is not null
            ? argIndex == 0
                ? target
                : args[argIndex - 1]
            : args[argIndex];
        return (T)argObject!;
    }
}
