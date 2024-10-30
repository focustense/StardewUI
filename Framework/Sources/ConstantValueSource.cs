namespace StardewUI.Framework.Sources;

/// <summary>
/// Value source with a constant value, generally used to hold the literal (text) value of an attribute.
/// </summary>
/// <param name="value">The attribute value.</param>
public class ConstantValueSource<T>(T value) : IValueSource<T>
{
    /// <inheritdoc />
    public bool CanRead => true;

    /// <inheritdoc />
    public bool CanWrite => false;

    /// <inheritdoc />
    public string DisplayName => $"\"{Value}\"";

    /// <inheritdoc />
    public T? Value
    {
        get => value;
        set => throw new NotSupportedException($"Writing to a {typeof(ConstantValueSource<>).Name} is not supported.");
    }

    object? IValueSource.Value
    {
        get => Value;
        set => Value = value is not null ? (T)value : default;
    }

    /// <inheritdoc />
    public Type ValueType => typeof(T);

    /// <inheritdoc />
    /// <remarks>
    /// As implemented on <see cref="ConstantValueSource{T}"/>, always returns <c>false</c> as there can never be any
    /// change that requires an update.
    /// </remarks>
    public bool Update()
    {
        return false;
    }
}
