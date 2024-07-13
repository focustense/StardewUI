using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Base class for any event involving the cursor/pointer, e.g. clicks.
/// </summary>
/// <param name="position">The position, relative to the view receiving the event, of the pointer when the event
/// occurred.</param>
public class PointerEventArgs(Vector2 position) : BubbleEventArgs
{
    /// <summary>
    /// The position, relative to the view receiving the event, of the pointer when the event occurred.
    /// </summary>
    public Vector2 Position { get; } = position;
}