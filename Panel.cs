using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// A view that holds another view, typically for the purpose of adding a border or background, or in some cases
/// swapping out the content.
/// </summary>
public class Panel : View
{
    /// <summary>
    /// The background sprite to draw for this panel.
    /// </summary>
    public Sprite? Background { get; set; }

    /// <summary>
    /// The border sprite to draw for this panel.
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
    /// Specifies how to align the <see cref="Content"/> horizontally within the panel's area. Only has an effect if the
    /// panel's content area is larger than the content size, i.e. when <see cref="LayoutParameters.Width"/> does
    /// <i>not</i> use <see cref="LengthType.Content"/>.
    /// </summary>
    public Alignment HorizontalContentAlignment { get; set; } = Alignment.Start;

    /// <summary>
    /// Specifies how to align the <see cref="Content"/> vertically within the panel's area. Only has an effect if the
    /// panel's content area is larger than the content size, i.e. when <see cref="LayoutParameters.Height"/> does
    /// <i>not</i> use <see cref="LengthType.Content"/>.
    /// </summary>
    public Alignment VerticalContentAlignment { get; set; } = Alignment.Start;

    private readonly DirtyTracker<Edges> borderThickness = new(Edges.NONE);
    private readonly DirtyTracker<IView?> content = new(null);

    private NineSlice? backgroundSlice;
    private NineSlice? borderSlice;

    protected override Edges GetBorderThickness()
    {
        return BorderThickness;
    }

    protected override bool IsContentDirty()
    {
        return borderThickness.IsDirty || content.IsDirty || (Content?.IsDirty() ?? false);
    }

    protected override void OnDrawBorder(ISpriteBatch b)
    {
        using (b.SaveTransform())
        {
            b.Translate(BorderThickness.Left, BorderThickness.Top);
            backgroundSlice?.Draw(b);
        }
        borderSlice?.Draw(b);
    }

    protected override void OnDrawContent(ISpriteBatch b)
    {
        if (Content is null)
        {
            return;
        }
        // ContentSize, as computed in OnMeasure, is the size we allocate to the content inside the padding; it is only
        // the same as Content.ActualSize when layout height and width are both set to Content. If they are fixed pixel
        // sizes or set to match the container size, then the values can be different and alignment will apply.
        if (Content.ActualSize == ContentSize)
        {
            Content.Draw(b);
            return;
        }
        var left = HorizontalContentAlignment.Align(Content.ActualSize.X, ContentSize.X);
        var top = VerticalContentAlignment.Align(Content.ActualSize.Y, ContentSize.Y);
        using var _ = b.SaveTransform();
        b.Translate(left, top);
        Content.Draw(b);
    }

    protected override void OnMeasure(Vector2 availableSize)
    {
        Content?.Measure(Layout.GetLimits(availableSize));

        ContentSize = Layout.Resolve(availableSize, () => Content?.ActualSize ?? Vector2.Zero);

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
        content.ResetDirty();
    }
}
