namespace StardewUITest.StarML;

/// <summary>
/// Value source that uses the literal (text) value of an attribute.
/// </summary>
/// <param name="value">The attribute value.</param>
public class LiteralValueSource(string value) : IValueSource<string>
{
    public string Value => value;

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