using Microsoft.Xna.Framework;
using StardewUI.Framework.Converters;
using System.Diagnostics.CodeAnalysis;

namespace StardewUI.Layout;

/// <summary>
/// Describes the position of a <see cref="FloatingElement"/>.
/// </summary>
/// <param name="offsetSelector">Calculates the position offset (relative to the parent) of the floating view. Takes the
/// measured floating view size, and then the parent size, as arguments.</param>
[DuckType]
public class FloatingPosition(Func<Vector2, Vector2, Vector2> offsetSelector)
{
    /// <summary>
    /// Positions the floating element immediately above the parent view, so that its bottom edge is flush with the
    /// parent's top edge.
    /// </summary>
    public static readonly FloatingPosition AboveParent = AboveParentWithOffset(Vector2.Zero);

    /// <summary>
    /// Positions the floating element immediately to the right of (after) the parent view, so that its left edge is
    /// flush with the parent's right edge.
    /// </summary>
    public static readonly FloatingPosition AfterParent = AfterParentWithOffset(Vector2.Zero);

    /// <summary>
    /// Positions the floating element immediately to the left of (before) the parent view, so that its right edge is
    /// flush with the parent's left edge.
    /// </summary>
    public static readonly FloatingPosition BeforeParent = BeforeParentWithOffset(Vector2.Zero);

    /// <summary>
    /// Positions the floating element immediately below the parent view, so that its top edge is flush with the
    /// parent's bottom edge.
    /// </summary>
    public static readonly FloatingPosition BelowParent = BelowParentWithOffset(Vector2.Zero);


    /// <summary>
    /// Parses a <see cref="FloatingPosition"/> from its string representation.
    /// </summary>
    /// <param name="text">The string value to parse.</param>
    /// <returns>The parsed position.</returns>
    /// <exception cref="FormatException">Thrown when the <paramref name="text"/> is not in a valid format.</exception>
    public static FloatingPosition Parse(string text)
    {
        return TryParse(text, out var result)
            ? result
            : throw new FormatException(
                $"Invalid floating position string '{text}'. Must be one of 'top', 'bottom', left' or 'right', " +
                "optionally followed by a semicolon and 2D coordinate in 'X, Y' format.");
    }

    /// <summary>
    /// Attempts to parse a <see cref="FloatingPosition"/> from its string representation.
    /// </summary>
    /// <param name="text">The string value to parse.</param>
    /// <param name="result">If the method returns <c>true</c>, holds the parsed position; otherwise
    /// <c>null</c>.</param>
    /// <returns><c>true</c> if the <paramref name="text"/> was successfully parsed into a position; <c>false</c> if the
    /// input was not in a valid format.</returns>
    public static bool TryParse(string text, [MaybeNullWhen(false)] out FloatingPosition result)
    {
        var span = text.AsSpan();
        var separatorIndex = span.IndexOf(';');
        var edgeSpan = (separatorIndex >= 0 ? span[..separatorIndex] : span).Trim();
        var offsetSpan = (separatorIndex >= 0) ? span[(separatorIndex + 1)..].Trim() : [];
        var offset = !offsetSpan.IsEmpty ? Vector2Converter.Parse(offsetSpan) : Vector2.Zero;
        result = edgeSpan switch
        {
            ['A' or 'a', 'B' or 'b', 'O' or 'o', 'V' or 'v', 'E' or 'e'] => AboveParentWithOffset(offset),
            ['A' or 'a', 'F' or 'f', 'T' or 't', 'E' or 'e', 'R' or 'r'] => AfterParentWithOffset(offset),
            ['B' or 'b', 'E' or 'e', 'F' or 'f', 'O' or 'o', 'R' or 'r', 'E' or 'e'] => BeforeParentWithOffset(offset),
            ['B' or 'b', 'E' or 'e', 'L' or 'l', 'O' or 'o', 'W' or 'w'] => BelowParentWithOffset(offset),
            _ => null
        };
        return result is not null;
    }

    private static FloatingPosition AboveParentWithOffset(Vector2 offset)
    {
        return new((viewSize, _) => new(offset.X, -viewSize.Y + offset.Y));
    }

    private static FloatingPosition AfterParentWithOffset(Vector2 offset)
    {
        return new((_, parentSize) => new(parentSize.X + offset.X, offset.Y));
    }

    private static FloatingPosition BeforeParentWithOffset(Vector2 offset)
    {
        return new((viewSize, _) => new(-viewSize.X + offset.X, offset.Y));
    }

    private static FloatingPosition BelowParentWithOffset(Vector2 offset)
    {
        return new((_, parentSize) => new(offset.X, parentSize.Y + offset.Y));
    }

    /// <summary>
    /// Calculates the final position of the floating view.
    /// </summary>
    /// <param name="view">The floating view to position.</param>
    /// <param name="parentView">The parent relative to which the floating view is being positioned.</param>
    /// <returns>The final position where the <paramref name="view"/> should be drawn.</returns>
    public Vector2 GetOffset(IView view, View parentView)
    {
        return GetOffset(view.OuterSize, parentView.OuterSize);
    }

    /// <summary>
    /// Calculates the relative origin position of a floating view based on its size and the size of its parent.
    /// </summary>
    /// <param name="viewSize">The size of the floating view.</param>
    /// <param name="parentViewSize">The size of the parent against which the floating element is positioned.</param>
    /// <returns>The final position where the floating element should be drawn.</returns>
    public Vector2 GetOffset(Vector2 viewSize, Vector2 parentViewSize)
    {
        return offsetSelector(viewSize, parentViewSize);
    }
}
