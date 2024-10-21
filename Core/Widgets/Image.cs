using Microsoft.Xna.Framework;
using StardewUI.Graphics;
using StardewUI.Layout;

namespace StardewUI.Widgets;

/// <summary>
/// A view that draws a sprite, scaled to the layout size.
/// </summary>
public class Image : View
{
    /// <summary>
    /// How to fit the image in the content area, if sizes differ.
    /// </summary>
    /// <remarks>
    /// The fit setting is always ignored when <i>both</i> the <see cref="LayoutParameters.Width"/> and
    /// <see cref="LayoutParameters.Height"/> use <see cref="LengthType.Content"/>, because that combination of settings
    /// will cause the exact <see cref="Sprite.SourceRect"/> (or texture bounds, if not specified) as the layout size.
    /// At least one dimension must be content-independent (fixed or container size) for this to have any effect.
    /// </remarks>
    public ImageFit Fit
    {
        get => fit;
        set
        {
            if (value != fit)
            {
                fit = value;
                OnPropertyChanged(nameof(Fit));
            }
        }
    }

    /// <summary>
    /// Specifies where to align the image horizontally if the image width is different from the final layout width.
    /// </summary>
    public Alignment HorizontalAlignment
    {
        get => horizontalAlignment;
        set
        {
            if (value != horizontalAlignment)
            {
                horizontalAlignment = value;
                OnPropertyChanged(nameof(HorizontalAlignment));
            }
        }
    }

    /// <summary>
    /// Rotation to apply to the image.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="Scale"/>, rotation potentially <b>does</b> affect layout under specific conditions;
    /// specifically, if any dimensions are <see cref="LengthType.Content"/> sized, and the rotation is 90° in either
    /// direction, it will take the opposite dimension for layout. However, images whose dimensions are entirely fixed
    /// or stretch-based will not have their layout affected.
    /// </remarks>
    public SimpleRotation? Rotation
    {
        get => rotation.Value;
        set
        {
            if (rotation.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Rotation));
            }
        }
    }

    /// <summary>
    /// Scale to apply to the image.
    /// </summary>
    /// <remarks>
    /// This scale acts only as a drawing transformation and does not affect layout; a scaled-up image can potentially
    /// draw (or clip) outside its container, and a scaled-down image will not shrink the size of an image that
    /// specifies <see cref="LengthType.Content"/> for either or both dimensions.
    /// </remarks>
    public float Scale
    {
        get => scale.Value;
        set
        {
            if (scale.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Scale));
            }
        }
    }

    /// <summary>
    /// Alpha value for the shadow. If set to the default of zero, no shadow will be drawn.
    /// </summary>
    public float ShadowAlpha
    {
        get => shadowAlpha;
        set
        {
            if (value != shadowAlpha)
            {
                shadowAlpha = value;
                OnPropertyChanged(nameof(ShadowAlpha));
            }
        }
    }

    /// <summary>
    /// Offset to draw the sprite shadow, which is a second copy of the <see cref="Sprite"/> drawn entirely black.
    /// Shadows will not be visible unless <see cref="ShadowAlpha"/> is non-zero.
    /// </summary>
    public Vector2 ShadowOffset
    {
        get => shadowOffset;
        set
        {
            if (value != shadowOffset)
            {
                shadowOffset = value;
                OnPropertyChanged(nameof(ShadowOffset));
            }
        }
    }

    /// <summary>
    /// The sprite to draw.
    /// </summary>
    /// <remarks>
    /// If <see cref="LayoutParameters"/> uses <see cref="LengthType.Content"/> for either dimension, then changing the
    /// sprite can affect layout depending on <see cref="Fit"/>.
    /// </remarks>
    public Sprite? Sprite
    {
        get => sprite.Value;
        set
        {
            if (sprite.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Sprite));
            }
        }
    }

    /// <summary>
    /// Tint color (multiplier) to apply when drawing.
    /// </summary>
    public Color Tint
    {
        get => tint;
        set
        {
            if (value != tint)
            {
                tint = value;
                OnPropertyChanged(nameof(Tint));
            }
        }
    }

    /// <summary>
    /// Specifies where to align the image vertically if the image height is different from the final layout height.
    /// </summary>
    public Alignment VerticalAlignment
    {
        get => verticalAlignment;
        set
        {
            if (value != verticalAlignment)
            {
                verticalAlignment = value;
                OnPropertyChanged(nameof(VerticalAlignment));
            }
        }
    }

    private readonly DirtyTracker<SimpleRotation?> rotation = new(null);
    private readonly DirtyTracker<float> scale = new(1.0f);
    private readonly DirtyTracker<Sprite?> sprite = new(null);

    private Rectangle destinationRect = Rectangle.Empty;
    private ImageFit fit = ImageFit.Contain;
    private Alignment horizontalAlignment = Alignment.Start;
    private float shadowAlpha;
    private Vector2 shadowOffset;
    private NineSlice? slice = null;
    private Color tint = Color.White;
    private Alignment verticalAlignment = Alignment.Start;

    /// <inheritdoc />
    protected override bool IsContentDirty()
    {
        // We intentionally don't check scale here, as scale doesn't affect layout size.
        // Instead, that is checked (and reset) in the draw method.
        return sprite.IsDirty || rotation.IsDirty;
    }

    /// <inheritdoc />
    protected override void OnDrawContent(ISpriteBatch b)
    {
        if (scale.IsDirty)
        {
            UpdateSlice();
        }
        Rectangle? clipRect = Fit == ImageFit.Cover ? new(0, 0, (int)ContentSize.X, (int)ContentSize.Y) : null;
        if (ShadowAlpha > 0 && slice is not null)
        {
            using var _transform = b.SaveTransform();
            b.Translate(ShadowOffset);
            using var _shadowClip = clipRect.HasValue ? b.Clip(clipRect.Value) : null;
            slice.Draw(b, new(Color.Black, ShadowAlpha));
        }
        using var _clip = clipRect.HasValue ? b.Clip(clipRect.Value) : null;
        slice?.Draw(b, Tint);
    }

    /// <inheritdoc />
    protected override void OnMeasure(Vector2 availableSize)
    {
        var limits = Layout.GetLimits(availableSize);
        var imageSize = GetImageSize(limits);
        ContentSize = Layout.Resolve(availableSize, () => imageSize);
        if (sprite.IsDirty)
        {
            slice = sprite.Value is not null ? new(sprite.Value) : null;
        }
        if (slice is not null)
        {
            var left = HorizontalAlignment.Align(imageSize.X, ContentSize.X);
            var top = VerticalAlignment.Align(imageSize.Y, ContentSize.Y);
            destinationRect = new Rectangle(new Vector2(left, top).ToPoint(), imageSize.ToPoint());
            UpdateSlice();
        }
    }

    /// <inheritdoc />
    protected override void ResetDirty()
    {
        rotation.ResetDirty();
        sprite.ResetDirty();
    }

    private Vector2 GetImageSize(Vector2 limits)
    {
        if (Sprite is null)
        {
            return Vector2.Zero;
        }
        var sourceRect = Sprite.SourceRect ?? Sprite.Texture.Bounds;
        var swapDimensions = Rotation?.IsQuarter() ?? false;
        var (sourceWidth, sourceHeight) = !swapDimensions
            ? (sourceRect.Width, sourceRect.Height)
            : (sourceRect.Height, sourceRect.Width);
        var scale = Sprite.SliceSettings?.Scale ?? 1;
        var scaledSourceWidth = sourceWidth * scale;
        var scaledSourceHeight = sourceHeight * scale;
        if (
            Layout.Width.Type == LengthType.Content
            && (Fit != ImageFit.Contain || Layout.Height.Type == LengthType.Content)
            && scaledSourceWidth < limits.X
        )
        {
            limits.X = scaledSourceWidth;
        }
        if (
            Layout.Height.Type == LengthType.Content
            && (Fit != ImageFit.Contain || Layout.Width.Type == LengthType.Content)
            && scaledSourceHeight < limits.Y
        )
        {
            limits.Y = scaledSourceHeight;
        }
        if (Fit == ImageFit.Stretch)
        {
            return limits;
        }
        if (Fit == ImageFit.None || IsSourceSize())
        {
            return new(scaledSourceWidth, scaledSourceHeight);
        }
        var maxScaleX = limits.X / sourceWidth;
        var maxScaleY = limits.Y / sourceHeight;
        return Fit switch
        {
            ImageFit.Contain => sourceRect.Size.ToVector2() * MathF.Min(maxScaleX, maxScaleY),
            ImageFit.Cover => sourceRect.Size.ToVector2() * MathF.Max(maxScaleX, maxScaleY),
            _ => throw new NotImplementedException($"Invalid fit type: {Fit}"),
        };
    }

    private bool IsSourceSize()
    {
        return Layout.Width.Type == LengthType.Content && Layout.Height.Type == LengthType.Content;
    }

    private void UpdateSlice()
    {
        if (slice is null)
        {
            // Still reset the dirty flag, because when the slice is eventually created it will have the newest scale.
            scale.ResetDirty();
            return;
        }

        if (Scale == 1.0f)
        {
            slice.Layout(destinationRect, Rotation);
        }
        else
        {
            var deltaSize = destinationRect.Size.ToVector2() * (Scale - 1) / 2;
            var scaledRect = destinationRect; // Make a copy (Rectangle is struct)
            scaledRect.Inflate(deltaSize.X, deltaSize.Y);
            slice.Layout(scaledRect, Rotation);
        }
        rotation.ResetDirty();
        scale.ResetDirty();
    }
}

/// <summary>
/// Specifies how an image should be scaled to fit the content area when the available size is different from the image
/// size, and especially when it has a different aspect ratio.
/// </summary>
public enum ImageFit
{
    /// <summary>
    /// Don't scale the image, i.e. draw it at its original size regardless of the eventual layout size.
    /// </summary>
    None,

    /// <summary>
    /// Force uniform scaling, and make both dimensions small enough to fit in the content area.
    /// </summary>
    /// <remarks>
    /// If one dimension uses <see cref="LengthType.Content"/>, then the other dimension will be scaled to fit, and the
    /// content-dependent dimension will be set according to the image's aspect ratio.
    /// </remarks>
    Contain,

    /// <summary>
    /// Force uniform scaling, and make both dimensions large enough to completely cover the content area (i.e. clip
    /// whatever parts are outside the bounds).
    /// </summary>
    /// <remarks>
    /// </remarks>
    Cover,

    /// <summary>
    /// Allow non-uniform scaling, and scale the image to exactly match the content area.
    /// </summary>
    Stretch,
}
