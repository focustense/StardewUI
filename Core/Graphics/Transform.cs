using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewUI.Graphics;

/// <summary>
/// Global transform applied to an <see cref="ISpriteBatch"/>.
/// </summary>
/// <remarks>
/// <para>
/// The order of the different translation parameters reflects the actual order in which the transformations will be
/// applied in a <see cref="GlobalTransform"/>. Scaling before rotation prevents unexpected skewing, and rotating before
/// translation keeps the coordinate system intact.
/// </para>
/// <para>
/// To deliberately apply the individual operations in a different order, use separate <see cref="Transform"/>
/// instances applied in sequence, or simply compute the <see cref="Matrix"/> directly.
/// </para>
/// </remarks>
/// <param name="Scale">The scale at which to draw. <see cref="Vector2.One"/> is unity scale (i.e. no scaling).</param>
/// <param name="Rotation">2D rotation (always along Z axis) to apply, in radians.</param>
/// <param name="Translation">Translation offset for drawn content.</param>
public record Transform(Vector2 Scale, float Rotation, Vector2 Translation)
{
    /// <summary>
    /// Default instance with no transformations applied.
    /// </summary>
    public static readonly Transform Default = new(Vector2.One, 0, Vector2.Zero);

    /// <summary>
    /// Whether the current instance has a non-uniform <see cref="Scale"/>.
    /// </summary>
    /// <remarks>
    /// Non-uniform scale sometimes needs to be treated differently from uniform scale because the former is not
    /// commutative with rotation.
    /// </remarks>
    public bool HasNonUniformScale => Scale.X != Scale.Y;

    /// <summary>
    /// Whether the current instance has a non-zero <see cref="Rotation"/>.
    /// </summary>
    public bool HasRotation => Rotation != 0;

    /// <summary>
    /// Whether the current instance has non-unity <see cref="Scale"/>, regardless of uniformity.
    /// </summary>
    public bool HasScale => Scale != Vector2.One;

    /// <summary>
    /// Whether the current instance has a non-zero <see cref="Translation"/>.
    /// </summary>
    public bool HasTranslation => Translation != Vector2.Zero;

    /// <summary>
    /// Whether the current transform is affected by transform origin.
    /// </summary>
    /// <remarks>
    /// Some types of transformations, specifically translation, have outcomes independent of the transformation origin
    /// and should therefore not attempt to use it or pass it on to global transforms.
    /// </remarks>
    public bool IsOriginRelative => HasRotation || HasScale;

    /// <summary>
    /// Creates a new <see cref="Transform"/> that applies a specific 2D rotation.
    /// </summary>
    /// <param name="angle">The rotation angle, in radians.</param>
    /// <returns>A <see cref="Transform"/> whose <see cref="Rotation"/> is equal to the specified
    /// <paramref name="angle"/>.</returns>
    public static Transform FromRotation(float angle)
    {
        return new(Vector2.One, angle, Vector2.Zero);
    }

    /// <summary>
    /// Creates a <see cref="Transform"/> using a specified scale.
    /// </summary>
    /// <param name="scale">The scale to apply.</param>
    /// <returns>A <see cref="Transform"/> whose <see cref="Scale"/> is equal to the specified
    /// <paramref name="scale"/>.</returns>
    public static Transform FromScale(Vector2 scale)
    {
        return new(scale, 0f, Vector2.Zero);
    }

    /// <summary>
    /// Creates a <see cref="Transform"/> using a specified translation offset.
    /// </summary>
    /// <param name="translation">The translation offset.</param>
    /// <returns>A <see cref="Transform"/> whose <see cref="Translation"/> is equal to the specified
    /// <paramref name="translation"/>.</returns>
    public static Transform FromTranslation(Vector2 translation)
    {
        return new(Vector2.One, 0f, translation);
    }

    /// <summary>
    /// Checks if a subsequent transform can be merged into this one while preserving the result as a simple local
    /// transform, i.e. not requiring the use of a transformation matrix.
    /// </summary>
    /// <remarks>
    /// Local transforms can be merged if they are either:
    /// <list type="bullet">
    /// <item>
    /// Mathematically commutative with the existing properties, such as uniform scaling, or additional translation on a
    /// local transform that is <em>only</em> translation; or
    /// </item>
    /// <item>
    /// Following the same transformation order that applies during the various <see cref="SpriteBatch.Draw"/> and
    /// <see cref="SpriteBatch.DrawString"/> methods, i.e. <see cref="Scale"/> followed by <see cref="Rotation"/>
    /// followed by <see cref="Translation"/>. For example, if the current instance has <see cref="Scale"/> and
    /// <see cref="Rotation"/>, and the <paramref name="next"/> transform has <c>Rotation</c> only, then the rotations
    /// can be trivially summed.
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="next">The local transformation to apply after the current instance.</param>
    /// <returns><c>true</c> if the cumulative sequence of transformations can continue to be represented as a simple
    /// local <see cref="Transform"/>; <c>false</c> if the combination requires converting <see cref="ToMatrix"/> and
    /// subsequent inclusion into a new <see cref="GlobalTransform"/>.</returns>
    public bool CanMergeLocally(Transform next)
    {
        // The implementation is somewhat easier to follow mathematically than linguistically, taking into account the
        // most important fact that the actual matrix multiplications happen in reverse order; i.e. to apply Scale, then
        // Rotation, then Transformation, we multiply T * R * S. Also note, however, that the local transform is the
        // model matrix while the global transform is treated (in XNA terms) as the view matrix. See e.g.
        // https://stackoverflow.com/a/6406274/38360. This means the multiplication order of entire matrices is opposite
        // to what we expect, even though the intra-transform order is correct.
        //
        // The question we're answering here, therefore, is whether we can combine this local transform (S1, R1, T1)
        // with a next transform (S2, R2, T2) such that `T1*R1*S1*T2*R2*S2 == T1*T2*R1*R2*S1*S2`.
        //
        // From this it's relatively simple to reason about:
        //
        // 1. Translations are commutative with each other, but not with rotation or scale. Therefore, T2 can only be
        //    folded into T1 if R1 and S1 are Identity. In other words, if `next` has translation, and `this` has either
        //    non-unity scale or non-zero rotation (or both), merging is NOT allowed.
        //
        // 2. Scale is only commutative with rotation if it is uniform. Therefore, assuming T2 is identity (i.e. we
        //    haven't broken rule 1 above), R1*S1*R2*S2 == R1*R2*S1*S2 only if S1*R2 == R2*S1, i.e. if S1 is uniform or
        //    if R2 is Identity. If `this` has non-uniform scale and `next` has any rotation, merging is NOT allowed.
        //
        // If both of the constraints above are respected, then the transformations can be combined by multiplying the
        // scale and rotation components, and summing the translation components.
        return !(next.HasTranslation && (HasScale || HasRotation)) && !(next.HasRotation && HasNonUniformScale);
    }

    /// <inheritdoc cref="CanMergeLocally(Transform)" path="//*[self::remarks|self::returns]"/>
    /// <summary>
    /// Checks if a subsequent transform, represented by its individual components, can be merged into this one while
    /// preserving the result as a simple local transform, i.e. not requiring the use of a transformation matrix.
    /// </summary>
    /// <param name="scale">The <see cref="Scale"/> component of the next transform.</param>
    /// <param name="rotation">The <see cref="Rotation"/> component of the next transform.</param>
    /// <param name="translation">The <see cref="Translation"/> component of the next transform.</param>
    public bool CanMergeLocally(Vector2 scale, float rotation, Vector2 translation)
    {
        return !(translation != Vector2.Zero && (HasScale || HasRotation)) && !(rotation != 0 && HasNonUniformScale);
    }

    /// <summary>
    /// Creates a transformation matrix from the properties of this transform.
    /// </summary>
    /// <remarks>
    /// The created matrix, when used with <see cref="SpriteBatch.Begin"/>, will have the same effect as if the current
    /// <see cref="Scale"/>, <see cref="Rotation"/> and <see cref="Translation"/> were to be provided directly as
    /// arguments to the sprite or text drawing method(s).
    /// </remarks>
    /// <returns>A transformation matrix equivalent to this transform.</returns>
    public Matrix ToMatrix()
    {
        var matrix = Matrix.Identity;
        if (HasScale)
        {
            matrix *= Matrix.CreateScale(new Vector3(Scale, 1));
        }
        if (HasRotation)
        {
            matrix *= Matrix.CreateRotationZ(Rotation);
        }
        if (HasTranslation)
        {
            matrix *= Matrix.CreateTranslation(new(Translation, 0));
        }
        return matrix;
    }
}
