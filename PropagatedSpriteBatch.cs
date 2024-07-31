using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewUI;

/// <summary>
/// Sprite batch wrapper with transform propagation.
/// </summary>
public class PropagatedSpriteBatch(SpriteBatch spriteBatch, Transform transform) : ISpriteBatch
{
    private readonly SpriteBatch spriteBatch = spriteBatch;
    private Transform transform = transform;

    public IDisposable Clip(Rectangle clipRect)
    {
        var previousRect = spriteBatch.GraphicsDevice.ScissorRectangle;
        var previousRasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
        var location = (clipRect.Location.ToVector2() + transform.Translation).ToPoint();
        spriteBatch.End();
        BeginSpriteBatch(new() { ScissorTestEnable = true });
        spriteBatch.GraphicsDevice.ScissorRectangle = new(location, clipRect.Size);
        return new ClipReverter(this, previousRasterizerState, previousRect);
    }

    public void DelegateDraw(Action<SpriteBatch, Vector2> draw)
    {
        draw(spriteBatch, transform.Translation);
    }

    public void Draw(
        Texture2D texture,
        Vector2 position,
        Rectangle? sourceRectangle,
        Color? color = null,
        float rotation = 0,
        Vector2? origin = null,
        float scale = 1.0f,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0)
    {
        spriteBatch.Draw(
            texture,
            position + transform.Translation,
            sourceRectangle,
            color ?? Color.White,
            rotation,
            origin ?? Vector2.Zero,
            scale,
            effects,
            layerDepth);
    }

    public void Draw(
        Texture2D texture,
        Rectangle destinationRectangle,
        Rectangle? sourceRectangle,
        Color? color = null,
        float rotation = 0,
        Vector2? origin = null,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0)
    {
        var location = (destinationRectangle.Location.ToVector2() + transform.Translation).ToPoint();
        spriteBatch.Draw(
            texture,
            new(location, destinationRectangle.Size),
            sourceRectangle,
            color ?? Color.White,
            rotation,
            origin ?? Vector2.Zero,
            effects,
            layerDepth);
    }

    public void DrawString(
        SpriteFont spriteFont,
        string text,
        Vector2 position,
        Color color,
        float rotation = 0,
        Vector2? origin = null,
        float scale = 1,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0)
    {
        spriteBatch.DrawString(
            spriteFont,
            text,
            position + transform.Translation,
            color,
            rotation,
            origin ?? Vector2.Zero,
            scale,
            effects,
            layerDepth);
    }

    public IDisposable SaveTransform()
    {
        return new TransformReverter(this);
    }

    public void Translate(float x, float y)
    {
        Translate(new(x, y));
    }

    public void Translate(Vector2 translation)
    {
        transform = transform.Translate(translation);
    }

    private void BeginSpriteBatch(RasterizerState rasterizerState)
    {
        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            rasterizerState: rasterizerState);
    }

    private class ClipReverter(
        PropagatedSpriteBatch owner,
        RasterizerState previousRasterizerState,
        Rectangle previousRect)
        : IDisposable
    {
        public void Dispose()
        {
            owner.spriteBatch.End();
            owner.BeginSpriteBatch(previousRasterizerState);
            owner.spriteBatch.GraphicsDevice.ScissorRectangle = previousRect;
            GC.SuppressFinalize(this);
        }
    }

    private class TransformReverter(PropagatedSpriteBatch owner) : IDisposable
    {
        private readonly Transform savedTransform = owner.transform;

        public void Dispose()
        {
            owner.transform = savedTransform;
            GC.SuppressFinalize(this);
        }
    }
}
