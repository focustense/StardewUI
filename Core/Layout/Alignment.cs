namespace StardewUI.Layout;

/// <summary>
/// Specifies an alignment (horizontal or vertical) for text or other layout.
/// </summary>
public enum Alignment
{
    /// <summary>
    /// Align to the start of the available space - horizontal left or vertical top.
    /// </summary>
    Start,

    /// <summary>
    /// Align to the middle of the available space.
    /// </summary>
    Middle,

    /// <summary>
    /// Align to the end of the available space - horizontal right or vertical bottom.
    /// </summary>
    End,
}

/// <summary>
/// Common helpers for <see cref="Alignment"/>.
/// </summary>
public static class AlignmentExtensions
{
    /// <summary>
    /// Applies an alignment to an axis starting at position 0.
    /// </summary>
    /// <param name="alignment">The alignment type.</param>
    /// <param name="contentLength">The length (width or height) of the content to align.</param>
    /// <param name="axisLength">The total space (width or height) available for the content.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static float Align(this Alignment alignment, float contentLength, float axisLength)
    {
        return alignment switch
        {
            Alignment.Start => 0.0f,
            Alignment.Middle => (axisLength - contentLength) / 2,
            Alignment.End => axisLength - contentLength,
            _ => throw new NotImplementedException($"Invalid horizontal alignment: {alignment}"),
        };
    }
}
