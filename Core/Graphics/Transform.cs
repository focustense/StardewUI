using Microsoft.Xna.Framework;

namespace StardewUI.Graphics;

/// <summary>
/// Global transform applied to an <see cref="ISpriteBatch"/>.
/// </summary>
/// <remarks>
/// Currently only propagates the translation, since this is comparatively trivial to implement, doesn't require any
/// matrix math and it is very rare for UI to have rotation or scale that needs to propagate.
/// </remarks>
public class Transform
{
    /// <summary>
    /// Creates a <see cref="Transform"/> using a specified translation offset.
    /// </summary>
    /// <param name="translation">The translation offset.</param>
    /// <returns>A <see cref="Transform"/> whose <see cref="Translation"/> is equal to
    /// <paramref name="translation"/>.</returns>
    public static Transform FromTranslation(Vector2 translation)
    {
        return new Transform { Translation = translation };
    }

    // Constructor is private in case we decide later on to extend this to full matrix propagation.
    //
    // Conventionally, single-aspect transforms use named factory methods instead, since the parameter lists in
    // constructor overloads could otherwise be the same - e.g. translation and scale are both 2D vectors.
    //
    // In a matrix-backed version, the actual constructor would generally accept the matrix itself.
    private Transform() { }

    /// <summary>
    /// The translation vector, i.e. global X/Y origin position.
    /// </summary>
    public Vector2 Translation { get; init; } = Vector2.Zero;

    /// <summary>
    /// Applies a specified translation.
    /// </summary>
    /// <param name="translation">The translation vector.</param>
    /// <returns>A new <see cref="Transform"/> with the specified translation added to any previous transform.</returns>
    public Transform Translate(Vector2 translation)
    {
        return new Transform { Translation = Translation + translation };
    }
}
