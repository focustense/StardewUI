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
