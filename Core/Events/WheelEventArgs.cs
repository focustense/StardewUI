using Microsoft.Xna.Framework;
using StardewUI.Layout;

namespace StardewUI.Events;

/// <summary>
/// Event arguments for mouse wheel/scroll wheel actions.
/// </summary>
/// <inheritdoc cref="PointerEventArgs(Vector2)" path="/param[@name='position']"/>
/// <param name="direction">Direction of the wheel movement.</param>
public class WheelEventArgs(Vector2 position, Direction direction)
    : PointerEventArgs(position),
        IOffsettable<WheelEventArgs>
{
    /// <summary>
    /// Direction of the wheel movement.
    /// </summary>
    public Direction Direction { get; } = direction;

    /// <inheritdoc/>
    public new WheelEventArgs Offset(Vector2 distance)
    {
        return new(Position + distance, Direction);
    }
}
