using Microsoft.Xna.Framework;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;

namespace StardewUI.Widgets;

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
        set
        {
            if (children.SetItems(value))
            {
                OnPropertyChanged(nameof(Children));
            }
        }
    }

    /// <summary>
    /// Specifies how to align each child <see cref="IView"/> horizontally within its respective cell, i.e. if the view
    /// is narrower than the cell's width.
    /// </summary>
    public Alignment HorizontalItemAlignment
    {
        get => horizontalItemAlignment;
        set
        {
            if (value != horizontalItemAlignment)
            {
                horizontalItemAlignment = value;
                OnPropertyChanged(nameof(HorizontalItemAlignment));
            }
        }
    }

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
        set
        {
            if (itemLayout.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(ItemLayout));
            }
        }
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
        set
        {
            if (itemSpacing.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(ItemSpacing));
            }
        }
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
        set
        {
            if (primaryOrientation.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(PrimaryOrientation));
            }
        }
    }

    /// <summary>
    /// Specifies how to align each child <see cref="IView"/> vertically within its respective cell, i.e. if the view
    /// is shorter than the cell's height.
    /// </summary>
    public Alignment VerticalItemAlignment
    {
        get => verticalItemAlignment;
        set
        {
            if (value != verticalItemAlignment)
            {
                verticalItemAlignment = value;
                OnPropertyChanged(nameof(VerticalItemAlignment));
            }
        }
    }

    private record CellPosition(int Column, int Row);

    private readonly DirtyTrackingList<IView> children = [];
    private readonly List<ViewChild> childPositions = [];
    private readonly DirtyTracker<GridItemLayout> itemLayout = new(new GridItemLayout.Count(5));
    private readonly DirtyTracker<Vector2> itemSpacing = new(Vector2.Zero);
    private readonly DirtyTracker<Orientation> primaryOrientation = new(Orientation.Horizontal);

    // Regular backing fields
    private Alignment horizontalItemAlignment = Alignment.Start;
    private Alignment verticalItemAlignment = Alignment.Start;

    // These are useful to cache for focus searches. Since the grid is uniform along the primary orientation, we can
    // determine from the coordinates exactly which cell the cursor is sitting in, including its index in the child list
    // and the offset of the previous/next.
    private int countBeforeWrap;
    private float itemLength;
    private readonly List<float> secondaryStartPositions = [];

    /// <inheritdoc />
    protected override FocusSearchResult? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        var cellPosition = GetCellAt(contentPosition);
        LogFocusSearch($"Current cell position: {cellPosition}");
        // If we could guarantee that the implementation were perfect, then there would not really be any need to track
        // the previous index. As it is, this helps prevent an infinite loop in case of an unexpected layout bug.
        int previousCheckedIndex = -1;
        while (true)
        {
            cellPosition = Advance(cellPosition, direction);
            LogFocusSearch($"Searching next cell position: {cellPosition}");
            var nextIndex = GetChildIndexAt(cellPosition);
            if (nextIndex < 0 || nextIndex == previousCheckedIndex)
            {
                LogFocusSearch("Cell position is out of bounds; ending search.");
                return null;
            }
            var nextChild =
                nextIndex < childPositions.Count ? childPositions[nextIndex]
                // GetChildIndexAt can return a position past the end of the list, only on the final row (if horizontal) or
                // final column (if vertical). Whether or not this is considered a valid navigation depends on the
                // direction. Navigating east on the final row, past the last item, should NOT return a value here as it
                // would be considered exiting the entire grid, but navigating up/down from a different row (or from out of
                // bounds) should snap to the last or closest item.
                : direction.GetOrientation() == PrimaryOrientation
                    ? nextIndex > countBeforeWrap ? childPositions[nextIndex - countBeforeWrap]
                        : null
                : childPositions.LastOrDefault();
            var found = nextChild?.FocusSearch(contentPosition, direction);
            if (found is not null)
            {
                return found;
            }
            previousCheckedIndex = nextIndex;
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return childPositions;
    }

    /// <inheritdoc />
    protected override IEnumerable<ViewChild> GetLocalChildrenAt(Vector2 contentPosition)
    {
        var cellPosition = GetCellAt(contentPosition);
        var childIndex = GetChildIndexAt(cellPosition);
        var child = childIndex >= 0 && childIndex < childPositions.Count ? childPositions[childIndex] : null;
        // GetChildIndexAt has some logic to clamp the final row, so double-check to make sure the point is actually
        // inside.
        return (child?.ContainsPoint(contentPosition) ?? false) ? [child] : [];
    }

    /// <inheritdoc />
    protected override bool IsContentDirty()
    {
        return itemLayout.IsDirty
            || itemSpacing.IsDirty
            || primaryOrientation.IsDirty
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
        childPositions.Clear();
        // TODO: If the secondary orientation specifies Content size, will it be constrained by available size?
        // This is actually something we don't want with an unbounded grid, i.e. it's usually going to be in a scroll
        // container with unknown inner dimension.
        var limits = Layout.GetLimits(availableSize);
        var primaryAvailable = PrimaryOrientation.Get(limits);
        var primarySpacing = PrimaryOrientation.Get(ItemSpacing);
        var (itemLength, countBeforeWrap) = ItemLayout.GetItemCountAndLength(primaryAvailable, primarySpacing);
        this.itemLength = itemLength;
        this.countBeforeWrap = countBeforeWrap;
        var secondaryOrientation = PrimaryOrientation.Swap();
        var secondaryAvailable = secondaryOrientation.Get(limits);
        var secondarySpacing = secondaryOrientation.Get(ItemSpacing);
        var secondaryUsed = 0.0f;
        var position = Vector2.Zero;
        int currentCount = 0;
        var maxSecondary = 0.0f;
        int laneStartIndex = 0;
        secondaryStartPositions.Clear();
        secondaryStartPositions.Add(0);
        for (int childIdx = 0; childIdx < Children.Count; childIdx++)
        {
            var child = Children[childIdx];
            var childLimits = Vector2.Zero;
            PrimaryOrientation.Set(ref childLimits, itemLength);
            secondaryOrientation.Set(ref childLimits, secondaryAvailable);
            child.Measure(childLimits);
            childPositions.Add(new(child, position));
            currentCount++;
            maxSecondary = MathF.Max(maxSecondary, secondaryOrientation.Get(child.OuterSize));
            if (currentCount >= countBeforeWrap || childIdx == Children.Count - 1)
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
                        HorizontalItemAlignment.Align(child.OuterSize.X, cellBounds.X),
                        VerticalItemAlignment.Align(child.OuterSize.Y, cellBounds.Y)
                    );
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
                secondaryStartPositions.Add(secondaryUsed);
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
        ContentSize = Layout.Resolve(availableSize, () => accumulatedSize);
    }

    /// <inheritdoc />
    protected override void ResetDirty()
    {
        itemLayout.ResetDirty();
        itemSpacing.ResetDirty();
        primaryOrientation.ResetDirty();
        children.ResetDirty();
    }

    private static CellPosition Advance(CellPosition position, Direction direction)
    {
        var (column, row) = position;
        return direction switch
        {
            Direction.North => new(column, row - 1),
            Direction.South => new(column, row + 1),
            Direction.West => new(column - 1, row),
            Direction.East => new(column + 1, row),
            _ => throw new NotImplementedException($"Invalid direction: {direction}"),
        };
    }

    private int FindPrimaryIndex(Vector2 position)
    {
        var axisPosition = PrimaryOrientation.Get(position);
        if (axisPosition < 0)
        {
            return -1;
        }
        else if (axisPosition >= PrimaryOrientation.Get(ContentSize))
        {
            return countBeforeWrap;
        }
        var cellLength = itemLength + PrimaryOrientation.Get(ItemSpacing);
        return Math.Clamp((int)(axisPosition / cellLength), 0, countBeforeWrap);
    }

    private int FindSecondaryIndex(Vector2 position)
    {
        var secondaryOrientation = PrimaryOrientation.Swap();
        var axisPosition = secondaryOrientation.Get(position);
        if (axisPosition < 0)
        {
            return -1;
        }
        else if (axisPosition >= secondaryOrientation.Get(ContentSize))
        {
            return secondaryStartPositions.Count;
        }
        var index = secondaryStartPositions.BinarySearch(axisPosition);
        return index >= 0 ? index : Math.Clamp(~index - 1, 0, secondaryStartPositions.Count - 1);
    }

    private CellPosition GetCellAt(Vector2 position)
    {
        var primaryIndex = FindPrimaryIndex(position);
        var secondaryIndex = FindSecondaryIndex(position);
        return PrimaryOrientation == Orientation.Horizontal
            ? new(primaryIndex, secondaryIndex)
            : new(secondaryIndex, primaryIndex);
    }

    private int GetChildIndexAt(CellPosition position)
    {
        var (column, row) = position;
        if (column < 0 || row < 0)
        {
            return -1;
        }
        var (primary, secondary) = PrimaryOrientation == Orientation.Horizontal ? (column, row) : (row, column);
        // Usually, going out of bounds on either axis isn't valid. The one special case we allow for focus search is
        // having a column > max on the final row, or vice versa for vertical orientation. Consequently, we allow
        // exceeding the actual item count in that specific case, and the caller must confirm if the navigation is valid
        // (i.e. was in the perpendicular direction).
        if (secondary >= secondaryStartPositions.Count)
        {
            return -1;
        }
        return secondary * countBeforeWrap + Math.Min(primary, countBeforeWrap - 1);
    }
}

/// <summary>
/// Describes the layout of all items in a <see cref="Grid"/>.
/// </summary>
public abstract record GridItemLayout
{
    /// <summary>
    /// A <see cref="GridItemLayout"/> specifying the maximum divisions - rows or columns, depending on the grid's
    /// <see cref="Orientation"/>; items will be sized distributed uniformly along that axis.
    /// </summary>
    /// <param name="ItemCount">Maximum number of cell divisions along the primary orientation axis.</param>
    public sealed record Count(int ItemCount) : GridItemLayout
    {
        /// <inheritdoc />
        public override (float, int) GetItemCountAndLength(float available, float spacing)
        {
            var validCount = Math.Max(ItemCount, 1);
            var length = (available + spacing) / validCount - spacing;
            return (length, validCount);
        }
    }

    /// <summary>
    /// A <see cref="GridItemLayout"/> specifying that each item is to have the same fixed length (width or height,
    /// depending on the grid's <see cref="Orientation"/>) and to wrap to the next row/column afterward.
    /// </summary>
    /// <param name="Px">The length, in pixels, of each item along the grid's orientation axis.</param>
    public sealed record Length(float Px) : GridItemLayout
    {
        /// <inheritdoc />
        public override (float, int) GetItemCountAndLength(float available, float spacing)
        {
            if (Px + spacing <= 0) // Invalid layout
            {
                return (1.0f, 1);
            }
            var exactCount = (available + spacing) / (Px + spacing);
            // Rounding this wouldn't be good, since that could overflow; but we also don't want tiny floating-point
            // errors to cause premature wrapping. OK solution is to truncate after adding some epsilon.
            var approximateCount = Math.Max((int)(exactCount + 4 * float.Epsilon), 1);
            return (Px, approximateCount);
        }
    }

    private GridItemLayout() { }

    /// <summary>
    /// Computes the length (along the grid's <see cref="Grid.PrimaryOrientation"/> axis) of a single item, and the
    /// number of items that can fit before wrapping.
    /// </summary>
    /// <param name="available">The length available along the same axis.</param>
    /// <param name="spacing">Spacing between items, to adjust count-based layouts.</param>
    /// <returns>The length to apply to each item.</returns>
    public abstract (float, int) GetItemCountAndLength(float available, float spacing);
}
