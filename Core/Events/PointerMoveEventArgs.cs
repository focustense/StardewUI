using Microsoft.Xna.Framework;

namespace StardewUI;

/// <summary>
/// Event raised when a pointer moves relative to some view.
/// </summary>
/// <param name="previousPosition">The previously-tracked position of the pointer.</param>
/// <param name="position">The new pointer position.</param>
public class PointerMoveEventArgs(Vector2 previousPosition, Vector2 position)
    : PointerEventArgs(position),
        IOffsettable<PointerMoveEventArgs>
{
    public Vector2 PreviousPosition { get; } = previousPosition;

    /// <inheritdoc/>
    public new PointerMoveEventArgs Offset(Vector2 distance)
    {
        return new(PreviousPosition + distance, Position + distance);
    }
}
