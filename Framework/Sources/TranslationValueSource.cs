using StardewUI.Framework.Content;

namespace StardewUI.Framework.Sources;

/// <summary>
/// Value source that reads the localized string from a translation key.
/// </summary>
/// <param name="scope">The scope providing access to translation values.</param>
/// <param name="key">The translation key.</param>
public class TranslationValueSource(IResolutionScope scope, string key) : IValueSource<string>
{
    /// <inheritdoc />
    public bool CanRead => true;

    /// <inheritdoc />
    public bool CanWrite => false;

    /// <inheritdoc />
    public string DisplayName => $"Translation#{key}";

    /// <inheritdoc />
    public string? Value
    {
        get => value;
        set => throw new NotSupportedException($"Writing to a {typeof(TranslationValueSource).Name} is not supported.");
    }

    /// <inheritdoc />
    public Type ValueType => throw new NotImplementedException();

    /// <inheritdoc />
    object? IValueSource.Value
    {
        get => Value;
        set => throw new NotSupportedException($"Writing to a {typeof(TranslationValueSource).Name} is not supported.");
    }

    private readonly string value = scope.GetTranslationValue(key);

    /// <inheritdoc />
    public bool Update()
    {
        return false;
    }
}
