using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SupplyChain.UI;

/// <summary>
/// Abstraction for the <see cref="SpriteBatch"/> providing sprite-drawing methods.
/// </summary>
/// <remarks>
/// Importantly, this interface represents a "local" sprite batch with inherited transforms, so that views using it do
/// not need to be given explicit information about global coordinates.
/// </remarks>
public interface ISpriteBatch
{
    /// <inheritdoc cref="SpriteBatch.Draw(Texture2D, Rectangle, Rectangle?, Color, float, Vector2, SpriteEffects, float)"/>
    void Draw(
        Texture2D texture,
        Vector2 position,
        Rectangle? sourceRectangle,
        Color? color = null,
        float rotation = 0.0f,
        Vector2? origin = null,
        float scale = 1.0f,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0.0f);

    /// <inheritdoc cref="SpriteBatch.Draw(Texture2D, Vector2, Rectangle?, Color, float, Vector2, float, SpriteEffects, float)"/>
    void Draw(
        Texture2D texture,
        Rectangle destinationRectangle,
        Rectangle? sourceRectangle,
        Color? color = null,
        float rotation = 0.0f,
        Vector2? origin = null,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0.0f);

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
        float layerDepth = 0.0f);

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
