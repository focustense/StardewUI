using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewUI.Layout;

namespace StardewUI.Events;

/// <summary>
/// Event arguments for an event relating to a button (or key) of some input device.
/// </summary>
/// <param name="position">The position of the mouse cursor when the button was pressed.</param>
/// <param name="button">The button that triggered the event.</param>
public class ButtonEventArgs(Vector2 position, SButton button)
    : PointerEventArgs(position),
        IOffsettable<ButtonEventArgs>
{
    /// <summary>
    /// The button that triggered the event.
    /// </summary>
    public SButton Button { get; } = button;

    /// <inheritdoc/>
    public new ButtonEventArgs Offset(Vector2 distance)
    {
        return new(Position + distance, Button);
    }
}
