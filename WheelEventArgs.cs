using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Event raised when the mouse wheel/scroll wheel is activated.
/// </summary>
/// <inheritdoc cref="PointerEventArgs(Vector2)" path="/param[@name='position']"/>
/// <param name="direction">Direction of the wheel movement.</param>
public class WheelEventArgs(Vector2 position, Direction direction) : PointerEventArgs(position)
{
    /// <summary>
    /// Direction of the wheel movement.
    /// </summary>
    public Direction Direction { get; } = direction;
}
