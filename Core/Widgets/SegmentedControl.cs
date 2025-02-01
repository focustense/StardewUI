using Microsoft.Xna.Framework;
using StardewUI.Animation;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;
using StardewValley;

namespace StardewUI.Widgets;

/// <summary>
/// A horizontal list of options that can each be independently clicked and selected.
/// </summary>
/// <remarks>
/// Segmented controls are a more discoverable and much more controller-friendly version of a
/// <see cref="DropDownList{T}"/> and are particularly effective when the number of possible options
/// (<see cref="Children"/>) is limited to about five.
/// </remarks>
/// <seealso href="https://developer.apple.com/design/human-interface-guidelines/segmented-controls"/>
/// <seealso href="https://m3.material.io/components/segmented-buttons/overview"/>
public partial class SegmentedControl : View
{
    /// <summary>
    /// Whether to balance the segments so that each has the same width.
    /// </summary>
    public bool Balanced
    {
        get => balanced.Value;
        set
        {
            if (balanced.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Balanced));
            }
        }
    }

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
    /// Highlight sprite to draw beneath the selected segment.
    /// </summary>
    /// <remarks>
    /// Always stretched to the exact dimensions of the selected segment, so a nine-slice sprite is recommended.
    /// </remarks>
    public Sprite? Highlight
    {
        get => highlight.Value;
        set
        {
            if (highlight.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Highlight));
            }
        }
    }

    /// <summary>
    /// Tint color with which to draw the <see cref="Highlight"/> sprite.
    /// </summary>
    public Color HighlightTint
    {
        get => highlightTint;
        set
        {
            if (value != highlightTint)
            {
                highlightTint = value;
                OnPropertyChanged(nameof(HighlightTint));
            }
        }
    }

    /// <summary>
    /// Transition animation to use for moving the <see cref="Highlight"/> when a new segment is selected.
    /// </summary>
    public Transition? HighlightTransition
    {
        get => highlightTransition;
        set
        {
            if (value != highlightTransition)
            {
                highlightTransition = value;
                OnPropertyChanged(nameof(HighlightTransition));
            }
        }
    }

    /// <summary>
    /// Horizontal alignment for each frame's content.
    /// </summary>
    /// <remarks>
    /// Only applies when the control is <see cref="Balanced"/> and therefore some segments may be wider than what is
    /// required for the content.
    /// </remarks>
    public Alignment HorizontalContentAlignment
    {
        get => horizontalContentAlignment;
        set
        {
            if (value != horizontalContentAlignment)
            {
                horizontalContentAlignment = value;
                foreach (var frame in lane.Children.OfType<Frame>())
                {
                    frame.HorizontalContentAlignment = horizontalContentAlignment;
                }
                OnPropertyChanged(nameof(HorizontalContentAlignment));
            }
        }
    }

    /// <summary>
    /// Index of the segment (element of <see cref="Children"/>) that is considered to be selected.
    /// </summary>
    public int SelectedIndex
    {
        get => selectedIndex.Value;
        set
        {
            if (selectedIndex.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }
    }

    /// <summary>
    /// Sound cue to play when a new item is selected.
    /// </summary>
    public string SelectionSound
    {
        get => selectionSound;
        set
        {
            if (selectionSound != value)
            {
                selectionSound = value;
                OnPropertyChanged(nameof(SelectionSound));
            }
        }
    }

    /// <summary>
    /// Separator sprite to draw between segments.
    /// </summary>
    /// <remarks>
    /// Separators are always drawn with the specified <see cref="SeparatorWidth"/>, if specified, or the sprite's
    /// native width if not, and are always stretched to the full layout height. No separators are drawn before the
    /// first segment or after the last segment.
    /// </remarks>
    public Sprite? Separator
    {
        get => separator.Value;
        set
        {
            if (separator.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Separator));
            }
        }
    }

    /// <summary>
    /// Tint color with which to draw the <see cref="Separator"/> sprite.
    /// </summary>
    public Color SeparatorTint
    {
        get => separatorTint;
        set
        {
            if (value != separatorTint)
            {
                separatorTint = value;
                foreach (var separator in lane.Children.OfType<Image>())
                {
                    separator.Tint = value;
                }
                OnPropertyChanged(nameof(SeparatorTint));
            }
        }
    }

    /// <summary>
    /// Width to draw the <see cref="Separator"/>, if any is specified.
    /// </summary>
    public int? SeparatorWidth
    {
        get => separatorWidth.Value;
        set
        {
            if (separatorWidth.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(SeparatorWidth));
            }
        }
    }

    /// <summary>
    /// Vertical alignment for each segment's content.
    /// </summary>
    public Alignment VerticalContentAlignment
    {
        get => lane.VerticalContentAlignment;
        set
        {
            if (value != lane.VerticalContentAlignment)
            {
                lane.VerticalContentAlignment = value;
                OnPropertyChanged(nameof(VerticalContentAlignment));
            }
        }
    }

    private readonly DirtyTracker<bool> balanced = new(false);
    private readonly DirtyTrackingList<IView> children = [];
    private readonly DirtyTracker<Sprite?> highlight = new(null);
    private readonly DirtyTracker<Rectangle> highlightRect = new(default);
    private readonly Lane lane = new();
    private readonly DirtyTracker<int> selectedIndex = new(0);
    private readonly DirtyTracker<Sprite?> separator = new(null);
    private readonly DirtyTracker<int?> separatorWidth = new(null);

    private bool hasNewLayout = true;
    private NineSlice? highlightSlice;
    private Color highlightTint = Color.White;
    private Transition? highlightTransition = null;
    private Alignment horizontalContentAlignment = Alignment.Middle;
    private bool isTransitioning;
    private Rectangle previousHighlightRect;
    private string selectionSound = "smallSelect";
    private Color separatorTint = Color.White;
    private TimeSpan transitionTime;

    /// <inheritdoc />
    public override void OnUpdate(TimeSpan elapsed)
    {
        base.OnUpdate(elapsed);
        if (isTransitioning && HighlightTransition is not null)
        {
            transitionTime += elapsed;
        }
    }

    /// <inheritdoc />
    protected override FocusSearchResult? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        return lane.FocusSearch(contentPosition, direction);
    }

    /// <inheritdoc />
    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return lane.GetChildren();
    }

    /// <inheritdoc />
    protected override bool IsContentDirty()
    {
        return balanced.IsDirty
            || separator.IsDirty
            || separatorWidth.IsDirty
            || children.IsDirty
            || children.Any(child => child.IsDirty());
    }

    /// <inheritdoc />
    protected override void OnDrawContent(ISpriteBatch b)
    {
        if (selectedIndex.IsDirty || hasNewLayout)
        {
            var previousHighlightRect = highlightRect.Value;
            var indexInLane = SelectedIndex * 2;
            if (
                highlightRect.SetIfChanged(
                    indexInLane >= 0 && lane.Children.Count > indexInLane
                        ? lane.GetChildren().ElementAt(indexInLane).GetActualBounds().Truncate()
                        : default
                )
            )
            {
                this.previousHighlightRect = previousHighlightRect;
            }
            selectedIndex.ResetDirty();
            hasNewLayout = false;
            transitionTime = TimeSpan.Zero;
            isTransitioning = HighlightTransition is not null && previousHighlightRect != Rectangle.Empty;
        }
        if (highlight.IsDirty)
        {
            highlightSlice = highlight.Value is { } sprite ? new(sprite) : null;
            highlight.ResetDirty();
        }
        if (highlightSlice is not null)
        {
            if (highlightRect.IsDirty || isTransitioning)
            {
                var transitionRect =
                    HighlightTransition is not null && isTransitioning
                        ? Lerps
                            .Get<Rectangle>()!
                            .Invoke(
                                previousHighlightRect,
                                highlightRect.Value,
                                HighlightTransition.GetPosition(transitionTime)
                            )
                        : highlightRect.Value;
                highlightSlice.Layout(transitionRect);
                highlightRect.ResetDirty();
            }
            highlightSlice.Draw(b, HighlightTint);
        }
        lane.Draw(b);

        // Do this in Draw rather than Update so that we still have the isTransitioning flag set earlier.
        if (transitionTime >= HighlightTransition?.TotalDuration)
        {
            transitionTime = HighlightTransition.TotalDuration;
            isTransitioning = false;
        }
    }

    /// <inheritdoc />
    protected override void OnMeasure(Vector2 availableSize)
    {
        if (Balanced)
        {
            var limits = Layout.GetLimits(availableSize);
            float maxSegmentWidth = 0;
            var separatorWidth = SeparatorWidth ?? Separator?.Size.X ?? 0;
            for (int i = 0; i < Children.Count; i++)
            {
                if (i > 0 && separatorWidth > 0)
                {
                    limits.X -= separatorWidth;
                }
                var child = Children[i];
                child.Measure(limits);
                limits.X -= child.OuterSize.X;
                maxSegmentWidth = MathF.Max(maxSegmentWidth, child.OuterSize.X);
            }
            UpdateLane(maxSegmentWidth);
        }
        else
        {
            UpdateLane(null);
        }
        lane.Layout = Layout;
        lane.Measure(availableSize);
        ContentSize = lane.OuterSize;
    }

    /// <inheritdoc />
    protected override void ResetDirty()
    {
        balanced.ResetDirty();
        separator.ResetDirty();
        separatorWidth.ResetDirty();
        children.ResetDirty();
    }

    private void UpdateLane(float? fixedSegmentWidth)
    {
        int indexInLane = 0;
        for (int i = 0; i < Children.Count; i++)
        {
            if (indexInLane > 0)
            {
                Image separatorImage;
                if (lane.Children.Count > indexInLane - 1)
                {
                    separatorImage = (Image)lane.Children[indexInLane - 1];
                }
                else
                {
                    separatorImage = new() { Fit = ImageFit.Stretch, Tint = SeparatorTint };
                    lane.Children.Add(separatorImage);
                }
                separatorImage.Layout = new()
                {
                    Width = Length.Px(SeparatorWidth ?? Separator?.Size.X ?? 0),
                    Height = Length.Stretch(),
                };
                separatorImage.Sprite = Separator;
            }
            var child = Children[i];
            Frame contentFrame;
            if (lane.Children.Count > indexInLane)
            {
                contentFrame = (Frame)lane.Children[indexInLane];
            }
            else
            {
                contentFrame = new() { Focusable = true, HorizontalContentAlignment = HorizontalContentAlignment };
                int capturedIndex = i;
                contentFrame.LeftClick += (_, e) =>
                {
                    if (capturedIndex != SelectedIndex)
                    {
                        Game1.playSound(selectionSound);
                        SelectedIndex = capturedIndex;
                        e.Handled = true;
                    }
                };
                lane.Children.Add(contentFrame);
            }
            contentFrame.Content = child;
            contentFrame.Layout = fixedSegmentWidth.HasValue
                ? new LayoutParameters() { Width = Length.Px(fixedSegmentWidth.Value) }
                : LayoutParameters.FitContent();
            indexInLane += 2;
        }
        while (lane.Children.Count > indexInLane)
        {
            lane.Children.RemoveAt(lane.Children.Count - 1);
        }
    }
}
