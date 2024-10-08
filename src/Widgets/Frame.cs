﻿using Microsoft.Xna.Framework;

namespace StardewUI;

/// <summary>
/// A view that holds another view, typically for the purpose of adding a border or background, or in some cases
/// swapping out the content.
/// </summary>
public class Frame : View
{
    /// <summary>
    /// The background sprite to draw for this frame.
    /// </summary>
    public Sprite? Background { get; set; }

    /// <summary>
    /// Tint color for the <see cref="Background"/> image.
    /// </summary>
    public Color BackgroundTint { get; set; } = Color.White;

    /// <summary>
    /// The border sprite to draw for this frame.
    /// </summary>
    /// <remarks>
    /// Setting a border here does not affect layout, even if <see cref="Sprite.FixedEdges"/> are set to non-zero
    /// values, since fixed edges only govern scaling and are not necessarily the same as the actual edge thicknesses.
    /// To ensure that inner content does not overlap with the border, <see cref="BorderThickness"/> should also be set
    /// when using a border.
    /// </remarks>
    public Sprite? Border { get; set; }

    /// <summary>
    /// The thickness of the border edges.
    /// </summary>
    /// <remarks>
    /// This property has no effect on the appearance of the <see cref="Border"/>, but affects how content is positioned
    /// inside the border. It is often correct to set it to the same value as the <see cref="Sprite.FixedEdges"/> of the
    /// <see cref="Border"/> sprite, but the values are considered independent.
    /// </remarks>
    public Edges BorderThickness
    {
        get => borderThickness.Value;
        set => borderThickness.Value = value;
    }

    /// <summary>
    /// The inner content view, which will render inside the border and padding.
    /// </summary>
    public IView? Content
    {
        get => content.Value;
        set => content.Value = value;
    }

    /// <summary>
    /// Specifies how to align the <see cref="Content"/> horizontally within the frame's area. Only has an effect if the
    /// frame's content area is larger than the content size, i.e. when <see cref="LayoutParameters.Width"/> does
    /// <i>not</i> use <see cref="LengthType.Content"/>.
    /// </summary>
    public Alignment HorizontalContentAlignment
    {
        get => horizontalContentAlignment.Value;
        set => horizontalContentAlignment.Value = value;
    }

    /// <summary>
    /// Alpha value for the shadow. If set to the default of zero, no shadow will be drawn.
    /// </summary>
    public float ShadowAlpha { get; set; } = 0.0f;

    /// <summary>
    /// Number of shadows to draw if <see cref="ShadowAlpha"/> is non-zero.
    /// </summary>
    /// <remarks>
    /// While rare, some game sprites are supposed to be drawn with multiple stacked shadows. If this number is higher
    /// than the default of <c>1</c>, shadows will be drawn stacked with the offset repeatedly applied.
    /// </remarks>
    public int ShadowCount { get; set; } = 1;

    /// <summary>
    /// Offset to draw the sprite shadow, which is a second copy of the <see cref="Background"/> drawn entirely black.
    /// Shadows will not be visible unless <see cref="ShadowAlpha"/> is non-zero.
    /// </summary>
    public Vector2 ShadowOffset { get; set; }

    /// <summary>
    /// Specifies how to align the <see cref="Content"/> vertically within the frame's area. Only has an effect if the
    /// frame's content area is larger than the content size, i.e. when <see cref="LayoutParameters.Height"/> does
    /// <i>not</i> use <see cref="LengthType.Content"/>.
    /// </summary>
    public Alignment VerticalContentAlignment
    {
        get => verticalContentAlignment.Value;
        set => verticalContentAlignment.Value = value;
    }

    private readonly DirtyTracker<Edges> borderThickness = new(Edges.NONE);
    private readonly DirtyTracker<IView?> content = new(null);
    private readonly DirtyTracker<Alignment> horizontalContentAlignment = new(Alignment.Start);
    private readonly DirtyTracker<Alignment> verticalContentAlignment = new(Alignment.Start);

    private NineSlice? backgroundSlice;
    private NineSlice? borderSlice;
    private Vector2 contentPosition;

    protected override FocusSearchResult? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        return Content?.FocusSearch(contentPosition, direction);
    }

    protected override Edges GetBorderThickness()
    {
        return BorderThickness;
    }

    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return Content is not null ? [new(Content, contentPosition)] : [];
    }

    protected override bool IsContentDirty()
    {
        return borderThickness.IsDirty
            || horizontalContentAlignment.IsDirty
            || verticalContentAlignment.IsDirty
            || content.IsDirty
            || (Content?.IsDirty() ?? false);
    }

    protected override void OnDrawBorder(ISpriteBatch b)
    {
        using (b.SaveTransform())
        {
            b.Translate(BorderThickness.Left, BorderThickness.Top);
            if (ShadowAlpha > 0 && ShadowCount >= 1 && backgroundSlice is not null)
            {
                using var _ = b.SaveTransform();
                for (int i = 0; i < ShadowCount; i++)
                {
                    b.Translate(ShadowOffset);
                    backgroundSlice.Draw(b, new(Color.Black, ShadowAlpha));
                }
            }
            backgroundSlice?.Draw(b, BackgroundTint);
        }
        borderSlice?.Draw(b);
    }

    protected override void OnDrawContent(ISpriteBatch b)
    {
        if (Content is null)
        {
            return;
        }
        using var _ = b.SaveTransform();
        b.Translate(contentPosition);
        Content.Draw(b);
    }

    protected override void OnMeasure(Vector2 availableSize)
    {
        Content?.Measure(Layout.GetLimits(availableSize));

        ContentSize = Layout.Resolve(availableSize, () => Content?.OuterSize ?? Vector2.Zero);
        UpdateContentPosition();

        if (borderSlice?.Sprite != Border)
        {
            borderSlice = Border is not null ? new(Border) : null;
        }
        borderSlice?.Layout(new(Point.Zero, BorderSize.ToPoint()));

        if (backgroundSlice?.Sprite != Background)
        {
            backgroundSlice = Background is not null ? new(Background) : null;
        }
        backgroundSlice?.Layout(new(Point.Zero, InnerSize.ToPoint()));
    }

    protected override void ResetDirty()
    {
        borderThickness.ResetDirty();
        horizontalContentAlignment.ResetDirty();
        verticalContentAlignment.ResetDirty();
        content.ResetDirty();
    }

    private void UpdateContentPosition()
    {
        if (Content is null || Content.OuterSize == ContentSize)
        {
            contentPosition = Vector2.Zero;
            return;
        }
        var left = HorizontalContentAlignment.Align(Content.OuterSize.X, ContentSize.X);
        var top = VerticalContentAlignment.Align(Content.OuterSize.Y, ContentSize.Y);
        contentPosition = new(left, top);
    }
}
