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
/// </remarks>
/// <param name="Matrix">The cumulative transformation matrix.</param>
/// <param name="Local">Local transform to apply after the <paramref name="Matrix"/> takes effect.</param>
public record GlobalTransform(Matrix Matrix, Transform Local)
{
    /// <summary>
    /// The default instance, which applies no transformation.
    /// </summary>
    public static readonly GlobalTransform Default = new(Matrix.Identity, Transform.Default);

    /// <summary>
    /// Applies a local transformation and returns the new, accumulated global transform.
    /// </summary>
    /// <param name="transform">The local transform to apply.</param>
    /// <param name="isNewMatrix">Whether the newly-created <see cref="GlobalTransform"/> has a different
    /// <see cref="Matrix"/> from the current instance.</param>
    /// <returns>A new <see cref="GlobalTransform"/> that combines the accumulated transformation of this instance with
    /// the specified <paramref name="transform"/>.</returns>
    public GlobalTransform Apply(Transform transform, out bool isNewMatrix)
    {
        isNewMatrix = false;
        if (transform == Transform.Default)
        {
            return this;
        }
        isNewMatrix = !Local.CanMergeLocally(transform);
        return isNewMatrix
            ? new(MergeLocal(), transform)
            : new(
                Matrix,
                new(
                    Local.Scale * transform.Scale,
                    Local.Rotation + transform.Rotation,
                    Local.Translation + transform.Translation
                )
            );
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
        return Local != Transform.Default ? new(MergeLocal(), Transform.Default) : this;
    }

    private Matrix MergeLocal()
    {
        return Local.ToMatrix() * Matrix;
    }
}
