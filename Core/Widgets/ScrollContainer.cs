using System.ComponentModel;
using Microsoft.Xna.Framework;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;

namespace StardewUI.Widgets;

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
        set
        {
            if (value != content.Value)
            {
                if (content.Value is not null)
                {
                    content.Value.PropertyChanged -= Content_PropertyChanged;
                }
                content.Value = value;
                if (value is not null)
                {
                    value.PropertyChanged += Content_PropertyChanged;
                }
                OnPropertyChanged(nameof(Content));
            }
        }
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
        set
        {
            if (value != orientation.Value)
            {
                orientation.Value = value;
                OnPropertyChanged(nameof(Orientation));
            }
        }
    }

    /// <summary>
    /// The amount of "peeking" to add when scrolling a component into view; adds extra space before/after the visible
    /// element so that all or part of the previous/next element is also visible.
    /// </summary>
    /// <remarks>
    /// Nonzero values help with discoverability, making it clear that there is more content.
    /// </remarks>
    public float Peeking
    {
        get => peeking;
        set
        {
            if (peeking != value)
            {
                peeking = value;
                OnPropertyChanged(nameof(Peeking));
            }
        }
    }

    /// <summary>
    /// The current scroll position along the <see cref="Orientation"/> axis.
    /// </summary>
    public float ScrollOffset
    {
        get => scrollOffset.Value;
        set
        {
            var clamped = Math.Clamp(value, 0, ScrollSize);
            if (clamped != scrollOffset.Value)
            {
                scrollOffset.Value = clamped;
                OnPropertyChanged(nameof(ScrollOffset));
            }
        }
    }

    /// <summary>
    /// The maximum amount by which the container can be scrolled without exceeding the inner content bounds.
    /// </summary>
    public float ScrollSize => MathF.Max(Orientation.Get(ContentViewSize) - Orientation.Get(ContentSize), 0);

    /// <summary>
    /// Default scroll distance when calling <see cref="ScrollForward"/> or <see cref="ScrollBackward"/>. Does not
    /// prevent directly setting the scroll position via <see cref="ScrollOffset"/>.
    /// </summary>
    public float ScrollStep
    {
        get => scrollStep;
        set
        {
            if (value != scrollStep)
            {
                scrollStep = value;
                OnPropertyChanged(nameof(ScrollStep));
            }
        }
    }

    /// <summary>
    /// The size of the current content view, or <see cref="Vector2.Zero"/> if there is no content.
    /// </summary>
    protected Vector2 ContentViewSize => Content?.OuterSize ?? Vector2.Zero;

    /// <inheritdoc />
    protected override Vector2 LayoutOffset => -GetScrollOrigin();

    private readonly DirtyTracker<IView?> content = new(null);
    private readonly DirtyTracker<Orientation> orientation = new(Orientation.Vertical);
    private readonly DirtyTracker<float> scrollOffset = new(0);

    private float peeking;
    private float previousScrollSize = -1;
    private float scrollStep = 32.0f;

    /// <summary>
    /// Scrolls backward (up or left) by the distance configured in <see cref="ScrollStep"/>.
    /// </summary>
    public bool ScrollBackward()
    {
        var previousOffset = ScrollOffset;
        ScrollOffset -= ScrollStep;
        return ScrollOffset != previousOffset;
    }

    /// <summary>
    /// Scrolls forward (down or right) by the distance configured in <see cref="ScrollStep"/>.
    /// </summary>
    public bool ScrollForward()
    {
        var previousOffset = ScrollOffset;
        ScrollOffset += ScrollStep;
        return ScrollOffset != previousOffset;
    }

    /// <inheritdoc />
    public override bool ScrollIntoView(IEnumerable<ViewChild> path, out Vector2 distance)
    {
        distance = Vector2.Zero;

        // Descendants may themselves be scrollable. Since we don't know how expensive a possibly-deferred `path` is,
        // save it here so it's cheap to iterate multiple times.
        var descendantList = path.SkipWhile(child => child.View == this).ToArray();
        // The first descendant will (should?) be the Content view, which, when already scrolled, is going to have a
        // negative position along the scroll axis. For the purposes of computing a new, fresh scroll position, we don't
        // want this; instead, we are trying to compute the correct position from scratch.
        // Note that this only applies to the content view, since children in the path are already parent-relative.
        var scrollOrigin = GetScrollOrigin();
        descendantList[0] = descendantList[0].Offset(scrollOrigin);
        bool descendantScrolled = false;
        for (int i = descendantList.Length - 2; i >= 0; i--)
        {
            if (descendantList[i].View.ScrollIntoView(descendantList[(i + 1)..], out var descendantDistance))
            {
                distance += descendantDistance;
                descendantScrolled = true;
            }
        }

        // The bounds we want to bring into view are essentially the union of everything in the path, except for the
        // content view itself which generally cannot fit.
        //
        // For example, ScrollContainer of height 200 includes a Lane of height 500, and has a scroll offset of 50,
        // meaning it currently shows the contents of the Lane between Y=50 and Y=250. We detect that the target is at
        // Y=350, with a height of 20; however, that is all the way at the leaf, for example a tiny button in a large
        // content row, and the row element (between the Lane and the button) actually has a height of 50. Thus, we want
        // to scroll such that the bottom becomes Y=400, not Y=370. The new scroll offset therefore becomes 200.
        //
        // Taking a union, as opposed to just taking the bounds of the first non-direct child, means we also account for
        // views that are positioned out of bounds, e.g. with a negative margin.
        //
        // Ignoring the content view is too much of a simplification, however, because we don't know how far down the
        // tree we're required to navigate to find something with discrete "rows" or "columns", if one even exists. The
        // content view might be a Panel, containing a Frame, containing another Panel, etc., all of which are still too
        // large to fit in the scroll view. Therefore, rather than trying to explicitly ignore the content view, a
        // better approach is to start from the bottom up, and stop as soon as we reach any view that's too large to
        // fit completely in the container; the previous (not too large) union are the bounds to scroll into view.
        var scrollability = TryAccumulateScrollableBounds(descendantList.ToGlobalPositions(), out var scrollableBounds);
        if (scrollability != ScrollResult.FullyScrollable && scrollability != ScrollResult.PartiallyScrollable)
        {
            return false;
        }
        var scrollDistance = GetScrollDistance(scrollableBounds);
        if (scrollDistance != 0)
        {
            var previousOffset = ScrollOffset;
            ScrollOffset += scrollDistance;
            distance += Orientation.CreateVector(ScrollOffset - previousOffset);
            return true;
        }
        return descendantScrolled;
    }

    /// <inheritdoc />
    protected override FocusSearchResult? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        return Content?.FocusSearch(contentPosition, direction);
    }

    /// <inheritdoc />
    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return Content is not null ? [new(Content, Vector2.Zero)] : [];
    }

    /// <inheritdoc />
    protected override IEnumerable<ViewChild> GetLocalChildrenAt(Vector2 contentPosition)
    {
        return Content?.ContainsPoint(contentPosition) == true ? [new(Content, Vector2.Zero)] : [];
    }

    /// <inheritdoc />
    protected override bool IsContentDirty()
    {
        // Don't check scrollOffset here as it doesn't require new layout.
        return orientation.IsDirty || content.IsDirty || (Content?.IsDirty() ?? false);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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
#pragma warning disable CA2245 // Do not assign a property to itself
        // Property clamps itself, so self-assignment is a cheap way to fix invalid offsets.
        ScrollOffset = ScrollOffset;
#pragma warning restore CA2245 // Do not assign a property to itself
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnPropertyChanged(args);
        if (args.PropertyName == nameof(ContentSize))
        {
            OnPropertyChanged(nameof(ScrollSize));
        }
    }

    /// <inheritdoc />
    protected override void ResetDirty()
    {
        content.ResetDirty();
        orientation.ResetDirty();
    }

    private enum ScrollResult
    {
        FullyScrollable,
        PartiallyScrollable,
        NotScrollable,
        NoMoreElements,
    }

    private void Content_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IView.OuterSize))
        {
            OnPropertyChanged(nameof(ScrollSize));
        }
    }

    private float GetScrollDistance(Bounds scrollableBounds)
    {
        var start = Orientation.Get(scrollableBounds.Position);
        if (start < ScrollOffset + Peeking)
        {
            return start - ScrollOffset - Peeking;
        }
        var end = Orientation.Get(scrollableBounds.Position + scrollableBounds.Size);
        var contentLength = Orientation.Get(ContentSize);
        if (end > ScrollOffset + contentLength - Peeking)
        {
            return end - contentLength - ScrollOffset + Peeking;
        }
        return 0;
    }

    private Vector2 GetScrollOrigin()
    {
        return Orientation.CreateVector(ScrollOffset);
    }

    private bool IsFullyScrollable(Bounds bounds)
    {
        return Orientation.Get(bounds.Size) <= Orientation.Get(ContentSize);
    }

    private ScrollResult TryAccumulateScrollableBounds(IEnumerable<ViewChild> globalPath, out Bounds bounds)
    {
        var (child, rest) = globalPath.SplitFirst();
        if (child is null)
        {
            bounds = Bounds.Empty;
            return ScrollResult.NoMoreElements;
        }
        var descendantResult = TryAccumulateScrollableBounds(rest, out bounds);
        switch (descendantResult)
        {
            case ScrollResult.NoMoreElements:
                bounds = child.GetActualBounds();
                return IsFullyScrollable(bounds) ? ScrollResult.FullyScrollable : ScrollResult.NotScrollable;
            case ScrollResult.FullyScrollable:
                if (child.View.ScrollWithChildren != Orientation)
                {
                    return ScrollResult.FullyScrollable;
                }
                var unionBounds = bounds.Union(child.GetActualBounds());
                if (IsFullyScrollable(unionBounds))
                {
                    bounds = unionBounds;
                    return ScrollResult.FullyScrollable;
                }
                return ScrollResult.PartiallyScrollable;
            case ScrollResult.PartiallyScrollable:
                return ScrollResult.PartiallyScrollable;
            default:
                return ScrollResult.NotScrollable;
        }
    }
}
