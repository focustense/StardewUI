using Microsoft.Xna.Framework;
using StardewUI.Layout;

namespace StardewUI.Events;

/// <summary>
/// Event arguments for pointer movement relative to some view.
/// </summary>
/// <param name="previousPosition">The previously-tracked position of the pointer.</param>
/// <param name="position">The new pointer position.</param>
public class PointerMoveEventArgs(Vector2 previousPosition, Vector2 position)
    : PointerEventArgs(position),
        IOffsettable<PointerMoveEventArgs>
{
    /// <summary>
    /// The previously-tracked position of the pointer.
    /// </summary>
    public Vector2 PreviousPosition { get; } = previousPosition;

    /// <inheritdoc/>
    public new PointerMoveEventArgs Offset(Vector2 distance)
    {
        return new(PreviousPosition + distance, Position + distance);
    }
}
