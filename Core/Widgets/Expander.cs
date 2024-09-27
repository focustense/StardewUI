using Microsoft.Xna.Framework;

namespace StardewUI;

/// <summary>
/// A widget that can be clicked to expand/collapse with additional content.
/// </summary>
public class Expander : WrapperView
{
    /// <summary>
    /// Event that fires when the <see cref="IsExpanded"/> property is changed, either externally
    /// or by clicking on the header.
    /// </summary>
    public event EventHandler<EventArgs>? ExpandedChange;

    /// <summary>
    /// The main content, displayed when expanded.
    /// </summary>
    public IView? Content
    {
        get => contentFrame?.Content;
        set
        {
            RequireView(() => contentFrame).Content = value;
            UpdateContent();
        }
    }

    /// <summary>
    /// Sprite to show next to the header when collapsed.
    /// </summary>
    public Sprite? CollapsedSprite { get; set; } = UiSprites.CaretRight;

    /// <summary>
    /// Sprite to show next to the header when expanded.
    /// </summary>
    /// <remarks>
    /// If this is <c>null</c>, and <see cref="CollapsedSprite"/> is not null, then the
    /// <see cref="CollapsedSprite"/> will be rotated clockwise on expansion.
    /// </remarks>
    public Sprite? ExpandedSprite { get; set; }

    /// <summary>
    /// The primary content, which displays inside the menu frame and is clipped/scrollable.
    /// </summary>
    public IView? Header
    {
        get => headerLane?.Children.ElementAtOrDefault(1);
        set => RequireView(() => headerLane).Children = value is not null ? [indicator, value] : [indicator];
    }

    /// <summary>
    /// Background sprite to display around the <see cref="Header"/> and expansion indicator.
    /// </summary>
    public Sprite? HeaderBackground
    {
        get => headerFrame?.Background;
        set => RequireView(() => headerFrame).Background = value;
    }

    /// <summary>
    /// Tint color for the <see cref="HeaderBackground"/>.
    /// </summary>
    public Color HeaderBackgroundTint
    {
        get => headerFrame?.BackgroundTint ?? Color.White;
        set => RequireView(() => headerFrame).BackgroundTint = value;
    }

    /// <summary>
    /// Configures the layout of the header lane that includes the indicator and
    /// <see cref="Header"/> content.
    /// </summary>
    public LayoutParameters HeaderLayout
    {
        get => headerFrame?.Layout ?? LayoutParameters.AutoRow();
        set => RequireView(() => headerFrame).Layout = value;
    }

    /// <summary>
    /// Padding to apply between the header border and content (including indicator).
    /// </summary>
    public Edges HeaderPadding
    {
        get => headerFrame?.Padding ?? Edges.NONE;
        set => RequireView(() => headerFrame).Padding = value;
    }

    /// <summary>
    /// Whether or not the view is expanded, i.e. whether or not to display the
    /// <see cref="Content"/>.
    /// </summary>
    public bool IsExpanded
    {
        get => isExpanded;
        set
        {
            if (value == isExpanded)
            {
                return;
            }
            isExpanded = value;
            UpdateContent();
            ExpandedChange?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Margin around the entire widget. Same behavior as <see cref="View.Margin"/>.
    /// </summary>
    public Edges Margin
    {
        get => layout?.Margin ?? Edges.NONE;
        set => RequireView(() => layout).Margin = value;
    }

    private bool isExpanded;

    // Initialized in CreateView
    private Frame contentFrame = null!;
    private Frame headerFrame = null!;
    private Lane headerLane = null!;
    private Image indicator = null!;
    private Lane layout = null!;

    protected override IView CreateView()
    {
        indicator = new Image()
        {
            Name = "ExpanderIndicator",
            Layout = new() { Width = Length.Content(), Height = Length.Stretch() },
            Margin = new(Left: 8, Right: 16),
            HorizontalAlignment = Alignment.Middle,
            VerticalAlignment = Alignment.Middle,
        };
        headerLane = new Lane()
        {
            Name = "ExpanderHeaderLane",
            Layout = LayoutParameters.FitContent(),
            VerticalContentAlignment = Alignment.Middle,
            Children = [indicator],
        };
        headerFrame = new Frame()
        {
            Name = "ExpanderHeaderFrame",
            Layout = LayoutParameters.AutoRow(),
            Content = headerLane,
            IsFocusable = true,
        };
        headerFrame.LeftClick += HeaderFrame_LeftClick;
        contentFrame = new Frame() { Name = "ExpanderContentFrame", Layout = LayoutParameters.FitContent() };
        layout = new Lane()
        {
            Name = "ExpanderLayout",
            Layout = LayoutParameters.FitContent(),
            Orientation = Orientation.Vertical,
        };
        UpdateContent();
        return layout;
    }

    private void HeaderFrame_LeftClick(object? sender, ClickEventArgs e)
    {
        IsExpanded = !IsExpanded;
    }

    private void UpdateContent()
    {
        if (layout is null || indicator is null)
        {
            return;
        }
        indicator.Sprite = isExpanded && ExpandedSprite is not null ? ExpandedSprite : CollapsedSprite;
        indicator.Rotation = isExpanded && ExpandedSprite is null ? SimpleRotation.QuarterCounterclockwise : null;
        layout.Children = isExpanded ? [headerFrame, contentFrame] : [headerFrame];
    }
}
