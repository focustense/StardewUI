namespace StardewUI.Framework.Sources;

/// <summary>
/// Value source that uses the literal (text) value of an attribute.
/// </summary>
/// <param name="value">The attribute value.</param>
public class LiteralValueSource(string value) : IValueSource<string>
{
    public bool CanRead => true;

    public bool CanWrite => false;

    public string DisplayName => $"\"{Value}\"";

    public string? Value
    {
        get => value;
        set => throw new NotSupportedException($"Writing to a {typeof(LiteralValueSource).Name} is not supported.");
    }

    /// <inheritdoc />
    /// <remarks>
    /// As implemented on <see cref="LiteralValueSource"/>, always returns <c>false</c> as there can never be any change
    /// that requires an update.
    /// </remarks>
    public bool Update()
    {
        return false;
    }
}
