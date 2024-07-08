using Microsoft.Xna.Framework;
using StardewValley.Characters;

namespace SupplyChain.UI;

/// <summary>
/// A uniform grid containing other views.
/// </summary>
/// <remarks>
/// Can be configured to use either a fixed cell size, and therefore a variable number of rows and columns depending on
/// the grid size, or a fixed number of rows and columns, with a variable size per cell.
/// </remarks>
public class Grid : View
{
    /// <summary>
    /// Child views to display in this layout, arranged according to the <see cref="ItemLayout"/>.
    /// </summary>
    public IList<IView> Children
    {
        get => children;
        set => children.SetItems(value);
    }

    /// <summary>
    /// Specifies how to align each child <see cref="IView"/> horizontally within its respective cell, i.e. if the view
    /// is narrower than the cell's width.
    /// </summary>
    public Alignment HorizontalItemAlignment { get; set; } = Alignment.Start;

    /// <summary>
    /// The layout for items (cells) in this grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Layouts are relative to the <see cref="PrimaryOrientation"/>. <see cref="GridItemLayout.Count"/> specifies the
    /// number of columns when <see cref="Orientation.Horizontal"/>, and number of rows when
    /// <see cref="Orientation.Vertical"/>; similarly, <see cref="GridItemLayout.Length"/> specifies the column width
    /// when horizontal and row height when vertical. The other dimension is determined by the individual item's own
    /// <see cref="LayoutParameters"/>.
    /// </para>
    /// <para>
    /// Note that this affects the <i>limits</i> for individual items, not necessarily their exact size. Children may be
    /// smaller than the cells that contain them, and if so are positioned according to the
    /// <see cref="HorizontalItemAlignment"/> and <see cref="VerticalItemAlignment"/>.
    /// </para>
    /// </remarks>
    public GridItemLayout ItemLayout
    {
        get => itemLayout.Value;
        set => itemLayout.Value = value;
    }

    /// <summary>
    /// Spacing between the edges of adjacent columns (<see cref="Vector2.X"/>) and rows (<see cref="Vector2.Y"/>).
    /// </summary>
    /// <remarks>
    /// Setting this is roughly equivalent to specifying the same <see cref="View.Margin"/> on each child, except that
    /// it will not add extra space before the first item or after the last item.
    /// </remarks>
    public Vector2 ItemSpacing
    {
        get => itemSpacing.Value;
        set => itemSpacing.Value = value;
    }

    /// <summary>
    /// Specifies the axis that items are added to before wrapping.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="Orientation.Horizontal"/> means children are added from left to right, and when reaching the edge or
    /// max column count, start over at the beginning of the next row. <see cref="Orientation.Vertical"/> means children
    /// flow from top to bottom, and when reaching the bottom, wrap to the top of the next column.
    /// </para>
    /// <para>
    /// Also affects which dimension is fixed and which is potentially unbounded. Horizontally-oriented grids have a
    /// fixed width and can grow to any height (if <see cref="LayoutParameters.Height"/> is set to
    /// <see cref="Length.Content"/>). Vertically-oriented grids are the opposite, having a fixed height and growing to
    /// an arbitrary width.
    /// </para>
    /// </remarks>
    public Orientation PrimaryOrientation
    {
        get => primaryOrientation.Value;
        set => primaryOrientation.Value = value;
    }

    /// <summary>
    /// Specifies how to align each child <see cref="IView"/> vertically within its respective cell, i.e. if the view
    /// is shorter than the cell's height.
    /// </summary>
    public Alignment VerticalItemAlignment { get; set; } = Alignment.Start;

    private readonly DirtyTrackingList<IView> children = [];
    private readonly List<ViewChild> childPositions = [];
    private readonly DirtyTracker<GridItemLayout> itemLayout = new(GridItemLayout.Count(5));
    private readonly DirtyTracker<Vector2> itemSpacing = new(Vector2.Zero);
    private readonly DirtyTracker<Orientation> primaryOrientation = new(Orientation.Horizontal);

    protected override ViewChild? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        return base.FindFocusableDescendant(contentPosition, direction);
    }

    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return childPositions;
    }

    protected override bool IsContentDirty()
    {
        return itemLayout.IsDirty || itemSpacing.IsDirty || primaryOrientation.IsDirty || children.IsDirty ||
            children.Any(child => child.IsDirty());
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
        childPositions.Clear();
        // TODO: If the secondary orientation specifies Content size, will it be constrained by available size?
        // This is actually something we don't want with an unbounded grid, i.e. it's usually going to be in a scroll
        // container with unknown inner dimension.
        var limits = Layout.GetLimits(availableSize);
        var primaryAvailable = PrimaryOrientation.Get(limits);
        var primarySpacing = PrimaryOrientation.Get(ItemSpacing);
        var (itemLength, countBeforeWrap) = ItemLayout.GetItemCountAndLength(primaryAvailable, primarySpacing);
        var secondaryOrientation = PrimaryOrientation.Swap();
        var secondaryAvailable = secondaryOrientation.Get(limits);
        var secondarySpacing = secondaryOrientation.Get(ItemSpacing);
        var secondaryUsed = 0.0f;
        var position = Vector2.Zero;
        int currentCount = 0;
        var maxSecondary = 0.0f;
        int laneStartIndex = 0;
        foreach (var child in Children)
        {
            var childLimits = Vector2.Zero;
            PrimaryOrientation.Set(ref childLimits, itemLength);
            secondaryOrientation.Set(ref childLimits, secondaryAvailable);
            child.Measure(childLimits);
            childPositions.Add(new(child, position));
            currentCount++;
            maxSecondary = MathF.Max(maxSecondary, secondaryOrientation.Get(child.ActualSize));
            if (currentCount >= countBeforeWrap)
            {
                var cellBounds = childLimits;
                // Limits will have the primary dimension be the actual length, but secondary dimension is the full
                // remaining length in the entire grid. So, adjust it to the max secondary.
                secondaryOrientation.Set(ref cellBounds, maxSecondary);
                // We didn't know the max-secondary value until after iterating all the children in this row/column, so
                // we now need to make a second pass to apply alignment.
                for (int i = laneStartIndex; i < childPositions.Count; i++)
                {
                    var positionOffset = new Vector2(
                        HorizontalItemAlignment.Align(child.ActualSize.X, cellBounds.X),
                        VerticalItemAlignment.Align(child.ActualSize.Y, cellBounds.Y));
                    childPositions[i] = new(childPositions[i].View, childPositions[i].Position + positionOffset);
                }
                PrimaryOrientation.Set(ref position, 0);
                secondaryOrientation.Update(ref position, v => v + maxSecondary + secondarySpacing);
                if (laneStartIndex > 0)
                {
                    secondaryUsed += secondarySpacing;
                }
                secondaryUsed += maxSecondary;
                secondaryAvailable -= maxSecondary + secondarySpacing;
                maxSecondary = 0;
                currentCount = 0;
                laneStartIndex = childPositions.Count;
            }
            else
            {
                PrimaryOrientation.Update(ref position, v => v + itemLength + primarySpacing);
            }
        }
        if (laneStartIndex > 0)
        {
            secondaryUsed += secondarySpacing;
        }
        secondaryUsed += maxSecondary;
        var accumulatedSize = limits;
        secondaryOrientation.Set(ref accumulatedSize, secondaryUsed);
        ContentSize = accumulatedSize;
    }

    protected override void ResetDirty()
    {
        itemLayout.ResetDirty();
        itemSpacing.ResetDirty();
        primaryOrientation.ResetDirty();
        children.ResetDirty();
    }
}

/// <summary>
/// Describes the layout of all items in a <see cref="Grid"/>.
/// </summary>
public sealed class GridItemLayout
{
    private GridItemLayout() { }

    private int? itemCount;
    private float? itemLength;

    /// <summary>
    /// Creates a <see cref="GridItemLayout"/> specifying the maximum divisions - rows or columns, depending on the
    /// grid's <see cref="Orientation"/>; items will be sized distributed uniformly along that axis.
    /// </summary>
    /// <param name="itemCount">Maximum number of cell divisions along the primary orientation axis.</param>
    public static GridItemLayout Count(int itemCount)
    {
        return new() { itemCount = itemCount };
    }

    /// <summary>
    /// Creates a <see cref="GridItemLayout"/> specifying that each item is to have the same fixed length (width or
    /// height, depending on the grid's <see cref="Orientation"/>) and to wrap to the next row/column afterward.
    /// </summary>
    /// <param name="px">The length, in pixels, of each item along the grid's orientation axis.</param>
    public static GridItemLayout Length(float px)
    {
        return new() { itemLength = px };
    }

    /// <summary>
    /// Computes the length (along the grid's <see cref="Grid.PrimaryOrientation"/> axis) of a single item, and the
    /// number of items that can fit before wrapping.
    /// </summary>
    /// <param name="available">The length available along the same axis.</param>
    /// <param name="spacing">Spacing between items, to adjust count-based layouts.</param>
    /// <returns>The length to apply to each item.</returns>
    internal (float, int) GetItemCountAndLength(float available, float spacing)
    {
        if (itemCount.HasValue)
        {
            var validCount = Math.Max(itemCount.Value, 1);
            var length = (available + spacing) / validCount - spacing;
            return (length, validCount);
        }
        else
        {
            var length = itemLength!.Value;
            if (length + spacing <= 0) // Invalid layout
            {
                return (1.0f, 1);
            }
            var exactCount = (available + spacing) / (length + spacing);
            // Rounding this wouldn't be good, since that could overflow; but we also don't want tiny floating-point
            // errors to cause premature wrapping. OK solution is to truncate after adding some epsilon.
            var approximateCount = Math.Max((int)(exactCount + 4 * float.Epsilon), 1);
            return (length, approximateCount);
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is GridItemLayout other && other.itemCount == itemCount && other.itemLength == itemLength;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(itemCount, itemLength);
    }

    public static bool operator ==(GridItemLayout left, GridItemLayout right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(GridItemLayout left, GridItemLayout right)
    {
        return !(left == right);
    }
}