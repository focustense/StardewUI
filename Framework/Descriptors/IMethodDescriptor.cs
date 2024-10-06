namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Describes a single method on some type.
/// </summary>
public interface IMethodDescriptor : IMemberDescriptor
{
    /// <summary>
    /// The exact types expected for the method's arguments.
    /// </summary>
    ReadOnlySpan<Type> ArgumentTypes { get; }

    /// <summary>
    /// The number of optional arguments at the end of the argument list.
    /// </summary>
    /// <remarks>
    /// Optional arguments can be provided with <see cref="Type.Missing"/> in order to ignore them in the invocation.
    /// </remarks>
    int OptionalArgumentCount { get; }

    /// <summary>
    /// The method's return type.
    /// </summary>
    Type ReturnType { get; }
}

/// <summary>
/// Describes a single method on some type, and provides a wrapper method to invoke it.
/// </summary>
/// <typeparam name="T">The return type of the described method.</typeparam>
public interface IMethodDescriptor<T> : IMethodDescriptor
{
    /// <summary>
    /// Invokes the underlying method.
    /// </summary>
    /// <param name="target">The object instance on which to invoke the method.</param>
    /// <param name="arguments">The arguments to provide to the method.</param>
    /// <returns>The return value.</returns>
    T Invoke(object? target, object?[] arguments);
}
