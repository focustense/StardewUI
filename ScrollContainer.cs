using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Renders inner content clipped to a boundary and with a modifiable scroll offset.
/// </summary>
/// <remarks>
/// <para>
/// Does not provide its own scroll bar; scrolling UI and behavior can be controlled via adding a
/// <see cref="Scrollbar"/> to any other part of the UI.
/// </para>
/// <para>
/// While nothing prevents a <see cref="ScrollContainer"/> from being set up with the <see cref="Orientation"/>
/// dimension set to use <see cref="LengthType.Content"/>, in general the container will only work correctly when the
/// scrolled dimension is constrained (<see cref="LengthType.Px"/> or <see cref="LengthType.Stretch"/>). Scrolling
/// behavior is enabled by providing an infinite available length to the <see cref="Content"/> view for layout, while
/// constraining its own size.
/// </para>
/// <para>
/// Scrolling is not virtual. Regardless of the difference in size between scroll container and content, the full
/// content will always be drawn on every frame, and simply clipped to the available area. This may therefore not be
/// suitable for extremely long lists or other unbounded content.
/// </para>
/// </remarks>
public class ScrollContainer : View
{
    /// <summary>
    /// Event raised when any aspect of the scrolling changes.
    /// </summary>
    /// <remarks>
    /// This tracks changes to the <see cref="ScrollOffset"/> but also the <see cref="ScrollSize"/>, even if the offset
    /// has not changed. <see cref="ScrollStep"/> is not included.
    /// </remarks>
    public event EventHandler? ScrollChanged;

    /// <summary>
    /// The inner content view which will be scrolled.
    /// </summary>
    public IView? Content
    {
        get => content.Value;
        set => content.Value = value;
    }

    /// <summary>
    /// The orientation, i.e. the direction of scrolling.
    /// </summary>
    /// <remarks>
    /// A single <see cref="ScrollContainer"/> can only scroll in one direction. If content needs to scroll both
    /// horizontally and vertically, a nested <see cref="ScrollContainer"/> can be used.
    /// </remarks>
    public Orientation Orientation
    {
        get => orientation.Value;
        set => orientation.Value = value;
    }

    /// <summary>
    /// The current scroll position along the <see cref="Orientation"/> axis.
    /// </summary>
    public float ScrollOffset
    {
        get => scrollOffset.Value;
        set => scrollOffset.Value = Math.Clamp(value, 0, ScrollSize);
    }

    /// <summary>
    /// The maximum amount by which the container can be scrolled without exceeding the inner content bounds.
    /// </summary>
    public float ScrollSize => MathF.Max(Orientation.Get(ContentViewSize) - Orientation.Get(ContentSize), 0);

    /// <summary>
    /// Default scroll distance when calling <see cref="ScrollForward"/> or <see cref="ScrollBackward"/>. Does not
    /// prevent directly setting the scroll position via <see cref="ScrollOffset"/>.
    /// </summary>
    public float ScrollStep { get; set; } = 32.0f;

    /// <summary>
    /// The size of the current content view, or <see cref="Vector2.Zero"/> if there is no content.
    /// </summary>
    protected Vector2 ContentViewSize => Content?.OuterSize ?? Vector2.Zero;

    private readonly DirtyTracker<IView?> content = new(null);
    private readonly DirtyTracker<Orientation> orientation = new(Orientation.Vertical);
    private readonly DirtyTracker<float> scrollOffset = new(0);

    private float previousScrollSize = -1;

    /// <summary>
    /// Scrolls backward (up or left) by the distance configured in <see cref="ScrollStep"/>.
    /// </summary>
    public void ScrollBackward()
    {
        ScrollOffset -= ScrollStep;
    }

    /// <summary>
    /// Scrolls forward (down or right) by the distance configured in <see cref="ScrollStep"/>.
    /// </summary>
    public void ScrollForward()
    {
        ScrollOffset += ScrollStep;
    }

    protected override FocusSearchResult? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        var origin = GetScrollOrigin();
        return Content?.FocusSearch(contentPosition + origin, direction)?.Offset(-origin);
    }

    protected override ViewChild? GetLocalChildAt(Vector2 contentPosition)
    {
        var origin = GetScrollOrigin();
        return Content?.GetChildAt(contentPosition + origin)?.Offset(-origin);
    }

    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return Content is not null ? [new(Content, -GetScrollOrigin())] : [];
    }

    protected override bool IsContentDirty()
    {
        // Don't check scrollOffset here as it doesn't require new layout.
        return orientation.IsDirty || content.IsDirty || (Content?.IsDirty() ?? false);
    }

    protected override void OnDrawContent(ISpriteBatch b)
    {
        // Doing this check in Draw is a little unusual, but changes to the scroll position/size generally do not affect
        // layout, so we can't rely on OnMeasureContent to do this check. Even hooking the dirty check isn't guaranted
        // to run if the parent already knows that layout hasn't changed.
        if (scrollOffset.IsDirty || ScrollSize != previousScrollSize)
        {
            scrollOffset.ResetDirty();
            previousScrollSize = ScrollSize;
            ScrollChanged?.Invoke(this, EventArgs.Empty);
        }

        if (Content is null)
        {
            return;
        }
        // Note, `ContentSize` is the content "viewport" in this case, it is not `Content.OuterSize`.
        var clipRect = new Rectangle(Point.Zero, ContentSize.ToPoint());
        using var _ = b.Clip(clipRect);
        var origin = GetScrollOrigin();
        b.Translate(-origin);
        Content.Draw(b);
    }

    protected override void OnMeasure(Vector2 availableSize)
    {
        var containerLimits = Layout.GetLimits(availableSize);

        var contentLimits = containerLimits;
        Orientation.Set(ref contentLimits, float.PositiveInfinity);
        Content?.Measure(contentLimits);

        var contentSize = Layout.Resolve(availableSize, () => ContentViewSize);
        var maxContentLength = Orientation.Get(containerLimits);
        if (!float.IsPositiveInfinity(maxContentLength))
        {
            Orientation.Set(ref contentSize, maxContentLength);
        }
        ContentSize = Layout.Resolve(availableSize, () => contentSize);
    }

    protected override void ResetDirty()
    {
        content.ResetDirty();
        orientation.ResetDirty();
    }

    private Vector2 GetScrollOrigin()
    {
        var origin = Vector2.Zero;
        Orientation.Set(ref origin, ScrollOffset);
        return origin;
    }
}
