using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewUI.Graphics;

/// <summary>
/// Global transform applied to an <see cref="ISpriteBatch"/>.
/// </summary>
/// <remarks>
/// <para>
/// Because the <see cref="SpriteBatch"/> in MonoGame/XNA has scale and rotation parameters for individual draw methods
/// (whether texture or text), which are presumably more optimized than computing a new global transform matrix and
/// restarting the sprite batch, the global transform maintains a current local transform and only "merges" it into the
/// transform matrix once the accumulated local transform can no longer be represented in one <see cref="Transform"/>.
/// </para>
/// <para>
/// These "local" transforms actually represent the model matrix, while the accumulated global transform is the view
/// matrix, so the relationship between them is quirky; see <see cref="Transform.CanMergeLocally(Transform)"/> for more
/// details on the process.
/// </para>
/// <para>
/// Global transforms are always "around" the viewport origin (0, 0). To use a different origin relative to the current
/// view, first translated by the negated origin position, then apply regular transforms, then translate by the
/// (positive) origin position again.
/// </para>
/// </remarks>
/// <param name="Matrix">The cumulative transformation matrix.</param>
/// <param name="Local">Local transform to apply after the <paramref name="Matrix"/> takes effect.</param>
/// <param name="LocalOrigin">Origin for the <see cref="Local"/> transform.</param>
public record GlobalTransform(Matrix Matrix, Transform Local, TransformOrigin LocalOrigin)
{
    /// <summary>
    /// The default instance, which applies no transformation.
    /// </summary>
    public static readonly GlobalTransform Default = new(Matrix.Identity, Transform.Default, TransformOrigin.Default);

    /// <summary>
    /// Applies a local transformation and returns the new, accumulated global transform.
    /// </summary>
    /// <param name="transform">The local transform to apply.</param>
    /// <param name="origin">Origin position for the <paramref name="transform"/>.</param>
    /// <param name="isNewMatrix">Whether the newly-created <see cref="GlobalTransform"/> has a different
    /// <see cref="Matrix"/> from the current instance.</param>
    /// <returns>A new <see cref="GlobalTransform"/> that combines the accumulated transformation of this instance with
    /// the specified <paramref name="transform"/>.</returns>
    public GlobalTransform Apply(Transform transform, TransformOrigin origin, out bool isNewMatrix)
    {
        isNewMatrix = false;
        if (transform == Transform.Default)
        {
            return this;
        }
        // Short-circuiting the first local transform added to any global transform helps make some simplifying
        // assumptions later, e.g. not prematurely collapsing the transform and triggering a new sprite batch when the
        // first transform has a non-default origin.
        if (Local == Transform.Default)
        {
            return new(Matrix, transform, origin);
        }
        isNewMatrix = !Local.CanMergeLocally(transform) || HasNewOrigin();
        var newOrigin = transform.IsOriginRelative ? origin : TransformOrigin.Default;
        return isNewMatrix
            ? new(MergeLocal(), transform, newOrigin)
            : new(
                Matrix,
                new(
                    Local.Scale * transform.Scale,
                    Local.Rotation + transform.Rotation,
                    Local.Translation + transform.Translation
                ),
                newOrigin
            );

        bool HasNewOrigin()
        {
            return Local.IsOriginRelative && transform.IsOriginRelative && LocalOrigin != origin;
        }
    }

    /// <summary>
    /// Merges the <see cref="Local"/> component into the global <see cref="Matrix"/>.
    /// </summary>
    /// <remarks>
    /// For use when the <see cref="Local"/> transform cannot be combined with the model transform of a specific drawing
    /// operation.
    /// </remarks>
    /// <returns>A new <see cref="GlobalTransform"/> whose <see cref="Matrix"/> is the combined <see cref="Matrix"/> and
    /// <see cref="Local"/> components of this instance, and whose <see cref="Local"/> transform is reset to the
    /// <see cref="Transform.Default"/>.</returns>
    public GlobalTransform Collapse()
    {
        return Local != Transform.Default ? new(MergeLocal(), Transform.Default, TransformOrigin.Default) : this;
    }

    private Matrix MergeLocal()
    {
        bool hasNonDefaultOrigin = LocalOrigin.Absolute != Vector2.Zero;
        // We can save on a matrix multiplication by rolling the inverse translation into the local translation before
        // converting it to a matrix.
        var localMatrix = hasNonDefaultOrigin
            ? new Transform(Local.Scale, Local.Rotation, Local.Translation + LocalOrigin.Absolute).ToMatrix()
            : Local.ToMatrix();
        if (LocalOrigin.Absolute != Vector2.Zero)
        {
            localMatrix = Matrix.CreateTranslation(new(-LocalOrigin.Absolute, 0)) * localMatrix;
        }
        return localMatrix * Matrix;
    }
}
