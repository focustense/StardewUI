namespace StardewUI.Framework.Descriptors;

/// <summary>
/// The exception that is thrown when an error occurs while reading or building the metadata for a bound view or one of
/// its data sources.
/// </summary>
public class DescriptorException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptorException"/> class.
    /// </summary>
    public DescriptorException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptorException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DescriptorException(string? message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptorException"/> class with a specified error message and a
    /// reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <c>null</c> if not
    /// specified.</param>
    public DescriptorException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
