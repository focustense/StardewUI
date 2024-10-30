using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewUI.Graphics;

/// <summary>
/// Abstraction for the <see cref="SpriteBatch"/> providing sprite-drawing methods.
/// </summary>
/// <remarks>
/// Importantly, this interface represents a "local" sprite batch with inherited transforms, so that views using it do
/// not need to be given explicit information about global coordinates.
/// </remarks>
public interface ISpriteBatch
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
        Vector2? origin = null,
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
        Vector2? origin,
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
        Vector2? origin = null,
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
        Vector2? origin = null,
        float scale = 1.0f,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0.0f
    );

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
    /// Applies a translation offset to subsequent operations.
    /// </summary>
    /// <param name="translation">The translation vector.</param>
    void Translate(Vector2 translation);

    /// <summary>
    /// Applies a translation offset to subsequent operations.
    /// </summary>
    /// <param name="x">The translation's X component.</param>
    /// <param name="y">The translation's Y component.</param>
    void Translate(float x, float y)
    {
        Translate(new(x, y));
    }
}
