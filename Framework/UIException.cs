namespace StardewUI.Framework;

/// <summary>
/// Base class for all exceptions specific to StardewUI.
/// </summary>
public class UIException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UIException"/> class.
    /// </summary>
    public UIException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UIException(string? message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIException"/> class with a specified error message and a reference
    /// to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <c>null</c> if not
    /// specified.</param>
    public UIException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
