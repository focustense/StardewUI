namespace StardewUI.Framework.Sources;

/// <summary>
/// Value source with a constant value, generally used to hold the literal (text) value of an attribute.
/// </summary>
/// <param name="value">The attribute value.</param>
public class ConstantValueSource<T>(T value) : IValueSource<T>
{
    public bool CanRead => true;

    public bool CanWrite => false;

    public string DisplayName => $"\"{Value}\"";

    public T? Value
    {
        get => value;
        set => throw new NotSupportedException($"Writing to a {typeof(ConstantValueSource<>).Name} is not supported.");
    }

    public Type ValueType => typeof(T);

    /// <inheritdoc />
    /// <remarks>
    /// As implemented on <see cref="ConstantValueSource"/>, always returns <c>false</c> as there can never be any change
    /// that requires an update.
    /// </remarks>
    public bool Update()
    {
        return false;
    }
}
