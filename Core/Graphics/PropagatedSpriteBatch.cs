using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewUI.Graphics;

/// <summary>
/// Sprite batch wrapper with transform propagation.
/// </summary>
/// <param name="spriteBatch">The XNA/MonoGame sprite batch.</param>
/// <param name="transform">Transformation to apply.</param>
public class PropagatedSpriteBatch(SpriteBatch spriteBatch, GlobalTransform transform) : ISpriteBatch
{
    private readonly GraphicsDevice graphicsDevice = spriteBatch.GraphicsDevice;
    private readonly GraphicsState initialState = new(spriteBatch);
    private readonly SpriteBatch spriteBatch = spriteBatch;

    // To improve performance, it's best not to immediately cycle the internal SpriteBatch simply because we received
    // a new transform; instead, we can defer this to when it's actually time to draw something, in case multiple
    // transforms have been accumulated.
    private bool hasPendingTransform;
    private bool isDisposed;
    private GlobalTransform transform = transform;

    /// <summary>
    /// Initializes a new <see cref="PropagatedSpriteBatch"/> using a local transform interpreted as global.
    /// </summary>
    /// <remarks>
    /// Provided for legacy compatibility; assumes that the local transform is the outermost transform and converts it
    /// directly to a global transform.
    /// </remarks>
    /// <param name="spriteBatch">The XNA/MonoGame sprite batch.</param>
    /// <param name="transform">Transformation to apply.</param>
    public PropagatedSpriteBatch(SpriteBatch spriteBatch, Transform transform)
        : this(spriteBatch, GlobalTransform.Default.Apply(transform, TransformOrigin.Default, out _)) { }

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
        if (transform.IsRectangular())
        {
            // Collapsing the transform isn't always strictly necessary, but since we have to begin a new SpriteBatch
            // anyway in order to create the scissor rectangle, the added cost is not significant and it makes the math
            // below a lot simpler.
            transform = transform.Collapse();
            var clipPosition = Vector2.Transform(clipRect.Location.ToVector2(), transform.Matrix);
            // Unsure if it's faster to compute the opposite corner location using the same matrix and subtract to get
            // the size, or create a size-only matrix (excluding translation) to compute the size directly. Probably not
            // worth worrying about the difference.
            var clipEnd = Vector2.Transform((clipRect.Location + clipRect.Size).ToVector2(), transform.Matrix);
            var clipSize = Vector2.Ceiling((clipEnd - clipPosition));
            var transformedClipRect = new Rectangle(clipPosition.ToPoint(), clipSize.ToPoint());
            spriteBatch.End();
            BeginSpriteBatch(new() { ScissorTestEnable = true });
            spriteBatch.GraphicsDevice.ScissorRectangle = Intersection(reverter.ScissorRect, transformedClipRect);
        }
        else
        {
            // FIXME: Scissor rects are screen-relative so this is always wrong whenever there is any global transform.
            // Probably need to switch to a RenderTarget-based implementation, at least when a global transform is detected.a
            var location = (clipRect.Location.ToVector2() + transform.Local.Translation).ToPoint();
            spriteBatch.End();
            BeginSpriteBatch(new() { ScissorTestEnable = true });
            spriteBatch.GraphicsDevice.ScissorRectangle = Intersection(reverter.ScissorRect, new(location, clipRect.Size));
        }
        return reverter;
    }

    /// <inheritdoc />
    public void DelegateDraw(Action<SpriteBatch, Vector2> draw)
    {
        // Since we have no idea what the delegate is going to try to do, always collapse the transform here unless it
        // is only translation, which is the only scenario where the "offset delegate" is still valid.
        if (transform.Local.HasScale || transform.Local.HasRotation)
        {
            transform = transform.Collapse();
        }
        ApplyPendingTransform();
        draw(spriteBatch, transform.Local.Translation);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }
        isDisposed = true;
        transform = GlobalTransform.Default;
        GraphicsReverter.Revert(this, initialState);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Draw(
        Texture2D texture,
        Vector2 position,
        Rectangle? sourceRectangle,
        Color? color = null,
        float rotation = 0,
        float scale = 1.0f,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0
    )
    {
        ApplyPendingTransform(position, new Vector2(scale, scale), rotation);
        var (location, origin) = ComputeLocationAndOrigin(texture, sourceRectangle, position, scale: new(scale, scale));
        spriteBatch.Draw(
            texture,
            location,
            sourceRectangle,
            color ?? Color.White,
            rotation + transform.Local.Rotation,
            origin,
            new Vector2(scale, scale) * transform.Local.Scale,
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
        Vector2? scale,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0
    )
    {
        ApplyPendingTransform(position, scale, rotation);
        var (location, origin) = ComputeLocationAndOrigin(texture, sourceRectangle, position, scale: scale);
        spriteBatch.Draw(
            texture,
            location,
            sourceRectangle,
            color ?? Color.White,
            rotation + transform.Local.Rotation,
            origin,
            scale ?? Vector2.One * transform.Local.Scale,
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
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0
    )
    {
        ApplyPendingTransform(destinationRectangle.Location.ToVector2(), Vector2.One, rotation);
        var (location, origin) = ComputeLocationAndOrigin(
            texture,
            sourceRectangle,
            destinationRectangle.Location.ToVector2(),
            size: destinationRectangle.Size.ToVector2()
        );
        if (transform.Local.HasScale)
        {
            var sourceSize = (sourceRectangle?.Size ?? texture.Bounds.Size).ToVector2();
            float scaleX = destinationRectangle.Width / sourceSize.X;
            float scaleY = destinationRectangle.Height / sourceSize.Y;
            var scale = new Vector2(scaleX, scaleY) * transform.Local.Scale;
            spriteBatch.Draw(
                texture,
                location,
                sourceRectangle,
                color ?? Color.White,
                rotation + transform.Local.Rotation,
                origin,
                scale,
                effects,
                layerDepth
            );
        }
        else
        {
            spriteBatch.Draw(
                texture,
                new(location.ToPoint(), destinationRectangle.Size),
                sourceRectangle,
                color ?? Color.White,
                rotation + transform.Local.Rotation,
                origin,
                effects,
                layerDepth
            );
        }
    }

    /// <inheritdoc />
    public void DrawString(
        SpriteFont spriteFont,
        string text,
        Vector2 position,
        Color color,
        float rotation = 0,
        float scale = 1,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0
    )
    {
        ApplyPendingTransform(position, new Vector2(scale, scale), rotation);
        var (location, origin) = ComputeLocationAndOrigin(position, () => spriteFont.MeasureString(text));
        spriteBatch.DrawString(
            spriteFont,
            text,
            location,
            color,
            rotation + transform.Local.Rotation,
            origin,
            new Vector2(scale, scale) * transform.Local.Scale,
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
        transform = GlobalTransform.Default;
        return new RenderTargetReverter(graphicsReverter, transformReverter);
    }

    /// <inheritdoc />
    public void Transform(Transform transform, TransformOrigin? origin = null)
    {
        this.transform = this.transform.Apply(transform, origin ?? TransformOrigin.Default, out var isNewMatrix);
        hasPendingTransform |= isNewMatrix;
    }

    private void ApplyPendingTransform(Vector2? position = null, Vector2? scale = null, float? rotation = null)
    {
        if (!transform.Local.CanMergeLocally(scale ?? Vector2.One, rotation ?? 0, position ?? Vector2.Zero))
        {
            transform = transform.Collapse();
            hasPendingTransform = true;
        }
        if (!hasPendingTransform)
        {
            return;
        }
        var graphicsState = new GraphicsState(spriteBatch);
        spriteBatch.End();
        // Calling BeginSpriteBatch will implicitly use the current transform, and also clear the pending flag.
        BeginSpriteBatch(graphicsState.RasterizerState, graphicsState.BlendState);
    }

    private void BeginSpriteBatch(RasterizerState rasterizerState, BlendState? blendState = null)
    {
        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            blendState ?? BlendState.AlphaBlend,
            SamplerState.PointClamp,
            rasterizerState: rasterizerState,
            transformMatrix: transform.Matrix
        );
        hasPendingTransform = false;
    }

    private (Vector2 location, Vector2 origin) ComputeLocationAndOrigin(
        Texture2D texture,
        Rectangle? sourceRectangle,
        Vector2 location,
        Vector2? size = null,
        Vector2? scale = null
    )
    {
        if (transform.LocalOrigin == TransformOrigin.Default)
        {
            return (location + transform.Local.Translation, Vector2.Zero);
        }
        var sourceSize = (sourceRectangle?.Size ?? texture.Bounds.Size).ToVector2();
        var destSize = size ?? sourceSize * (scale ?? Vector2.One);
        return ComputeLocationAndOrigin(location, () => sourceSize, () => destSize);
    }

    private (Vector2 location, Vector2 origin) ComputeLocationAndOrigin(
        Vector2 location,
        Func<Vector2> sourceSize,
        Func<Vector2>? destSize = null
    )
    {
        if (transform.LocalOrigin == TransformOrigin.Default)
        {
            return (location + transform.Local.Translation, Vector2.Zero);
        }
        var relativeOrigin = destSize is not null
            ? transform.LocalOrigin.Absolute / destSize() * sourceSize()
            : transform.LocalOrigin.Absolute;
        var adjustedLocation = location + transform.Local.Translation + transform.LocalOrigin.Absolute;
        return (adjustedLocation, relativeOrigin);
    }

    private static Rectangle Intersection(Rectangle r1, Rectangle r2)
    {
        var left = Math.Max(r1.Left, r2.Left);
        var top = Math.Max(r1.Top, r2.Top);
        var right = Math.Min(r1.Right, r2.Right);
        var bottom = Math.Min(r1.Bottom, r2.Bottom);
        return new(left, top, Math.Max(right - left, 0), Math.Max(bottom - top, 0));
    }

    private class GraphicsState(SpriteBatch spriteBatch)
    {
        // Doing this with reflection in a draw loop sucks for performance, but there seems to be no other way to get
        // access to the previous state. `SpriteBatch.GraphcisDevice.RasterizerState` does not sync with it.
        public BlendState? BlendState { get; } = (BlendState)blendStateField.GetValue(spriteBatch)!;
        public RasterizerState RasterizerState { get; } = (RasterizerState)rasterizerStateField.GetValue(spriteBatch)!;
        public RenderTargetBinding[] RenderTargets { get; } = spriteBatch.GraphicsDevice.GetRenderTargets();
        public Rectangle ScissorRect { get; } = spriteBatch.GraphicsDevice.ScissorRectangle;

        private static readonly FieldInfo blendStateField = typeof(SpriteBatch).GetField(
            "_blendState",
            BindingFlags.Instance | BindingFlags.NonPublic
        )!;
        private static readonly FieldInfo rasterizerStateField = typeof(SpriteBatch).GetField(
            "_rasterizerState",
            BindingFlags.Instance | BindingFlags.NonPublic
        )!;
    }

    private class GraphicsReverter(PropagatedSpriteBatch owner) : IDisposable
    {
        public BlendState? BlendState => previousState.BlendState;
        public RasterizerState RasterizerState => previousState.RasterizerState;
        public RenderTargetBinding[] RenderTargets => previousState.RenderTargets;
        public Rectangle ScissorRect => previousState.ScissorRect;

        private readonly GraphicsState previousState = new(owner.spriteBatch);

        public static void Revert(PropagatedSpriteBatch target, GraphicsState previousState)
        {
            target.spriteBatch.End();
            target.graphicsDevice.SetRenderTargets(previousState.RenderTargets);
            target.BeginSpriteBatch(previousState.RasterizerState, previousState.BlendState);
            target.graphicsDevice.ScissorRectangle = previousState.ScissorRect;
        }

        public void Dispose()
        {
            Revert(owner, previousState);
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
        private readonly GlobalTransform savedTransform = owner.transform;

        public void Dispose()
        {
            if (owner.transform.Matrix != savedTransform.Matrix)
            {
                owner.hasPendingTransform = true;
            }
            owner.transform = savedTransform;
            GC.SuppressFinalize(this);
        }
    }
}
