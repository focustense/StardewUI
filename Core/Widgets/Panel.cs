using Microsoft.Xna.Framework;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;

namespace StardewUI.Widgets;

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
        set
        {
            if (children.SetItems(value))
            {
                OnPropertyChanged(nameof(Children));
            }
        }
    }

    /// <summary>
    /// Specifies how to align each child in <see cref="Children"/> horizontally within the frame's area.
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
    /// Specifies how to align each child in <see cref="Children"/> vertically within the frame's area.
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
    /// Creates a <see cref="Panel"/> that is used to align some inner content inside a parent, typically another
    /// <see cref="Panel"/>.
    /// </summary>
    /// <remarks>
    /// The created panel will stretch to fill all available area, and align the <paramref name="content"/> view within
    /// itself according to the <paramref name="horizontal"/> and <paramref name="vertical"/> alignments. Several
    /// <see cref="Align"/> helpers can be used to align different content/controls to different edges or corners of the
    /// same parent <see cref="Panel"/>.
    /// </remarks>
    /// <param name="content">The content to align.</param>
    /// <param name="horizontal">Horizontal alignment of the content.</param>
    /// <param name="vertical">Vertical alignment of the content.</param>
    /// <param name="name">Optional name to give to the panel, for debugging.</param>
    public static Panel Align(
        IView content,
        Alignment horizontal = Alignment.Start,
        Alignment vertical = Alignment.Start,
        string name = ""
    )
    {
        return new Panel()
        {
            Name = name,
            Layout = LayoutParameters.Fill(),
            HorizontalContentAlignment = horizontal,
            VerticalContentAlignment = vertical,
            Children = [content],
        };
    }

    private readonly DirtyTrackingList<IView> children = [];
    private readonly List<ViewChild> childPositions = [];
    private readonly DirtyTracker<Alignment> horizontalContentAlignment = new(Alignment.Start);
    private readonly DirtyTracker<Alignment> verticalContentAlignment = new(Alignment.Start);

    /// <inheritdoc />
    protected override FocusSearchResult? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        foreach (
            var childPosition in childPositions
                .OrderByDescending(child => child.ContainsPoint(contentPosition))
                .ThenByDescending(child => child.View.ZIndex)
        )
        {
            var (view, position) = childPosition;
            // It's possible to move focus to any panel as long as it's in the search direction, but we want to
            // prioritize the child that already has the focus, which is already in the iteration order above.
            var isPossibleMatch = childPosition.IsInDirection(contentPosition, direction);
            if (isPossibleMatch)
            {
                LogFocusSearch(
                    $"Found candidate child '{childPosition.View.Name}' with bounds: "
                        + $"[{childPosition.Position}, {childPosition.View.OuterSize}]"
                );
            }
            if (
                isPossibleMatch
                && new ViewChild(view, position).FocusSearch(contentPosition, direction) is FocusSearchResult found
            )
            {
                return found;
            }
        }
        return null;
    }

    /// <inheritdoc />
    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return childPositions;
    }

    /// <inheritdoc />
    protected override bool IsContentDirty()
    {
        return horizontalContentAlignment.IsDirty
            || verticalContentAlignment.IsDirty
            || children.IsDirty
            || children.Any(child => child.IsDirty());
    }

    /// <inheritdoc />
    protected override void OnDrawContent(ISpriteBatch b)
    {
        foreach (var (child, position) in childPositions.OrderBy(child => child.View.ZIndex))
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
        // Any children set to Stretch should wait until non-Stretch children have measured. That
        // way, they stretch to whatever size the fixed/content children use.
        // Similar to Lane, we don't attempt to perfectly resolve ambiguities such as having one
        // child with stretched width and another with stretched height.
        var deferredChildren = new List<IView>();
        Vector2 maxChildSize = Vector2.Zero;
        foreach (var child in Children)
        {
            if (child.Layout.Width.Type == LengthType.Stretch || child.Layout.Height.Type == LengthType.Stretch)
            {
                // HACK: It's a bit of a cheat, and inconsistent with the way Lane works, but we can resolve *some*
                // potential ambiguity by incorporating fixed minimum sizes before actually layout out these children.
                // Minimum sizes aren't used very often, but the typical use case for this hack would be a border or
                // background image that is meant to scale around content but looks bad when too small.
                maxChildSize.X = Math.Max(maxChildSize.X, child.Layout.MinWidth ?? 0);
                maxChildSize.Y = Math.Max(maxChildSize.Y, child.Layout.MinHeight ?? 0);
                deferredChildren.Add(child);
                continue;
            }
            child.Measure(limits);
            maxChildSize = Vector2.Max(maxChildSize, child.OuterSize);
        }
        var deferredLimits = maxChildSize != Vector2.Zero ? Layout.Resolve(availableSize, () => maxChildSize) : limits;
        foreach (var child in deferredChildren)
        {
            child.Measure(deferredLimits);
            maxChildSize = Vector2.Max(maxChildSize, child.OuterSize);
        }
        ContentSize = Layout.Resolve(availableSize, () => maxChildSize);
        UpdateChildPositions();
    }

    /// <inheritdoc />
    protected override void ResetDirty()
    {
        horizontalContentAlignment.ResetDirty();
        verticalContentAlignment.ResetDirty();
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
