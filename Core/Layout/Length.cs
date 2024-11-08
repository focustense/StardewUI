namespace StardewUI.Layout;

/// <summary>
/// Specifies how to calculate the length of a single dimension (width or height).
/// </summary>
/// <param name="Type">Specifies how to interpret the <see cref="Value"/>.</param>
/// <param name="Value">The dimension value, with behavior determined by <see cref="Type"/>.</param>
[DuckType]
public readonly record struct Length(LengthType Type, float Value)
{
    /// <summary>
    /// Creates a new <see cref="Length"/> having <see cref="LengthType.Content"/>.
    /// </summary>
    public static Length Content()
    {
        return new(LengthType.Content, 0);
    }

    /// <summary>
    /// Parses a <see cref="Length"/> from its string representation.
    /// </summary>
    /// <param name="value">The string representation of the <see cref="Length"/>.</param>
    /// <returns>The parsed <see cref="Length"/>.</returns>
    /// <exception cref="FormatException">Thrown when <paramref name="value"/> is not in a recognized
    /// format.</exception>
    public static Length Parse(ReadOnlySpan<char> value)
    {
        return value switch
        {
            "content" => Content(),
            "stretch" => Stretch(),
            [.., 'p', 'x'] => Px(float.Parse(value[..^2])),
            [.., '%'] => Percent(float.Parse(value[..^1])),
            _ => throw new FormatException(
                $"Invalid length '{value}'. "
                    + "Must be one of: 'content', 'stretch', or a number followed by 'px' or '%'."
            ),
        };
    }

    /// <inheritdoc cref="Parse(ReadOnlySpan{char})" />
    public static Length Parse(string value)
    {
        return Parse(value.AsSpan());
    }

    /// <summary>
    /// Creates a new <see cref="Length"/> having <see cref="LengthType.Percent"/> and the specified percent size.
    /// </summary>
    /// <param name="value">The length in 100-based percent units (e.g. <c>50.0</c> is 50%).</param>
    public static Length Percent(float value)
    {
        return new(LengthType.Percent, value);
    }

    /// <summary>
    /// Creates a new <see cref="Length"/> having <see cref="LengthType.Px"/> and the specified pixel size.
    /// </summary>
    /// <param name="value">The length in pixels.</param>
    public static Length Px(float value)
    {
        return new(LengthType.Px, value);
    }

    /// <summary>
    /// Creates a new <see cref="Length"/> having <see cref="LengthType.Stretch"/>.
    /// </summary>
    public static Length Stretch()
    {
        return new(LengthType.Stretch, 0);
    }

    /// <summary>
    /// Resolves an actual (pixel) length.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a convenience method for common layout scenarios, where content length is relatively simple to compute.
    /// Its use is optional; complex widgets can use any means they prefer to compute <see cref="View.ContentSize"/>.
    /// </para>
    /// <para>
    /// The result is intentionally not constrained to <paramref name="availableLength"/>, which is only used for the
    /// <see cref="LengthType.Stretch"/> method. This allows callers to check if the bounds were exceeded (e.g. to
    /// render a scroll bar, ellipsis, etc.) before clamping it.
    /// </para>
    /// </remarks>
    /// <param name="availableLength">The remaining space available.</param>
    /// <param name="getContentLength">A function to get the length of inner content. Will not be called unless the
    /// <see cref="LengthType"/> requires it.</param>
    /// <returns></returns>
    public readonly float Resolve(float availableLength, Func<float> getContentLength)
    {
        return Type switch
        {
            LengthType.Px => Value,
            LengthType.Percent => availableLength * Value / 100,
            LengthType.Stretch => availableLength,
            LengthType.Content => getContentLength(),
            _ => throw new NotImplementedException($"Invalid length type: {Type}"),
        };
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Type switch
        {
            LengthType.Content => "content",
            LengthType.Stretch => "stretch",
            LengthType.Px => $"{Value}px",
            LengthType.Percent => $"{Value}%",
            _ => $"({Type}, {Value})",
        };
    }
}

/// <summary>
/// Types of length calculation available for a <see cref="Length"/>.
/// </summary>
/// <remarks>
/// For all types, content may overflow or be clipped if the available size is not large enough.
/// </remarks>
public enum LengthType
{
    /// <summary>
    /// Ignore the specified <see cref="Length.Value"/> and use a value just high enough to fit all content.
    /// </summary>
    Content,

    /// <summary>
    /// Use the exact <see cref="Length.Value"/> specified, in pixels.
    /// </summary>
    Px,

    /// <summary>
    /// Use the specified <see cref="Length.Value"/> as a percentage of the available width/height.
    /// </summary>
    Percent,

    /// <summary>
    /// Ignore the specified <see cref="Length.Value"/> and stretch to the full available width/height.
    /// </summary>
    Stretch,
}
