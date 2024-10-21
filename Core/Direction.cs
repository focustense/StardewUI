using StardewUI.Layout;

namespace StardewUI;

/// <summary>
/// Cardinal directions used in UI, matching gamepad stick/button directions for navigation.
/// </summary>
public enum Direction
{
    /// <summary>
    /// "Up" in screen space.
    /// </summary>
    North = 0,

    /// <summary>
    /// "Right" in screen space.
    /// </summary>
    East,

    /// <summary>
    /// "Down" in screen space.
    /// </summary>
    South,

    /// <summary>
    /// "Left" in screen space.
    /// </summary>
    West,
}

/// <summary>
/// Helpers for working with <see cref="Direction"/>.
/// </summary>
public static class DirectionExtensions
{
    /// <summary>
    /// Gets the orientation axis associated with a given <paramref name="direction" />, i.e. whether it flows
    /// horizontally or vertically.
    /// </summary>
    public static Orientation GetOrientation(this Direction direction)
    {
        return direction switch
        {
            Direction.North or Direction.South => Orientation.Vertical,
            _ => Orientation.Horizontal,
        };
    }

    /// <summary>
    /// Returns <c>true</c> if the specified <paramref name="direction"/> is along the horizontal (width) axis,
    /// otherwise <c>false</c>.
    /// </summary>
    public static bool IsHorizontal(this Direction direction)
    {
        return direction == Direction.East || direction == Direction.West;
    }

    /// <summary>
    /// Returns <c>true</c> if the specified <paramref name="direction"/> is along the vertical (height) axis, otherwise
    /// <c>false</c>.
    /// </summary>
    public static bool IsVertical(this Direction direction)
    {
        return direction == Direction.North || direction == Direction.South;
    }
}
