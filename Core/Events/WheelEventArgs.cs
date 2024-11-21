using Microsoft.Xna.Framework;
using StardewUI.Layout;

namespace StardewUI.Events;

/// <summary>
/// Event arguments for mouse wheel/scroll wheel actions.
/// </summary>
/// <inheritdoc cref="PointerEventArgs(Vector2)" path="/param[@name='position']"/>
/// <param name="direction">Direction of the wheel movement.</param>
public class WheelEventArgs(
#pragma warning disable 1573
    Vector2 position,
#pragma warning restore 1573
    Direction direction) : PointerEventArgs(position), IOffsettable<WheelEventArgs>
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
