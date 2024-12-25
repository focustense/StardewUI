namespace StardewUI;

/// <summary>
/// Utilities for parsing third-party types, generally related MonoGame/XNA.
/// </summary>
public static class Parsers
{
    /// <inheritdoc cref="ParseVector2(ReadOnlySpan{char})" />
    public static Vector2 ParseVector2(string value)
    {
        return ParseVector2(value.AsSpan());
    }

    /// <summary>
    /// Parses a <see cref="Vector2"/> from a comma-separated value pair.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <returns>The parsed <see cref="Vector2"/>.</returns>
    /// <exception cref="FormatException">Thrown when <paramref name="value"/> is not a valid format.</exception>
    public static Vector2 ParseVector2(ReadOnlySpan<char> value)
    {
        return TryParseVector2(value, out var result)
            ? result
            : throw new FormatException(
                $"Invalid Vector2 string '{value}'. Vector2 strings must be of the form 'X,Y'."
            );
    }

    /// <summary>
    /// Attempts to parse a <see cref="Vector2"/> from a comma-separated value pair.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <param name="result">The result if successful, otherwise a default <see cref="Vector2"/></param>.
    /// <returns><c>true</c> if the <paramref name="value"/> was successfully parsed into <paramref name="result"/>;
    /// <c>false</c> if the parsing was unsuccessful.</returns>
    public static bool TryParseVector2(ReadOnlySpan<char> value, out Vector2 result)
    {
        var separatorIndex = value.IndexOf(',');
        if (separatorIndex < 0)
        {
            result = default;
            return false;
        }
        float x = float.Parse(value[..separatorIndex]);
        float y = float.Parse(value[(separatorIndex + 1)..]);
        result = new(x, y);
        return true;
    }
}
