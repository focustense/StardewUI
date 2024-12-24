using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewUI.Graphics;

using LocalTransform = Transform;

/// <summary>
/// Abstraction for the <see cref="SpriteBatch"/> providing sprite-drawing methods.
/// </summary>
/// <remarks>
/// Importantly, this interface represents a "local" sprite batch with inherited transforms, so that views using it do
/// not need to be given explicit information about global coordinates.
/// </remarks>
public interface ISpriteBatch : IDisposable
{
    /// <summary>
    /// Sets up subsequent draw calls to use the designated blending settings.
    /// </summary>
    /// <param name="blendState">Blend state determining the color/alpha blend behavior.</param>
    /// <returns>A disposable instance which, when disposed, will revert to the previous blending state.</returns>
    IDisposable Blend(BlendState blendState);

    /// <summary>
    /// Sets up subsequent draw calls to clip contents within the specified bounds.
    /// </summary>
    /// <param name="clipRect">The clipping bounds in local coordinates.</param>
    /// <returns>A disposable instance which, when disposed, will revert to the previous clipping state.</returns>
    IDisposable Clip(Rectangle clipRect);

    /// <summary>
    /// Draws using a delegate action on a concrete <see cref="SpriteBatch"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Delegation is provided as a fallback for game-specific "utilities" that require a <see cref="SpriteBatch"/> and
    /// are not trivial to reimplement; the method acts as a bridge between the abstract <see cref="ISpriteBatch"/> and
    /// the concrete-dependent logic.
    /// </para>
    /// <para>
    /// Most view types shouldn't use this; it is only needed for a few niche features like
    /// <see cref="StardewValley.BellsAndWhistles.SpriteText"/>.
    /// </para>
    /// </remarks>
    /// <param name="draw">A function that accepts an underlying <see cref="SpriteBatch"/> as well as the transformed
    /// (global/screen) position and draws using that position as the origin (top left).</param>
    void DelegateDraw(Action<SpriteBatch, Vector2> draw);

    /// <inheritdoc cref="SpriteBatch.Draw(Texture2D, Vector2, Rectangle?, Color, float, Vector2, float, SpriteEffects, float)"/>
    void Draw(
        Texture2D texture,
        Vector2 position,
        Rectangle? sourceRectangle,
        Color? color = null,
        float rotation = 0.0f,
        float scale = 1.0f,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0.0f
    );

    /// <inheritdoc cref="SpriteBatch.Draw(Texture2D, Vector2, Rectangle?, Color, float, Vector2, Vector2, SpriteEffects, float)"/>
    void Draw(
        Texture2D texture,
        Vector2 position,
        Rectangle? sourceRectangle,
        Color? color,
        float rotation,
        Vector2? scale,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0.0f
    );

    /// <inheritdoc cref="SpriteBatch.Draw(Texture2D, Vector2, Rectangle?, Color, float, Vector2, float, SpriteEffects, float)"/>
    void Draw(
        Texture2D texture,
        Rectangle destinationRectangle,
        Rectangle? sourceRectangle,
        Color? color = null,
        float rotation = 0.0f,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0.0f
    );

    /// <inheritdoc cref="SpriteBatch.DrawString(SpriteFont, string, Vector2, Color, float, Vector2, float, SpriteEffects, float)"/>
    void DrawString(
        SpriteFont spriteFont,
        string text,
        Vector2 position,
        Color color,
        float rotation = 0.0f,
        float scale = 1.0f,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0.0f
    );

    /// <summary>
    /// Initializes a <see cref="RenderTarget2D"/> for use with <see cref="SetRenderTarget"/>.
    /// </summary>
    /// <remarks>
    /// This will reuse an existing render target if available, i.e. if <paramref name="target"/> is not <c>null</c>
    /// and matches the specified <paramref name="width"/> and <paramref name="height"/>; otherwise it will replace any
    /// previous <paramref name="target"/> and replace it with a new instance.
    /// </remarks>
    /// <param name="target">The previous render target, if any, to reuse if possible.</param>
    /// <param name="width">The target width.</param>
    /// <param name="height">The target height.</param>
    void InitializeRenderTarget([NotNull] ref RenderTarget2D? target, int width, int height);

    /// <summary>
    /// Applies a rotation transformation to subsequent operations.
    /// </summary>
    /// <param name="angle">The rotation angle, in radians.</param>
    /// <param name="origin">The center of the rotation, or <c>null</c> to use the <see cref="TransformOrigin.Default"/>
    /// origin.</param>
    void Rotate(float angle, TransformOrigin? origin = null)
    {
        Transform(LocalTransform.FromRotation(angle), origin);
    }

    /// <summary>
    /// Saves the current transform, so that it can later be restored to its current state.
    /// </summary>
    /// <remarks>
    /// This is typically used in hierarchical layout; i.e. a view with children would apply a transform before handing
    /// the canvas or sprite batch down to any of those children, and then restore it after the child is done with it.
    /// This enables a single <see cref="ISpriteBatch"/> instance to be used for the entire layout rather than having to
    /// create a tree.
    /// </remarks>
    /// <returns>A disposable instance which, when disposed, restores the transform of this <see cref="ISpriteBatch"/>
    /// to the same state it was in before <c>SaveTransform</c> was called.</returns>
    IDisposable SaveTransform();

    /// <summary>
    /// Applies a uniform scale transformation to subsequent operations.
    /// </summary>
    /// <param name="scale">Amount to scale, both horizontally and vertically. <c>1</c> is unity scale.</param>
    /// <param name="origin">The center of the scaling, or <c>null</c> to use the <see cref="TransformOrigin.Default"/>
    /// origin.</param>
    void Scale(float scale, TransformOrigin? origin = null)
    {
        Transform(LocalTransform.FromScale(new(scale, scale)), origin);
    }

    /// <summary>
    /// Applies a scale transformation to subsequent operations.
    /// </summary>
    /// <param name="x">Amount of horizontal scaling. <c>1</c> is unity scale.</param>
    /// <param name="y">Amount of vertical scaling. <c>1</c> is unity scale.</param>
    /// <param name="origin">The center of the scaling, or <c>null</c> to use the <see cref="TransformOrigin.Default"/>
    /// origin.</param>
    void Scale(float x, float y, TransformOrigin? origin = null)
    {
        Transform(LocalTransform.FromScale(new(x, y)), origin);
    }

    /// <summary>
    /// Applies a scale transformation to subsequent operations.
    /// </summary>
    /// <param name="scale">Scaling vector containing the horizontal (<see cref="Vector2.X"/>) and vertical
    /// (<see cref="Vector2.Y"/>) scaling amounts.</param>
    /// <param name="origin">The center of the scaling, or <c>null</c> to use the <see cref="TransformOrigin.Default"/>
    /// origin.</param>
    void Scale(Vector2 scale, TransformOrigin? origin = null)
    {
        Transform(LocalTransform.FromScale(scale), origin);
    }

    /// <summary>
    /// Sets up subsequent draw calls to use a custom render target.
    /// </summary>
    /// <remarks>
    /// This will also reset any active transforms for the new render target, e.g. those resulting from
    /// <see cref="Translate(Vector2)"/>. Previously-active transforms will be restored when the render target is
    /// reverted by calling <see cref="IDisposable.Dispose"/> on the result.
    /// </remarks>
    /// <param name="renderTarget">The new render target.</param>
    /// <param name="clearColor">Color to clear the <paramref name="renderTarget"/> with after making it active, or
    /// <c>null</c> to skip clearing.</param>
    /// <returns>A disposable instance which, when disposed, will revert to the previous render target(s).</returns>
    IDisposable SetRenderTarget(RenderTarget2D renderTarget, Color? clearColor = null);

    /// <summary>
    /// Applies an arbitrary transformation to subsequent operations.
    /// </summary>
    /// <param name="transform">The transform properties (scale, rotation and translation).</param>
    /// <param name="origin">The origin (i.e. center) of the transformation, or <c>null</c> to use the
    /// <see cref="TransformOrigin.Default"/> origin.</param>
    void Transform(LocalTransform transform, TransformOrigin? origin = null);

    /// <summary>
    /// Applies a translation transformation to subsequent operations.
    /// </summary>
    /// <param name="translation">The translation vector.</param>
    void Translate(Vector2 translation)
    {
        Transform(LocalTransform.FromTranslation(translation));
    }

    /// <summary>
    /// Applies a translation transformation to subsequent operations.
    /// </summary>
    /// <param name="x">The translation's X component.</param>
    /// <param name="y">The translation's Y component.</param>
    void Translate(float x, float y)
    {
        Transform(LocalTransform.FromTranslation(new(x, y)));
    }
}
