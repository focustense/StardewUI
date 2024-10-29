using Microsoft.Xna.Framework;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;

namespace StardewUI.Widgets;

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
        set
        {
            if (children.SetItems(value))
            {
                OnPropertyChanged(nameof(Children));
                OnPropertyChanged(nameof(VisibleChildren));
            }
        }
    }

    /// <summary>
    /// Specifies how to align the <see cref="Children"/> horizontally within the lane's area. Only has an effect if the
    /// total content area is larger than the content size, i.e. when <see cref="LayoutParameters.Width"/> does
    /// <i>not</i> use <see cref="LengthType.Content"/>.
    /// </summary>
    public Alignment HorizontalContentAlignment
    {
        get => horizontalContentAlignment.Value;
        set
        {
            if (horizontalContentAlignment.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(HorizontalContentAlignment));
            }
        }
    }

    /// <summary>
    /// The layout orientation.
    /// </summary>
    public Orientation Orientation
    {
        get => orientation.Value;
        set
        {
            if (orientation.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Orientation));
            }
        }
    }

    /// <summary>
    /// Specifies how to align the <see cref="Children"/> vertically within the lane's area. Only has an effect if the
    /// total content area is larger than the content size, i.e. when <see cref="LayoutParameters.Height"/> does
    /// <i>not</i> use <see cref="LengthType.Content"/>.
    /// </summary>
    public Alignment VerticalContentAlignment
    {
        get => verticalContentAlignment.Value;
        set
        {
            if (verticalContentAlignment.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(VerticalContentAlignment));
            }
        }
    }

    /// <summary>
    /// The children that have received layout and have at least some content visible.
    /// </summary>
    public IEnumerable<IView> VisibleChildren => Children.Take(visibleChildCount);

    private readonly DirtyTrackingList<IView> children = [];
    private readonly DirtyTracker<Alignment> horizontalContentAlignment = new(Alignment.Start);
    private readonly DirtyTracker<Orientation> orientation = new(Orientation.Horizontal);
    private readonly DirtyTracker<Alignment> verticalContentAlignment = new(Alignment.Start);
    private readonly List<ViewChild> visibleChildPositions = [];

    private Vector2 childrenSize;
    private int visibleChildCount;

    /// <inheritdoc />
    protected override FocusSearchResult? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        // No matter what the navigation direction is, if the cursor is already within one of the children, then we
        // should perform a recursive focus search on that child before doing anything else, so that delegation works
        // properly.
        var nearestChildIndex = FindNearestChildIndex(contentPosition);
        if (nearestChildIndex < 0)
        {
            LogFocusSearch("Couldn't find any matching children; ending search.");
            return null;
        }

        var nearestChild = visibleChildPositions[nearestChildIndex];
        LogFocusSearch(
            $"Nearest child '{nearestChild.View.Name}' is at index {nearestChildIndex} with bounds: "
                + $"[{nearestChild.Position}, {nearestChild.View.OuterSize}]"
        );
        var nearestResult = nearestChild.FocusSearch(contentPosition, direction);
        var nearestTarget = nearestResult?.Target;
        if (nearestTarget is not null)
        {
            LogFocusSearch(
                $"Nearest child '{nearestChild.View.Name}' matched the query: '{nearestTarget.View.Name}' with "
                    + $"bounds: [{nearestTarget.Position}, {nearestTarget.View.OuterSize}]"
            );
        }
        else
        {
            LogFocusSearch($"Nearest child '{nearestChild.View.Name}' does NOT match the current query.");
        }
        // The search result from the nearest child is always where we want to start, but not always where we want to
        // finish. If the cursor is actually within the child bounds, then it's always correct, but otherwise we have
        // to account for the fact that distance to "descendant focusable" may not be the same as distance to to the
        // child itself; that is, if the children are themselves layout views, then the nearest child may have its only
        // focusable element be much farther away than the second-nearest child.
        //
        // Fortunately it is always going to be either the nearest or second-nearest match, if we assume non-overlapping
        // layout.
        if (nearestTarget is not null && nearestChild.ContainsPoint(contentPosition))
        {
            LogFocusSearch(
                $"Returning nearest child's result '{nearestTarget.View.Name}' since it contains the specified point "
                    + "and has its own match."
            );
            return nearestResult;
        }

        // At this point we either didn't find anything focusable in the nearest child, or aren't sure if it's the best
        // match. We proceed differently depending on whether the direction is along the orientation axis, or orthogonal
        // to it. The parallel case is easier; we only need to traverse the list in the specified direction until
        // something finds focus - which could be the result we already have, the only catch being that the "nearest"
        // element might be on the wrong side, so we have to check.
        if (direction.GetOrientation() == Orientation)
        {
            LogFocusSearch($"Nearest child wasn't exact match; searching ALONG the orientation ({Orientation}) axis.");
            if (nearestTarget is not null && IsCorrectDirection(contentPosition, nearestTarget, direction))
            {
                LogFocusSearch(
                    $"Nearest child's result '{nearestTarget.View.Name}' is already in the requested direction "
                        + $"({direction} of {contentPosition}); returning it."
                );
                return nearestResult;
            }
            var searchStep = IsReverseDirection(direction) ? -1 : 1;
            for (int i = nearestChildIndex + searchStep; i >= 0 && i < visibleChildCount; i += searchStep)
            {
                var searchChild = visibleChildPositions[i];
                LogFocusSearch($"Searching next child '{searchChild.View.Name}' at index {i}");
                var childResult = searchChild.FocusSearch(contentPosition, direction);
                if (childResult is not null)
                {
                    LogFocusSearch(
                        $"Found a match: '{searchChild.View.Name}' matched the query: "
                            + $"'{childResult.Target.View.Name}' with bounds: "
                            + $"[{childResult.Target.Position}, {childResult.Target.View.OuterSize}]"
                    );
                    return childResult;
                }
                LogFocusSearch("No result at this position.");
            }
            LogFocusSearch("No matches anywhere in this lane.");
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
        LogFocusSearch($"Nearest child wasn't exact match; searching OPPOSITE the orientation ({Orientation}) axis.");
        if (nearestChild.ContainsPoint(contentPosition))
        {
            LogFocusSearch("Nearest child already contains the requested position; no more possible results.");
            return null;
        }
        var nearestDistance = nearestTarget is not null
            ? GetDistance(contentPosition, nearestTarget, Orientation)
            : float.PositiveInfinity;
        var ahead = (index: nearestChildIndex, distance: nearestDistance, result: nearestResult);
        var behind = ahead;
        LogFocusSearch("Starting distance search in forward direction");
        for (int i = nearestChildIndex + 1; i < visibleChildCount; i++)
        {
            var nextChild = visibleChildPositions[i];
            LogFocusSearch($"Next candidate is '{nextChild.View.Name}' at position {i}");
            var nextResult = nextChild.FocusSearch(contentPosition, direction);
            var nextDistance = nextResult is not null
                ? MathF.Abs(GetDistance(contentPosition, nextResult.Target, Orientation))
                : float.PositiveInfinity;
            if (nextResult is not null)
            {
                LogFocusSearch(
                    $"Found possible match: '{nextResult.Target.View.Name}' with distance {nextDistance} and bounds: "
                        + $"[{nextResult.Target.Position}, {nextResult.Target.View.OuterSize}]"
                );
            }
            else
            {
                LogFocusSearch("No matches in this child.");
            }
            if (nextDistance < ahead.distance)
            {
                LogFocusSearch($"Distance {nextDistance} is smaller than best ahead distance {ahead.distance}.");
                ahead = (i, nextDistance, nextResult);
            }
            else if (!float.IsPositiveInfinity(nextDistance))
            {
                // We found something to focus, but it's farther away, so stop searching since everything else we
                // subsequently find will be even farther.
                break;
            }
        }
        LogFocusSearch("Starting distance search in reverse direction");
        for (int i = nearestChildIndex - 1; i >= 0; i--)
        {
            var nextChild = visibleChildPositions[i];
            LogFocusSearch($"Next candidate is {nextChild.View.Name} at position {i}");
            var prevResult = nextChild.FocusSearch(contentPosition, direction);
            var prevDistance = prevResult is not null
                ? MathF.Abs(GetDistance(contentPosition, prevResult.Target, Orientation))
                : float.PositiveInfinity;
            if (prevResult is not null)
            {
                LogFocusSearch(
                    $"Found possible match: '{prevResult.Target.View.Name}' with distance {prevDistance} and bounds: "
                        + $"[{prevResult.Target.Position}, {prevResult.Target.View.OuterSize}]"
                );
            }
            else
            {
                LogFocusSearch("No matches in this child.");
            }
            if (prevDistance < behind.distance)
            {
                LogFocusSearch($"Distance {prevDistance} is smaller than best behind distance {behind.distance}.");
                behind = (i, prevDistance, prevResult);
            }
            else if (!float.IsPositiveInfinity(prevDistance))
            {
                break;
            }
        }
        if (ahead.result is null && behind.result is null)
        {
            LogFocusSearch("Didn't find any matches either behind or ahead of the nearest position");
            return null;
        }
        LogFocusSearch(
            "Matches found:\n"
                + $"  Ahead: '{ahead.result?.Target.View.Name}' [{ahead.result?.Target.Position}, "
                + $"{ahead.result?.Target.View.OuterSize}] distance={ahead.distance}\n"
                + $"  Behind: '{behind.result?.Target.View.Name}' [{behind.result?.Target.Position}, "
                + $"{behind.result?.Target.View.OuterSize}] distance={behind.distance}"
        );
        return ahead.distance < behind.distance ? ahead.result : behind.result;
    }

    /// <inheritdoc />
    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return visibleChildPositions;
    }

    /// <inheritdoc />
    protected override bool IsContentDirty()
    {
        return orientation.IsDirty
            || horizontalContentAlignment.IsDirty
            || verticalContentAlignment.IsDirty
            || children.IsDirty
            || visibleChildPositions.Any(child => child.View.IsDirty());
    }

    /// <inheritdoc />
    protected override void OnDrawContent(ISpriteBatch b)
    {
        foreach (var (child, position) in visibleChildPositions.OrderBy(child => child.View.ZIndex))
        {
            using var _ = b.SaveTransform();
            b.Translate(position);
            child.Draw(b);
        }
    }

    /// <inheritdoc />
    protected override void OnMeasure(Vector2 availableSize)
    {
        var limits = Layout.GetLimits(availableSize);
        var swapOrientation = Orientation.Swap();
        childrenSize = Vector2.Zero;
        int previousVisibleChildCount = visibleChildCount;
        visibleChildCount = 0;

        void measureChild(IView child, Vector2 childLimits, bool isDeferred)
        {
            bool hasSwapOrientationStretch = swapOrientation.Length(child.Layout).Type == LengthType.Stretch;
            if (hasSwapOrientationStretch && (!isDeferred || swapOrientation.Length(Layout).Type == LengthType.Content))
            {
                var swapLength = swapOrientation.Get(childrenSize);
                if (swapLength > 0)
                {
                    swapOrientation.Set(ref childLimits, swapLength);
                }
            }
            child.Measure(childLimits);
            var outerLength = Orientation.Get(child.OuterSize);
            Orientation.Update(ref limits, v => v - outerLength);
            Orientation.Update(ref childrenSize, v => v + outerLength);
            if (!hasSwapOrientationStretch)
            {
                swapOrientation.Update(ref childrenSize, v => MathF.Max(v, swapOrientation.Get(child.OuterSize)));
            }
            visibleChildCount++;
        }

        var deferredChildren = new List<IView>();
        foreach (var child in Children)
        {
            if (child.Layout.Width.Type == LengthType.Stretch || child.Layout.Height.Type == LengthType.Stretch)
            {
                // Stretched views have special treatment. A lane should be able to have fixed-size views and then one
                // or more stretched views that use the *remaining* space (instead of greedily consuming the entire
                // space). We'll first process all the other children, then figure out how long the stretches can be.
                //
                // Children stretched along the opposite axis also receive special, but different treatment. Instead of
                // expanding to fill all the available size, they should be limited to the max size of other children.
                //
                // It's possible to end up with combinations that aren't unambiguously resolvable, e.g. if one child
                // uses stretched width and has a content height that depends on width, and another child uses stretched
                // height with width depending on height. We aren't going to implement a huge system of byzantine rules
                // here like CSS, just enough to get the majority of cases correct; unusual/exceptional cases can be
                // handled by simply requiring the caller to be more precise, i.e. specify more fixed dimensions.
                deferredChildren.Add(child);
                continue;
            }
            measureChild(child, limits, false);
        }
        if (deferredChildren.Count > 0)
        {
            var deferredLimits = limits;
            if (childrenSize != Vector2.Zero)
            {
                var swapLimit = swapOrientation
                    .Length(Layout)
                    .Resolve(swapOrientation.Get(availableSize), () => swapOrientation.Get(childrenSize));
                swapOrientation.Set(ref deferredLimits, swapLimit);
            }
            foreach (var child in deferredChildren)
            {
                Orientation.Set(ref deferredLimits, Orientation.Get(limits));
                measureChild(child, deferredLimits, true);
            }
        }

        if (swapOrientation.Get(childrenSize) == 0 && Children.Count > 0)
        {
            swapOrientation.Set(ref childrenSize, Children.Max(c => swapOrientation.Get(c.OuterSize)));
        }

        ContentSize = Layout.Resolve(availableSize, () => childrenSize);
        UpdateVisibleChildPositions();

        if (visibleChildCount != previousVisibleChildCount)
        {
            OnPropertyChanged(nameof(VisibleChildren));
        }
    }

    /// <inheritdoc />
    protected override void ResetDirty()
    {
        horizontalContentAlignment.ResetDirty();
        verticalContentAlignment.ResetDirty();
        children.ResetDirty();
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
            var maxExtent = Orientation.Get(child.Position + child.View.OuterSize);
            var distance =
                (axisPosition >= minExtent && axisPosition < maxExtent)
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
        var maxExtent = orientation.Get(target.Position + target.View.OuterSize);
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
                var y = VerticalContentAlignment.Align(child.OuterSize.Y, ContentSize.Y);
                visibleChildPositions.Add(new(child, new(x, y)));
                x += child.OuterSize.X;
            }
        }
        else
        {
            var y = VerticalContentAlignment.Align(childrenSize.Y, ContentSize.Y);
            foreach (var child in VisibleChildren)
            {
                var x = HorizontalContentAlignment.Align(child.OuterSize.X, ContentSize.X);
                visibleChildPositions.Add(new(child, new(x, y)));
                y += child.OuterSize.Y;
            }
        }
    }
}
