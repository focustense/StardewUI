using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Binding;

/// <summary>
/// The exception that is thrown when an unrecoverable error happens during data binding for a view.
/// </summary>
public class BindingException : Exception
{
    /// <summary>
    /// The specific node that failed to bind, if known.
    /// </summary>
    public SNode? Node { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BindingException"/> class.
    /// </summary>
    public BindingException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BindingException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public BindingException(string? message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BindingException"/> class with a specified error message and a
    /// reference to the failed node.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="node">The specific node that failed to bind.</param>
    public BindingException(string? message, SNode node)
        : base(message)
    {
        Node = node;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BindingException"/> class with a specified error message and a
    /// reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <c>null</c> if not
    /// specified.</param>
    public BindingException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BindingException"/> class with a specified error message and
    /// references to the failed node and inner exception that are the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="node">The specific node that failed to bind.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <c>null</c> if not
    /// specified.</param>
    public BindingException(string? message, SNode node, Exception? innerException)
        : base(message, innerException)
    {
        Node = node;
    }
}
