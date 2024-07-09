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
        // No matter what the navigation direction is, if the cursor is already within one of the children, then we
        // should perform a recursive focus search on that child before doing anything else, so that delegation works
        // properly.
        var nearestChildIndex = FindNearestChildIndex(contentPosition);
        if (nearestChildIndex < 0)
        {
            return null;
        }

        var nearestChild = visibleChildPositions[nearestChildIndex];
        var nearestResult = nearestChild.FocusSearch(contentPosition, direction);
        // The search result from the nearest child is always where we want to start, but not always where we want to
        // finish. If the cursor is actually within the child bounds, then it's always correct, but otherwise we have
        // to account for the fact that distance to "descendant focusable" may not be the same as distance to to the
        // child itself; that is, if the children are themselves layout views, then the nearest child may have its only
        // focusable element be much farther away than the second-nearest child.
        //
        // Fortunately it is always going to be either the nearest or second-nearest match, if we assume non-overlapping
        // layout.
        if (nearestResult is not null && nearestChild.ContainsPoint(contentPosition))
        {
            return nearestResult;
        }

        // At this point we either didn't find anything focusable in the nearest child, or aren't sure if it's the best
        // match. We proceed differently depending on whether the direction is along the orientation axis, or orthogonal
        // to it. The parallel case is easier; we only need to traverse the list in the specified direction until
        // something finds focus - which could be the result we already have, the only catch being that the "nearest"
        // element might be on the wrong side, so we have to check.
        if (direction.GetOrientation() == Orientation)
        {
            if (nearestResult is not null && IsCorrectDirection(contentPosition, nearestResult, direction))
            {
                return nearestResult;
            }
            var searchStep = IsReverseDirection(direction) ? -1 : 1;
            for (int i = nearestChildIndex + searchStep; i >= 0 && i < visibleChildCount; i += searchStep)
            {
                var childResult = visibleChildPositions[i].FocusSearch(contentPosition, direction);
                if (childResult is not null)
                {
                    return childResult;
                }
            }
            return null;
        }

        // Perpendicular to the orientation is more intuitive visually, but trickier to implement. We have to search in
        // both directions to be sure we've found the closest point. If we're willing to accept a bit of redundancy then
        // a relatively simple approach is to just fan out until we find we're getting farther away.
        //
        // One useful caveat is that if the cursor is already inside the bounds of one of the children when moving this
        // way, as opposed to entering the bounds of the entire lane from a different view entirely, then we don't allow
        // the movement, otherwise nonintuitive things can happen like moving UP several views when the RIGHT button is
        // pressed.
        if (nearestChild.ContainsPoint(contentPosition))
        {
            return null;
        }
        var nearestDistance = nearestResult is not null
            ? GetDistance(contentPosition, nearestResult, Orientation)
            : float.PositiveInfinity;
        var ahead = (index: nearestChildIndex, distance: nearestDistance, result: nearestResult);
        var behind = ahead;
        for (int i = nearestChildIndex + 1; i < visibleChildCount; i++)
        {
            var nextResult = visibleChildPositions[i].FocusSearch(contentPosition, direction);
            var nextDistance = nextResult is not null
                ? MathF.Abs(GetDistance(contentPosition, nextResult, Orientation))
                : float.PositiveInfinity;
            if (nextDistance < ahead.distance)
            {
                ahead = (i, nextDistance, nextResult);
            }
            else if (!float.IsPositiveInfinity(nextDistance))
            {
                // We found something to focus, but it's farther away, so stop searching since everything else we
                // subsequently find will be even farther.
                break;
            }
        }
        for (int i = nearestChildIndex - 1; i >= 0; i--)
        {
            var prevResult = visibleChildPositions[i].FocusSearch(contentPosition, direction);
            var prevDistance = prevResult is not null
                ? MathF.Abs(GetDistance(contentPosition, prevResult, Orientation))
                : float.PositiveInfinity;
            if (prevDistance < behind.distance)
            {
                behind = (i, prevDistance, prevResult);
            }
            else if (!float.IsPositiveInfinity(prevDistance))
            {
                break;
            }
        }
        if (ahead.result is null && behind.result is null)
        {
            return null;
        }
        return ahead.distance < behind.distance ? ahead.result : behind.result;
    }    

    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return visibleChildPositions;
    }

    protected override bool IsContentDirty()
    {
        return orientation.IsDirty || children.IsDirty || children.Any(child => child.IsDirty());
    }

    protected override void OnDrawContent(ISpriteBatch b)
    {
        foreach (var (child, position) in visibleChildPositions.OrderBy(child => child.View.ZIndex))
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

    private int FindNearestChildIndex(Vector2 position)
    {
        // Child positions are sorted, so we could technically do a binary search. For the number of elements typically
        // found in a lane, it's probably not worth the extra complexity.
        var axisPosition = Orientation.Get(position);
        int bestIndex = -1;
        var maxDistance = float.PositiveInfinity;
        for (int i = 0; i < visibleChildCount; i++)
        {
            var child = visibleChildPositions[i];
            var minExtent = Orientation.Get(child.Position);
            var maxExtent = Orientation.Get(child.Position + child.View.ActualSize);
            var distance = (axisPosition >= minExtent && axisPosition < maxExtent)
                ? 0
                : MathF.Min(MathF.Abs(axisPosition - minExtent), MathF.Abs(axisPosition - maxExtent));
            if (distance < maxDistance)
            {
                maxDistance = distance;
                bestIndex = i;
            }
        }
        return bestIndex;
    }

    private static float GetDistance(Vector2 position, ViewChild target, Orientation orientation)
    {
        var axisPosition = orientation.Get(position);
        var minExtent = orientation.Get(target.Position);
        var maxExtent = orientation.Get(target.Position + target.View.ActualSize);
        if (axisPosition >= minExtent && axisPosition < maxExtent)
        {
            return 0;
        }
        var distanceToMin = axisPosition - minExtent;
        var distanceToMax = axisPosition - maxExtent;
        // Note: Doing it this way preserves the sign, so other helpers like IsCorrectDirection can determine which side
        // the child is on.
        return MathF.Abs(distanceToMin) < MathF.Abs(distanceToMax) ? distanceToMin : distanceToMax;
    }

    private static bool IsCorrectDirection(Vector2 position, ViewChild child, Direction direction)
    {
        var distance = GetDistance(position, child, direction.GetOrientation());
        // The distance is measured from child to position, so a negative distance corresponds to a positive direction.
        return float.IsNegative(distance) ^ IsReverseDirection(direction);
    }

    // Whether a direction is the reverse of the iteration order of the child list.
    private static bool IsReverseDirection(Direction direction)
    {
        return direction == Direction.North || direction == Direction.West;
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
