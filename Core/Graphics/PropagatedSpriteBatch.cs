using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewUI.Graphics;

/// <summary>
/// Sprite batch wrapper with transform propagation.
/// </summary>
public class PropagatedSpriteBatch(SpriteBatch spriteBatch, Transform transform) : ISpriteBatch
{
    private readonly GraphicsDevice graphicsDevice = spriteBatch.GraphicsDevice;
    private readonly SpriteBatch spriteBatch = spriteBatch;
    private Transform transform = transform;

    /// <inheritdoc />
    public IDisposable Blend(BlendState blendState)
    {
        var reverter = new GraphicsReverter(this);
        spriteBatch.End();
        BeginSpriteBatch(reverter.RasterizerState, blendState);
        return reverter;
    }

    /// <inheritdoc />
    public IDisposable Clip(Rectangle clipRect)
    {
        var reverter = new GraphicsReverter(this);
        var location = (clipRect.Location.ToVector2() + transform.Translation).ToPoint();
        spriteBatch.End();
        BeginSpriteBatch(new() { ScissorTestEnable = true });
        spriteBatch.GraphicsDevice.ScissorRectangle = Intersection(reverter.ScissorRect, new(location, clipRect.Size));
        return reverter;
    }

    /// <inheritdoc />
    public void DelegateDraw(Action<SpriteBatch, Vector2> draw)
    {
        draw(spriteBatch, transform.Translation);
    }

    /// <inheritdoc />
    public void Draw(
        Texture2D texture,
        Vector2 position,
        Rectangle? sourceRectangle,
        Color? color = null,
        float rotation = 0,
        Vector2? origin = null,
        float scale = 1.0f,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0
    )
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
            layerDepth
        );
    }

    /// <inheritdoc />
    public void Draw(
        Texture2D texture,
        Vector2 position,
        Rectangle? sourceRectangle,
        Color? color,
        float rotation,
        Vector2? origin,
        Vector2? scale,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0
    )
    {
        spriteBatch.Draw(
            texture,
            position + transform.Translation,
            sourceRectangle,
            color ?? Color.White,
            rotation,
            origin ?? Vector2.Zero,
            scale ?? Vector2.One,
            effects,
            layerDepth
        );
    }

    /// <inheritdoc />
    public void Draw(
        Texture2D texture,
        Rectangle destinationRectangle,
        Rectangle? sourceRectangle,
        Color? color = null,
        float rotation = 0,
        Vector2? origin = null,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0
    )
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
            layerDepth
        );
    }

    /// <inheritdoc />
    public void DrawString(
        SpriteFont spriteFont,
        string text,
        Vector2 position,
        Color color,
        float rotation = 0,
        Vector2? origin = null,
        float scale = 1,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0
    )
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
            layerDepth
        );
    }

    /// <inheritdoc />
    public void InitializeRenderTarget([NotNull] ref RenderTarget2D? target, int width, int height)
    {
        if (target is null || target.Width != width || target.Height != height)
        {
            target?.Dispose();
            target = new(
                graphicsDevice,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents
            );
        }
    }

    /// <inheritdoc />
    public IDisposable SaveTransform()
    {
        return new TransformReverter(this);
    }

    /// <inheritdoc />
    public IDisposable SetRenderTarget(RenderTarget2D renderTarget, Color? clearColor = null)
    {
        var graphicsReverter = new GraphicsReverter(this);
        var transformReverter = new TransformReverter(this);
        spriteBatch.End();
        graphicsDevice.SetRenderTarget(renderTarget);
        if (clearColor.HasValue)
        {
            graphicsDevice.Clear(clearColor.Value);
        }
        BeginSpriteBatch(graphicsReverter.RasterizerState, graphicsReverter.BlendState);
        transform = Transform.Default;
        return new RenderTargetReverter(graphicsReverter, transformReverter);
    }

    /// <inheritdoc />
    public void Translate(float x, float y)
    {
        Translate(new(x, y));
    }

    /// <inheritdoc />
    public void Translate(Vector2 translation)
    {
        transform = transform.Translate(translation);
    }

    private void BeginSpriteBatch(RasterizerState rasterizerState, BlendState? blendState = null)
    {
        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            blendState ?? BlendState.AlphaBlend,
            SamplerState.PointClamp,
            rasterizerState: rasterizerState
        );
    }

    private static Rectangle Intersection(Rectangle r1, Rectangle r2)
    {
        var left = Math.Max(r1.Left, r2.Left);
        var top = Math.Max(r1.Top, r2.Top);
        var right = Math.Min(r1.Right, r2.Right);
        var bottom = Math.Min(r1.Bottom, r2.Bottom);
        return new(left, top, Math.Max(right - left, 0), Math.Max(bottom - top, 0));
    }

    private class GraphicsReverter(PropagatedSpriteBatch owner) : IDisposable
    {
        // Doing this with reflection in a draw loop sucks for performance, but there seems to be no other way to get
        // access to the previous state. `SpriteBatch.GraphcisDevice.RasterizerState` does not sync with it.
        public BlendState? BlendState { get; } = (BlendState)blendStateField.GetValue(owner.spriteBatch)!;
        public RasterizerState RasterizerState { get; } =
            (RasterizerState)rasterizerStateField.GetValue(owner.spriteBatch)!;
        public RenderTargetBinding[] RenderTargets { get; } = owner.graphicsDevice.GetRenderTargets();
        public Rectangle ScissorRect { get; } = owner.spriteBatch.GraphicsDevice.ScissorRectangle;

        private static readonly FieldInfo blendStateField = typeof(SpriteBatch).GetField(
            "_blendState",
            BindingFlags.Instance | BindingFlags.NonPublic
        )!;
        private static readonly FieldInfo rasterizerStateField = typeof(SpriteBatch).GetField(
            "_rasterizerState",
            BindingFlags.Instance | BindingFlags.NonPublic
        )!;

        public void Dispose()
        {
            owner.spriteBatch.End();
            owner.graphicsDevice.SetRenderTargets(RenderTargets);
            owner.BeginSpriteBatch(RasterizerState, BlendState);
            owner.graphicsDevice.ScissorRectangle = ScissorRect;
            GC.SuppressFinalize(this);
        }
    }

    private class RenderTargetReverter(GraphicsReverter graphicsReverter, TransformReverter transformReverter)
        : IDisposable
    {
        public void Dispose()
        {
            graphicsReverter.Dispose();
            transformReverter.Dispose();
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
