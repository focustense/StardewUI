using Microsoft.Xna.Framework;

namespace StardewUI;

/// <summary>
/// A layout view whose children all overlap the same boundaries.
/// </summary>
/// <remarks>
/// <para>
/// A panel's content size (i.e. if any dimensions are <see cref="LengthType.Content"/>) is always equal to the largest
/// child; alignment applies to each child individually, and children are drawn according to their
/// <see cref="IView.ZIndex"/> first and then their order in <see cref="Children"/>.
/// </para>
/// <para>
/// Children can be positioned more precisely using their <see cref="View.Margin"/> and <see cref="View.Padding"/> for
/// standard view types, or drawing at non-origin positions for custom <see cref="IView"/> implementations.
/// </para>
/// <para>
/// A common use of panels is to draw overlapping images, in cases where a <see cref="Frame"/> doesn't really make
/// sense, e.g. there is no explicit "background" or "border", or if there are more than 2 layers to draw.
/// </para>
/// </remarks>
public class Panel : View
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
    /// Specifies how to align each child in <see cref="Children"/> horizontally within the frame's area.
    /// </summary>
    public Alignment HorizontalContentAlignment { get; set; } = Alignment.Start;

    /// <summary>
    /// Specifies how to align each child in <see cref="Children"/> vertically within the frame's area.
    /// </summary>
    public Alignment VerticalContentAlignment { get; set; } = Alignment.Start;

    private readonly DirtyTrackingList<IView> children = [];
    private readonly List<ViewChild> childPositions = [];

    protected override FocusSearchResult? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        foreach (var childPosition in childPositions
            .OrderByDescending(child => child.ContainsPoint(contentPosition))
            .ThenByDescending(child => child.View.ZIndex))
        {
            var (view, position) = childPosition;
            // It's possible to move focus to any panel as long as it's in the search direction, but we want to
            // prioritize the child that already has the focus, which is already in the iteration order above.
            var isPossibleMatch = childPosition.IsInDirection(contentPosition, direction);
            if (isPossibleMatch)
            {
                LogFocusSearch(
                    $"Found candidate child '{childPosition.View.Name}' with bounds: " +
                    $"[{childPosition.Position}, {childPosition.View.OuterSize}]");
            }
            if (isPossibleMatch
                && new ViewChild(view, position).FocusSearch(contentPosition, direction) is FocusSearchResult found)
            {
                return found;
            }
        }
        return null;
    }

    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return childPositions;
    }

    protected override bool IsContentDirty()
    {
        return children.IsDirty || children.Any(child => child.IsDirty());
    }

    protected override void OnDrawContent(ISpriteBatch b)
    {
        foreach (var (child, position) in childPositions.OrderBy(child => child.View.ZIndex))
        {
            using var _ = b.SaveTransform();
            b.Translate(position);
            child.Draw(b);
        }
    }

    protected override void OnMeasure(Vector2 availableSize)
    {
        var limits = Layout.GetLimits(availableSize);
        Vector2 maxChildSize = Vector2.Zero;
        foreach (var child in Children)
        {
            child.Measure(limits);
            maxChildSize = Vector2.Max(maxChildSize, child.OuterSize);
        }
        ContentSize = Layout.Resolve(availableSize, () => maxChildSize);
        UpdateChildPositions();
    }

    protected override void ResetDirty()
    {
        children.ResetDirty();
    }

    private void UpdateChildPositions()
    {
        childPositions.Clear();
        foreach (var child in Children)
        {
            var left = HorizontalContentAlignment.Align(child.OuterSize.X, ContentSize.X);
            var top = VerticalContentAlignment.Align(child.OuterSize.Y, ContentSize.Y);
            childPositions.Add(new(child, new(left, top)));
        }
    }
}
