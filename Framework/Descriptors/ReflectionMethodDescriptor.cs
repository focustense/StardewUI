using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

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
    private static readonly ConcurrentDictionary<MethodInfo, IMethodDescriptor> descriptorCache = [];
    private static readonly ConcurrentDictionary<Type, DescriptorFactory> factoryCache = [];

    /// <summary>
    /// Creates or retrieves a descriptor for a given method.
    /// </summary>
    /// <param name="method">The method info.</param>
    /// <returns>The descriptor for the specified <paramref name="method"/>.</returns>
    internal static IMethodDescriptor FromMethodInfo(MethodInfo method)
    {
        using var _ = Trace.Begin(nameof(ReflectionMethodDescriptor), nameof(FromMethodInfo));
        var descriptor = descriptorCache.GetOrAdd(
            method,
            static method =>
            {
                var descriptorFactory = GetDescriptorFactory(method.ReturnType);
                return descriptorFactory(method);
            }
        );
        return descriptor;
    }

    /// <summary>
    /// Checks if a method is supported for view binding.
    /// </summary>
    /// <param name="method">The method info.</param>
    /// <returns><c>true</c> if a <see cref="ReflectionMethodDescriptor{TResult}"/> can be created for the specified
    /// <paramref name="method"/>, otherwise <c>false</c>.</returns>
    public static bool IsSupported(MethodInfo method)
    {
        return method.DeclaringType is not null
            && !method.IsGenericMethod
            && !method.IsGenericMethodDefinition
            && method.GetParameters().All(p => !p.IsIn && !p.IsOut && !p.ParameterType.IsByRef);
    }

    /// <summary>
    /// Pre-initializes some reflection state in order to make future invocations faster.
    /// </summary>
    internal static void Warmup()
    {
        ReflectionMethodDescriptor<object>.Warmup();
        ReflectionMethodDescriptor<bool>.Warmup();
    }

    private static IMethodDescriptor CreateDescriptorFactory<TResult>(MethodInfo method)
    {
        using var _ = Trace.Begin(nameof(ReflectionMethodDescriptor), nameof(CreateDescriptorFactory));
        GetArgumentAndResultTypes(
            method,
            out var argumentAndResultTypes,
            out var defaultValues,
            out int optionalArgumentCount
        );
        return new ReflectionMethodDescriptor<TResult>(
            method,
            argumentAndResultTypes,
            defaultValues,
            optionalArgumentCount
        );
    }

    private static void GetArgumentAndResultTypes(
        MethodInfo method,
        out Type[] argumentAndResultTypes,
        out object?[] defaultValues,
        out int optionalArgumentCount
    )
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
        argumentAndResultTypes = new Type[argCount];
        defaultValues = new object?[argCount - 2];
        int argIndex = 0;
        if (!method.IsStatic)
        {
            argumentAndResultTypes[0] = declaringType;
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
            defaultValues[argIndex - 1] = param.DefaultValue;
            argumentAndResultTypes[argIndex++] = param.ParameterType;
            if (param.IsOptional)
            {
                optionalArgumentCount++;
            }
        }
        argumentAndResultTypes[argIndex] = method.ReturnType != typeof(void) ? method.ReturnType : typeof(object);
    }

    private static DescriptorFactory GetDescriptorFactory(Type returnType)
    {
        using var _ = Trace.Begin(nameof(ReflectionMethodDescriptor), nameof(GetDescriptorFactory));
        if (returnType == typeof(void))
        {
            returnType = typeof(object);
        }
        return factoryCache.GetOrAdd(
            returnType,
            static returnType =>
                createDescriptorFactoryMethod.MakeGenericMethod(returnType).CreateDelegate<DescriptorFactory>()
        );
    }
}

/// <summary>
/// Reflection-based implementation of a method descriptor.
/// </summary>
/// <typeparam name="TResult">The method's return type.</typeparam>
internal class ReflectionMethodDescriptor<TResult> : IMethodDescriptor<TResult>
{
    public ReadOnlySpan<Type> ArgumentTypes =>
        method.IsStatic ? argumentAndResultTypes.AsSpan()[..^1] : argumentAndResultTypes.AsSpan()[1..^1];

    public Type DeclaringType => method.DeclaringType!; // Validated to be non-null in GetArgumentTypes.

    public string Name => method.Name;

    public int OptionalArgumentCount { get; }

    public Type ReturnType => method.ReturnType;

    private readonly Type[] argumentAndResultTypes;
    private readonly object?[] defaultValues;
    private readonly IInvoker<TResult> invoker;
    private readonly MethodInfo method;
    private readonly Dictionary<int, IInvoker<TResult>> optionalInvokers = [];

    /// <summary>
    /// Pre-initializes some reflection state in order to make future invocations faster.
    /// </summary>
    internal static void Warmup()
    {
        for (int i = 1; i < 9; i++)
        {
            var argumentTypes = new Type[i];
            Array.Fill(argumentTypes, typeof(object));
            GetInvokerType(argumentTypes);
        }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ReflectionMethodDescriptor{TResult}"/>.
    /// </summary>
    /// <param name="method">The method to be described/invoked.</param>
    /// <param name="argumentAndResultTypes">An array whose elements are the method's declaring type (only if the method
    /// is not <c>static</c>), followed by all the normal argument types, and ending with the method's return type
    /// or the <see cref="Object"/> type if the method has void return.</param>
    /// <param name="defaultValues">An array of the default values for each argument, in the same order that they appear
    /// in <paramref name="argumentAndResultTypes"/> but not including the final entry for the return type.</param>
    /// <param name="optionalArgumentCount">Number of optional arguments at the end of the argument list.</param>
    public ReflectionMethodDescriptor(
        MethodInfo method,
        Type[] argumentAndResultTypes,
        object?[] defaultValues,
        int optionalArgumentCount
    )
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
        this.defaultValues = defaultValues;
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
        using var _ = Trace.Begin(this, nameof(Invoke));
        if (!invoker.SupportsMissingArguments && args.Length > 0 && args[^1] == Type.Missing)
        {
            for (int i = args.Length - 1; i >= 0; i--)
            {
                if (args[i] != Type.Missing)
                {
                    break;
                }
                args[i] = defaultValues[i];
            }
        }
        return invoker.Invoke(target, args);
    }

    private IInvoker<TResult> CreateInvoker()
    {
        return CreateInvoker(argumentAndResultTypes);
    }

    private IInvoker<TResult> CreateInvoker(Type[] argumentTypes)
    {
        using var _ = Trace.Begin(this, nameof(CreateInvoker));
        var invokerType = GetInvokerType(argumentTypes);
        return invokerType is not null
            ? (IInvoker<TResult>)invokerType.GetConstructor([typeof(MethodInfo)])!.Invoke([method])
            : new ReflectionInvoker<TResult>(method);
    }

    private static Type? GetInvokerType(Type[] argumentTypes)
    {
        return argumentTypes.Length switch
        {
            1 => typeof(Invoker<>).MakeGenericType(argumentTypes),
            2 => typeof(Invoker<,>).MakeGenericType(argumentTypes),
            3 => typeof(Invoker<,,>).MakeGenericType(argumentTypes),
            4 => typeof(Invoker<,,,>).MakeGenericType(argumentTypes),
            5 => typeof(Invoker<,,,,>).MakeGenericType(argumentTypes),
            6 => typeof(Invoker<,,,,,>).MakeGenericType(argumentTypes),
            7 => typeof(Invoker<,,,,,,>).MakeGenericType(argumentTypes),
            8 => typeof(Invoker<,,,,,,,>).MakeGenericType(argumentTypes),
            9 => typeof(Invoker<,,,,,,,,>).MakeGenericType(argumentTypes),
            _ => null,
        };
    }
}

/// <summary>
/// Delegate used by <see cref="ReflectionMethodDescriptor{TResult}"/> to perform the method invocation.
/// </summary>
/// <typeparam name="TResult">The method's return type.</typeparam>
internal interface IInvoker<TResult>
{
    /// <summary>
    /// Whether or not the invoker can handle <see cref="Type.Missing"/> in arguments.
    /// </summary>
    /// <remarks>
    /// Invokers that do not support this (i.e. any delegates) must have those arguments replaced with defaults.
    /// </remarks>
    bool SupportsMissingArguments => false;

    /// <summary>
    /// Invokes the method.
    /// </summary>
    /// <param name="target">The instance on which to invoke.</param>
    /// <param name="args">The arguments to provide to the method.</param>
    /// <returns>The method's return value.</returns>
    TResult Invoke(object? target, object?[] args);
}

/// <summary>
/// Invoker using standard reflection.
/// </summary>
/// <remarks>
/// Optional arguments must be provided as <see cref="Type.Missing"/>.
/// </remarks>
/// <typeparam name="TResult">The method's return type.</typeparam>
/// <param name="method">The method to invoke.</param>
file class ReflectionInvoker<TResult>(MethodInfo method) : IInvoker<TResult>
{
    public bool SupportsMissingArguments => true;

    public TResult Invoke(object? target, object?[] args)
    {
        return (TResult)method.Invoke(target, args)!;
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
    private readonly Action? invokeAction =
        method.ReturnType == typeof(void) ? method.SafeCreateDelegate<Action>() : null;
    private readonly Func<TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.SafeCreateDelegate<Func<TResult>>() : null;

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
        method.ReturnType == typeof(void) ? method.SafeCreateDelegate<Action<T0>>() : null;
    private readonly Func<T0, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.SafeCreateDelegate<Func<T0, TResult>>() : null;

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
        method.ReturnType == typeof(void) ? method.SafeCreateDelegate<Action<T0, T1>>() : null;
    private readonly Func<T0, T1, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.SafeCreateDelegate<Func<T0, T1, TResult>>() : null;

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
        method.ReturnType == typeof(void) ? method.SafeCreateDelegate<Action<T0, T1, T2>>() : null;
    private readonly Func<T0, T1, T2, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.SafeCreateDelegate<Func<T0, T1, T2, TResult>>() : null;

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
        method.ReturnType == typeof(void) ? method.SafeCreateDelegate<Action<T0, T1, T2, T3>>() : null;
    private readonly Func<T0, T1, T2, T3, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.SafeCreateDelegate<Func<T0, T1, T2, T3, TResult>>() : null;

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
        method.ReturnType == typeof(void) ? method.SafeCreateDelegate<Action<T0, T1, T2, T3, T4>>() : null;
    private readonly Func<T0, T1, T2, T3, T4, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.SafeCreateDelegate<Func<T0, T1, T2, T3, T4, TResult>>() : null;

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
        method.ReturnType == typeof(void) ? method.SafeCreateDelegate<Action<T0, T1, T2, T3, T4, T5>>() : null;
    private readonly Func<T0, T1, T2, T3, T4, T5, TResult>? invokeFunc =
        method.ReturnType != typeof(void) ? method.SafeCreateDelegate<Func<T0, T1, T2, T3, T4, T5, TResult>>() : null;

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
        method.ReturnType == typeof(void) ? method.SafeCreateDelegate<Action<T0, T1, T2, T3, T4, T5, T6>>() : null;
    private readonly Func<T0, T1, T2, T3, T4, T5, T6, TResult>? invokeFunc =
        method.ReturnType != typeof(void)
            ? method.SafeCreateDelegate<Func<T0, T1, T2, T3, T4, T5, T6, TResult>>()
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
        method.ReturnType == typeof(void) ? method.SafeCreateDelegate<Action<T0, T1, T2, T3, T4, T5, T6, T7>>() : null;
    private readonly Func<T0, T1, T2, T3, T4, T5, T6, T7, TResult>? invokeFunc =
        method.ReturnType != typeof(void)
            ? method.SafeCreateDelegate<Func<T0, T1, T2, T3, T4, T5, T6, T7, TResult>>()
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

file static class MethodInfoExtensions
{
    public static T SafeCreateDelegate<T>(this MethodInfo method)
        where T : Delegate
    {
        if (
            method.IsStatic
            || method.DeclaringType is null
            || (!method.DeclaringType.IsValueType && method.DeclaringType != typeof(ValueType))
        )
        {
            return method.CreateDelegate<T>();
        }
        // Compiling an expression here is substantially slower than MethodInfo.CreateDelegate, but it is the only
        // mechanism (other than IL emit, which is even worse) that seems to work correctly for value types.
        var instanceType = method.DeclaringType == typeof(ValueType) ? method.ReflectedType! : method.DeclaringType;
        var instanceParam = Expression.Parameter(instanceType, "instance");
        var parameters = method.GetParameters();
        var arguments = new Expression[parameters.Length];
        var argumentsWithInstance = new ParameterExpression[parameters.Length + 1];
        argumentsWithInstance[0] = instanceParam;
        for (int i = 0; i < parameters.Length; i++)
        {
            arguments[i] = argumentsWithInstance[i + 1] = Expression.Parameter(parameters[i].ParameterType, "arg" + i);
        }
        return Expression.Lambda<T>(Expression.Call(instanceParam, method, arguments), argumentsWithInstance).Compile();
    }
}
