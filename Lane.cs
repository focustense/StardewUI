using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Simple unidirectional layout that draws multiple child views in a row or column arrangement.
/// </summary>
public class Lane : View
{
    /// <summary>
    /// Child views to display in this layout.
    /// </summary>
    public IList<IView> Children
    {
        get => children;
        set => children.SetItems(value);
    }

    /// <summary>
    /// Specifies how to align the <see cref="Content"/> horizontally within the lane's area. Only has an effect if the
    /// total content area is larger than the content size, i.e. when <see cref="LayoutParameters.Width"/> does
    /// <i>not</i> use <see cref="LengthType.Content"/>.
    /// </summary>
    public Alignment HorizontalContentAlignment { get; set; } = Alignment.Start;

    /// <summary>
    /// The layout orientation.
    /// </summary>
    public Orientation Orientation
    {
        get => orientation.Value;
        set => orientation.Value = value;
    }

    /// <summary>
    /// Specifies how to align the <see cref="Content"/> vertically within the lane's area. Only has an effect if the
    /// total content area is larger than the content size, i.e. when <see cref="LayoutParameters.Height"/> does
    /// <i>not</i> use <see cref="LengthType.Content"/>.
    /// </summary>
    public Alignment VerticalContentAlignment { get; set; } = Alignment.Start;

    /// <summary>
    /// The children that have received layout and have at least some content visible.
    /// </summary>
    public IEnumerable<IView> VisibleChildren => Children.Take(visibleChildCount);

    private readonly DirtyTrackingList<IView> children = [];
    private readonly DirtyTracker<Orientation> orientation = new(Orientation.Horizontal);
    private readonly List<ViewChild> visibleChildPositions = [];

    private Vector2 childrenSize;
    private int visibleChildCount;

    protected override ViewChild? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        var isDirectionParallelToOrientation = direction switch
        {
            Direction.North | Direction.South => Orientation == Orientation.Vertical,
            _ => Orientation == Orientation.Horizontal
        };
        return isDirectionParallelToOrientation
            ? ParallelFocusSearch(contentPosition, direction)
            : PerpendicularFocusSearch(contentPosition, direction);
    }

    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return visibleChildPositions;
    }

    protected override bool IsContentDirty()
    {
        return orientation.IsDirty || children.Any(child => child.IsDirty());
    }

    protected override void OnDrawContent(ISpriteBatch b)
    {
        foreach (var (child, position) in visibleChildPositions)
        {
            using var _ = b.SaveTransform();
            b.Translate(position);
            child.Draw(b);
        }
    }

    protected override void OnMeasure(Vector2 availableSize)
    {
        var limits = Layout.GetLimits(availableSize);
        var contentWidth = 0.0f;
        var contentHeight = 0.0f;
        visibleChildCount = 0;
        if (Orientation == Orientation.Horizontal)
        {
            foreach (var child in Children)
            {
                child.Measure(limits);
                limits.X -= child.ActualSize.X;
                contentWidth += child.ActualSize.X;
                contentHeight = MathF.Max(contentHeight, child.ActualSize.Y);
                visibleChildCount++;
                if (limits.X <= 0)
                {
                    break;
                }
            }
        }
        else
        {
            foreach (var child in Children)
            {
                child.Measure(limits);
                limits.Y -= child.ActualSize.Y;
                contentHeight += child.ActualSize.Y;
                contentWidth = MathF.Max(contentWidth, child.ActualSize.X);
                visibleChildCount++;
                if (limits.Y <= 0)
                {
                    break;
                }
            }
        }
        childrenSize = new(contentWidth, contentHeight);
        ContentSize = Layout.Resolve(availableSize, () => childrenSize);
        UpdateVisibleChildPositions();
    }

    protected override void ResetDirty()
    {
        orientation.ResetDirty();
    }

    private record FocusSearchInfo(int Index, ViewChild? Focusable, float Distance);

    private float Axis(Vector2 position)
    {
        return Orientation == Orientation.Horizontal ? position.X : position.Y;
    }

    private int FindNearestChildIndex(Vector2 position, out float distance)
    {
        var target = Axis(position);
        int previousChildIndex = -1;
        distance = float.PositiveInfinity;
        float previousDistance = float.PositiveInfinity;
        for (int i = 0; i < visibleChildCount; i++)
        {
            var nextChild = visibleChildPositions[i];
            float nextDistance = GetAxisDistance(nextChild, target);
            if (nextDistance >= previousDistance)
            {
                distance = previousDistance;
                break;
            }
            previousChildIndex = i;
            previousDistance = nextDistance;
        }
        return previousChildIndex;
    }

    private float GetAxisDistance(ViewChild child, float target)
    {
        // We're trying to understand the "distance" of what is actually an entire range, from a point. What we want
        // is actually the minimum distance; that is, if the point lies anywhere inside the child's bounds, then the
        // distance is zero, and if outside the bounds, it is distance to the nearest edge.
        float startEdge = Axis(child.Position);
        float endEdge = startEdge + Axis(child.View.ActualSize);
        return (target >= startEdge && target <= endEdge)
            ? 0
            : MathF.Min(MathF.Abs(target - startEdge), MathF.Abs(target - endEdge));
    }

    private FocusSearchInfo GetFocusSearchInfo(int index, Vector2 position, Direction direction)
    {
        var child = visibleChildPositions[index];
        var focusable = child.View.FocusSearch(position, direction);
        var distance = focusable is not null
            ? MathF.Abs(Axis(focusable.Position) - Axis(position))
            : float.PositiveInfinity;
        return new(index, focusable, distance);
    }

    private static bool IsForwardDirection(Direction direction)
    {
        return direction switch
        {
            Direction.North | Direction.East => true,
            _ => false
        };
    }

    private ViewChild? ParallelFocusSearch(Vector2 position, Direction direction)
    {
        // If a parallel focus search has been requested, then it means we don't care about the orthogonal axis position
        // at all; just find the descendant adjacent to the current focus (if known), otherwise the outer edge.
        var (defaultStartIndex, endIndex, step) = IsForwardDirection(direction)
                ? (0, visibleChildCount, 1)
                : (visibleChildCount - 1, -1, -1);
        var focusedIndex = visibleChildPositions.FindIndex(child => child.ContainsPoint(position));
        var startIndex = focusedIndex >= 0 ? focusedIndex : defaultStartIndex;
        for (int i = startIndex; i != endIndex; i += step)
        {
            var child = visibleChildPositions[focusedIndex];
            var childResult = child.View.FocusSearch(child.Position, direction);
            if (childResult is not null)
            {
                return childResult;
            }
        }
        return null;
    }

    private ViewChild? PerpendicularFocusSearch(Vector2 position, Direction direction)
    {
        // Perpendicular search follows these rules:
        // - If the focus is already within the view, then it's an automatic null; since we are 1D, there is no way to
        //   navigate in the perpendicular direction.
        // - Similarly, if the focus is out of bounds but the direction would take it further out of bounds, then this
        //   is also an automatic null.
        // - If the focus would be "coming into" this view - i.e. the position is out of bounds, but movement in the
        //   given direction could place it into bounds - then we try to find the focusable descendant that's closest
        //   along the other axis.

        switch (direction)
        {
            // These conditions look inverted, but they are guard clauses that are supposed to make sure we are OUT of
            // bounds and that the focus could come INTO bounds.
            case Direction.North:
                if (position.Y <= ContentSize.Y) return null;
                break;
            case Direction.South:
                if (position.Y >= 0) return null;
                break;
            case Direction.West:
                if (position.X <= ContentSize.X) return null;
                break;
            case Direction.East:
                if (position.X >= 0) return null;
                break;
        }

        int nearestIndex = FindNearestChildIndex(position, out var distance);
        if (nearestIndex == -1)
        {
            // No visible children?
            return null;
        }

        // The nearest child index isn't necessarily focusable. There may or may not be anything focusable inside it.
        // This is only our starting point, we may have to fan out in both directions until we find a match.
        // Consider a vertical lane containing many horizontal lanes, i.e. a grid, and the user presses "east" to enter
        // the vertical lane, e.g. from a nav column. The lanes themselves aren't focusable and it's possible that some
        // horizontal sub-lanes don't have any focusable children or any children at all. So we start with the lane that
        // lines up with the cursor, but may have to go up or down some lanes to find focus.
        var startInfo = GetFocusSearchInfo(nearestIndex, position, direction);
        var endInfo = distance == 0 || nearestIndex < visibleChildCount - 1
            // Zero distance tells us that the cursor's position along this axis would actually be within the child's
            // bounds. This doesn't guarantee there's a focusable element, but it does mean that if there IS a focusable
            // element, then it is definitely the best one; therefore we can begin our search with both the "start" and
            // "end" hits on the same index, so that if a match is found here, it will be immediately returned.
            ? startInfo
            // Otherwise, we calculated the distance to the edge, but the nearest focusable _descendant_ (assuming the
            // child is not directly focusable) could be very far away from that edge. We aren't sure yet whether the
            // ideal candidate is really in the "nearest index", or the following index, and have to try both.
            : GetFocusSearchInfo(nearestIndex + 1, position, direction);
        // Now we have to make sure that both the start and end positions have an actual result, unless there's nothing
        // left to search in that direction.
        while (startInfo.Focusable is null && startInfo.Index >= 0)
        {
            startInfo = GetFocusSearchInfo(startInfo.Index - 1, position, direction);
        }
        while (endInfo.Focusable is null && endInfo.Index < visibleChildCount - 1)
        {
            endInfo = GetFocusSearchInfo(endInfo.Index + 1, position, direction);
        }
        // Can finally compare these; whichever subtree had the closer focusable child wins. Note that
        // GetFocusSearchInfo already yields infinite distance if nothing is found, so we don't need to check nullness.
        return startInfo.Distance < endInfo.Distance ? startInfo.Focusable : endInfo.Focusable;
    }

    private void UpdateVisibleChildPositions()
    {
        visibleChildPositions.Clear();
        if (Orientation == Orientation.Horizontal)
        {
            var x = HorizontalContentAlignment.Align(childrenSize.X, ContentSize.X);
            foreach (var child in VisibleChildren)
            {
                var y = VerticalContentAlignment.Align(child.ActualSize.Y, ContentSize.Y);
                visibleChildPositions.Add(new(child, new(x, y)));
                x += child.ActualSize.X;
            }
        }
        else
        {
            var y = VerticalContentAlignment.Align(childrenSize.Y, ContentSize.Y);
            foreach (var child in VisibleChildren)
            {
                var x = HorizontalContentAlignment.Align(child.ActualSize.X, ContentSize.X);
                visibleChildPositions.Add(new(child, new(x, y)));
                y += child.ActualSize.Y;
            }
        }
    }
}
