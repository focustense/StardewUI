﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewUI.Graphics;

namespace StardewUI.Framework.Tests;

internal interface ISpriteBatchLogEntry { }

internal record BlendInfo(BlendState BlendState) : ISpriteBatchLogEntry;

internal record ClipInfo(Rectangle ClipRect) : ISpriteBatchLogEntry;

internal record DrawStringInfo(
    SpriteFont SpriteFont,
    string Text,
    Vector2 Position,
    Color Color,
    float Rotation = 0,
    Vector2? Origin = null,
    float Scale = 1,
    SpriteEffects Effects = SpriteEffects.None,
    float LayerDepth = 0
) : ISpriteBatchLogEntry;

internal record DrawTextureInfo(
    Texture2D Texture,
    Vector2? Position = null,
    Rectangle? DestinationRectangle = null,
    Rectangle? SourceRectangle = null,
    Color? Color = null,
    float Rotation = 1,
    Vector2? Origin = null,
    Vector2? Scale = null,
    SpriteEffects Effects = SpriteEffects.None,
    float LayerDepth = 0
) : ISpriteBatchLogEntry;

internal record RevertInfo(ISpriteBatchLogEntry State) : ISpriteBatchLogEntry;

internal record SaveTransformInfo() : ISpriteBatchLogEntry;

internal record TranslateInfo(Vector2 Translation) : ISpriteBatchLogEntry;

internal class FakeSpriteBatch : ISpriteBatch
{
    public IReadOnlyList<ISpriteBatchLogEntry> History => history;

    private readonly List<ISpriteBatchLogEntry> history = [];

    public IDisposable Blend(BlendState blendState)
    {
        var entry = new BlendInfo(blendState);
        history.Add(entry);
        return new StateReverter(this, entry);
    }

    public IDisposable Clip(Rectangle clipRect)
    {
        var entry = new ClipInfo(clipRect);
        history.Add(entry);
        return new StateReverter(this, entry);
    }

    public void DelegateDraw(Action<SpriteBatch, Vector2> draw)
    {
        throw new NotImplementedException();
    }

    public void Draw(
        Texture2D texture,
        Vector2 position,
        Rectangle? sourceRectangle,
        Color? color = null,
        float rotation = 0,
        Vector2? origin = null,
        float scale = 1,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0
    )
    {
        history.Add(
            new DrawTextureInfo(
                texture,
                position,
                null,
                sourceRectangle,
                color,
                rotation,
                origin,
                new(scale, scale),
                effects,
                layerDepth
            )
        );
    }

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
        history.Add(
            new DrawTextureInfo(
                texture,
                position,
                null,
                sourceRectangle,
                color,
                rotation,
                origin,
                scale,
                effects,
                layerDepth
            )
        );
    }

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
        history.Add(
            new DrawTextureInfo(
                texture,
                null,
                destinationRectangle,
                sourceRectangle,
                color,
                rotation,
                origin,
                null,
                effects,
                layerDepth
            )
        );
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
        float layerDepth = 0
    )
    {
        history.Add(
            new DrawStringInfo(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth)
        );
    }

    public void InitializeRenderTarget([NotNull] ref RenderTarget2D? target, int width, int height)
    {
        throw new NotImplementedException();
    }

    public IDisposable SaveTransform()
    {
        var entry = new SaveTransformInfo();
        history.Add(entry);
        return new StateReverter(this, entry);
    }

    public IDisposable SetRenderTarget(RenderTarget2D renderTarget, Color? clearColor = null)
    {
        throw new NotImplementedException();
    }

    public void Translate(Vector2 translation)
    {
        history.Add(new TranslateInfo(translation));
    }

    private class StateReverter(FakeSpriteBatch owner, ISpriteBatchLogEntry state) : IDisposable
    {
        public void Dispose()
        {
            owner.history.Add(new RevertInfo(state));
            GC.SuppressFinalize(this);
        }
    }
}