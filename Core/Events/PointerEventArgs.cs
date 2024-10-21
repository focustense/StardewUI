using Microsoft.Xna.Framework;
using StardewUI.Layout;

namespace StardewUI.Events;

/// <summary>
/// Base class for any event involving the cursor/pointer, e.g. clicks.
/// </summary>
/// <param name="position">The position, relative to the view receiving the event, of the pointer when the event
/// occurred.</param>
public class PointerEventArgs(Vector2 position) : BubbleEventArgs, IOffsettable<PointerEventArgs>
{
    /// <summary>
    /// The position, relative to the view receiving the event, of the pointer when the event occurred.
    /// </summary>
    public Vector2 Position { get; } = position;

    /// <inheritdoc/>
    public PointerEventArgs Offset(Vector2 distance)
    {
        return new(Position + distance);
    }
}
