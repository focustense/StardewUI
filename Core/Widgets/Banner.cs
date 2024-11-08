using Microsoft.Xna.Framework;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewValley;
using StardewValley.BellsAndWhistles;

namespace StardewUI.Widgets;

/// <summary>
/// Draws banner-style text with an optional background.
/// </summary>
/// <remarks>
/// This is very similar to a <see cref="Label"/> inside a <see cref="Frame"/>, but uses the special
/// <see cref="SpriteText"/> font which is more prominent than any of the game's available
/// <see cref="Microsoft.Xna.Framework.Graphics.SpriteFont"/>s and often used for top-level headings/menu titles.
/// </remarks>
public class Banner : View
{
    /// <summary>
    /// Background sprite (including border) to draw underneath the text.
    /// </summary>
    public Sprite? Background
    {
        get => background;
        set
        {
            if (value != background)
            {
                background = value;
                OnPropertyChanged(nameof(Background));
            }
        }
    }

    /// <summary>
    /// The thickness of the border edges within the <see cref="Background"/>. sprite.
    /// </summary>
    /// <remarks>
    /// This property has no effect on the appearance of the <see cref="Background"/>, but affects how content is
    /// positioned inside the border. It is often correct to set it to the same value as the
    /// <see cref="Sprite.FixedEdges"/> of the <see cref="Background"/> sprite, but the values are considered
    /// independent.
    /// </remarks>
    public Edges BackgroundBorderThickness
    {
        get => backgroundBorderThickness.Value;
        set
        {
            if (backgroundBorderThickness.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(BackgroundBorderThickness));
            }
        }
    }

    /// <summary>
    /// The text to display within the banner.
    /// </summary>
    public string Text
    {
        get => text.Value;
        set
        {
            if (text.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Text));
            }
        }
    }

    /// <summary>
    /// Alpha value for the text shadow, per layer in <see cref="ShadowLayers"/>.
    /// </summary>
    /// <remarks>
    /// If set to zero, no text shadow will be drawn.
    /// </remarks>
    public float TextShadowAlpha
    {
        get => textShadowAlpha;
        set
        {
            if (value != textShadowAlpha)
            {
                textShadowAlpha = value;
                OnPropertyChanged(nameof(TextShadowAlpha));
            }
        }
    }

    /// <summary>
    /// Base color for the text shadow, before applying <see cref="TextShadowAlpha"/>.
    /// </summary>
    public Color TextShadowColor
    {
        get => textShadowColor;
        set
        {
            if (value != textShadowColor)
            {
                textShadowColor = value;
                OnPropertyChanged(nameof(textShadowColor));
            }
        }
    }

    /// <summary>
    /// Specifies which layers of the text shadow should be drawn.
    /// </summary>
    /// <remarks>
    /// Layers are additive, so the same <see cref="TextShadowAlpha"/> will have a different visual intensity depending
    /// on which layers are allowed. If set to <see cref="ShadowLayers.None"/>, then no shadow will be drawn.
    /// </remarks>
    public ShadowLayers TextShadowLayers
    {
        get => textShadowLayers;
        set
        {
            if (value != textShadowLayers)
            {
                textShadowLayers = value;
                OnPropertyChanged(nameof(TextShadowLayers));
            }
        }
    }

    /// <summary>
    /// Offset to draw the text shadow, which is a second copy of the <see cref="Text"/> drawn entirely black.
    /// Text shadows will not be visible unless <see cref="TextShadowAlpha"/> is non-zero.
    /// </summary>
    public Vector2 TextShadowOffset
    {
        get => textShadowOffset;
        set
        {
            if (value != textShadowOffset)
            {
                textShadowOffset = value;
                OnPropertyChanged(nameof(TextShadowOffset));
            }
        }
    }

    private static readonly ShadowLayers[] shadowLayerOrder =
    [
        ShadowLayers.Diagonal,
        ShadowLayers.Vertical,
        ShadowLayers.Horizontal,
    ];

    private readonly DirtyTracker<Edges> backgroundBorderThickness = new(Edges.NONE);
    private readonly DirtyTracker<string> text = new("");

    private Sprite? background;
    private NineSlice? backgroundSlice;
    private float textShadowAlpha;
    private Color textShadowColor = Game1.textShadowDarkerColor;
    private ShadowLayers textShadowLayers = ShadowLayers.All;
    private Vector2 textShadowOffset = new(-3, 3);
    private Vector2 textSize;

    /// <inheritdoc />
    protected override Edges GetBorderThickness()
    {
        return BackgroundBorderThickness;
    }

    /// <inheritdoc />
    protected override bool IsContentDirty()
    {
        return backgroundBorderThickness.IsDirty || text.IsDirty;
    }

    /// <inheritdoc />
    protected override void OnDrawBorder(ISpriteBatch b)
    {
        backgroundSlice?.Draw(b);
    }

    /// <inheritdoc />
    protected override void OnDrawContent(ISpriteBatch b)
    {
        var centerX = ContentSize.X / 2;
        b.DelegateDraw(
            (wb, origin) =>
            {
                if (TextShadowAlpha > 0 && TextShadowLayers > 0)
                {
                    var shadowAlphaColor = TextShadowColor * TextShadowAlpha;
                    foreach (var layer in shadowLayerOrder)
                    {
                        if ((TextShadowLayers & layer) == 0)
                        {
                            continue;
                        }
                        var offset = layer switch
                        {
                            ShadowLayers.Diagonal => TextShadowOffset,
                            ShadowLayers.Horizontal => new(TextShadowOffset.X, 0),
                            ShadowLayers.Vertical => new(0, TextShadowOffset.Y),
                            _ => throw new InvalidOperationException($"Invalid shadow layer {layer}"),
                        };
                        SpriteText.drawStringHorizontallyCenteredAt(
                            wb,
                            Text,
                            (int)(origin.X + centerX + offset.X),
                            (int)(origin.Y + offset.Y),
                            color: shadowAlphaColor
                        );
                    }
                }
                SpriteText.drawStringHorizontallyCenteredAt(wb, Text, (int)(origin.X + centerX), (int)origin.Y);
            }
        );
    }

    /// <inheritdoc />
    protected override void OnMeasure(Vector2 availableSize)
    {
        var width = SpriteText.getWidthOfString(Text);
        var height = SpriteText.getHeightOfString(Text);
        textSize = new(width, height);
        ContentSize = Layout.Resolve(availableSize, () => textSize);

        if (backgroundSlice?.Sprite != Background)
        {
            backgroundSlice = Background is not null ? new(Background) : null;
        }
        backgroundSlice?.Layout(new(Point.Zero, BorderSize.ToPoint()));
    }

    /// <inheritdoc />
    protected override void ResetDirty()
    {
        backgroundBorderThickness.ResetDirty();
        text.ResetDirty();
    }
}
