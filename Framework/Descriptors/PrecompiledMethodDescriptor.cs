namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Statically-typed implementation of an <see cref="IMethodDescriptor{T}"/> with predefined attributes.
/// </summary>
/// <typeparam name="TTarget">The method's declaring type.</typeparam>
/// <typeparam name="TReturn">The type of the method's return value.</typeparam>
/// <param name="name">The method name.</param>
/// <param name="argumentTypes">Types of all method parameters, including optional parameters.</param>
/// <param name="defaultValues">Default values for all optional parameters at the end of the argument list.</param>
/// <param name="invoke">Function to invoke the method on a given target with a specified argument list.</param>
public class PrecompiledMethodDescriptor<TTarget, TReturn>(
    string name,
    Type[] argumentTypes,
    object?[] defaultValues,
    Func<TTarget, object?[], object> invoke
) : IMethodDescriptor<TReturn>
{
    /// <inheritdoc />
    public ReadOnlySpan<Type> ArgumentTypes => argumentTypes;

    /// <inheritdoc />
    public Type DeclaringType => typeof(TTarget);

    /// <inheritdoc />
    public string Name => name;

    /// <inheritdoc />
    public int OptionalArgumentCount => defaultValues.Length;

    /// <inheritdoc />
    public Type ReturnType => typeof(TReturn);

    private readonly int requiredArgumentCount = argumentTypes.Length - defaultValues.Length;

    /// <inheritdoc />
    public TReturn Invoke(object? target, object?[] arguments)
    {
        if (arguments.Length == argumentTypes.Length)
        {
            return (TReturn)invoke((TTarget)target!, arguments);
        }
        if (arguments.Length > argumentTypes.Length)
        {
            throw new ArgumentException(
                $"Too many arguments to method {typeof(TTarget).FullName}.{Name} "
                    + $"(requires up to {argumentTypes.Length}, received {arguments.Length}).",
                nameof(arguments)
            );
        }
        if (arguments.Length < requiredArgumentCount)
        {
            throw new ArgumentException(
                $"Too few arguments to method {typeof(TTarget).FullName}.{Name} "
                    + $"(requires {requiredArgumentCount}, received {arguments.Length}).",
                nameof(arguments)
            );
        }

        var resolvedArguments = new object?[argumentTypes.Length];
        Array.Copy(arguments, resolvedArguments, arguments.Length);
        int optionalCount = argumentTypes.Length - arguments.Length;
        for (int i = 0; i < optionalCount; i++)
        {
            resolvedArguments[arguments.Length + i] = defaultValues[defaultValues.Length - optionalCount + i];
        }
        return (TReturn)invoke((TTarget)target!, resolvedArguments);
    }
}
