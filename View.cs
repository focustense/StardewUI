using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Base class for typical widgets wanting to implement <see cref="IView"/>.
/// </summary>
/// <remarks>
/// Use of this class isn't required, but provides some useful behaviors so that view types don't need to keep
/// re-implementing them, such as a standard <see cref="Measure"/> implementation that skips unnecessary layouts.
/// </remarks>
public abstract class View : IView
{
    /// <summary>
    /// Event raised when the view receives a click.
    /// </summary>
    public event EventHandler<ClickEventArgs>? Click;

    /// <summary>
    /// The size of the entire area occupied by this view including margins, border and padding.
    /// </summary>
    public Vector2 ActualSize => BorderSize + Margin.Total;

    /// <summary>
    /// The layout size (not edge thickness) of the entire drawn area including the border, i.e. the
    /// <see cref="InnerSize"/> plus any borders defined in <see cref="GetBorderThickness"/>. Does not include the
    /// <see cref="Margin"/>.
    /// </summary>
    public Vector2 BorderSize => InnerSize + GetBorderThickness().Total;

    /// <summary>
    /// The size of the view's content, which is drawn inside the padding. Subclasses set this in their
    /// <see cref="OnMeasure"/> method and padding, margins, etc. are handled automatically.
    /// </summary>
    public Vector2 ContentSize { get; protected set; }

    /// <summary>
    /// The size allocated to the entire area inside the border, i.e. <see cref="ContentSize"/> plus any
    /// <see cref="Padding"/>. Does not include border or <see cref="Margin"/>.
    /// </summary>
    public Vector2 InnerSize => ContentSize + Padding.Total;

    /// <summary>
    /// Whether or not the view should be able to receive focus. Applies only to this specific view, not its children.
    /// </summary>
    /// <remarks>
    /// All views are non-focusable by default and must have their focus enabled explicitly. Subclasses may choose to
    /// override the default value if they should always be focusable.
    /// </remarks>
    public virtual bool IsFocusable { get; set; }

    /// <summary>
    /// Layout settings for this view; determines how its dimensions will be computed.
    /// </summary>
    public LayoutParameters Layout
    {
        get => layout.Value;
        set => layout.Value = value;
    }

    /// <summary>
    /// Margins (whitespace outside border) for this view.
    /// </summary>
    public Edges Margin
    {
        get => margin.Value;
        set => margin.Value = value;
    }

    /// <summary>
    /// Padding (whitespace inside border) for this view.
    /// </summary>
    public Edges Padding
    {
        get => padding.Value;
        set => padding.Value = value;
    }

    /// <summary>
    /// Localized tooltip to display on hover, if any.
    /// </summary>
    public string Tooltip { get; set; } = "";

    /// <summary>
    /// Z order for this view within its direct parent. Higher indices draw later (on top).
    /// </summary>
    public int ZIndex { get; set; }

    /// <summary>
    /// The most recent size used in a <see cref="Measure"/> pass. Used for additional dirty checks.
    /// </summary>
    protected Vector2 LastAvailableSize { get; private set; } = Vector2.Zero;

    private readonly DirtyTracker<LayoutParameters> layout = new(default);
    private readonly DirtyTracker<Edges> margin = new(Edges.NONE);
    private readonly DirtyTracker<Edges> padding = new(Edges.NONE);

    public void Draw(ISpriteBatch b)
    {
        using var _ = b.SaveTransform();
        b.Translate(Margin.Left, Margin.Top);
        OnDrawBorder(b);
        var borderThickness = GetBorderThickness();
        b.Translate(borderThickness.Left + Padding.Left, borderThickness.Top + Padding.Top);
        OnDrawContent(b);
    }

    /// <inheritdoc/>
    /// This will first call <see cref="FindFocusableDescendant"/> to see if the specific view type wants to implement
    /// its own focus search. If there is no focusable descendant, then this will return a reference to the current view
    /// if <see cref="IsFocusable"/> is <c>true</c> and the position is <i>not</i> already within the view's bounds -
    /// meaning, any focusable view can accept focus from any direction, but will not consider itself a result if it is
    /// already focused (since we are trying to "move" focus).
    public ViewChild? FocusSearch(Vector2 position, Direction direction)
    {
        var borderThickness = GetBorderThickness();
        var offset = new Vector2(Margin.Left, Margin.Top)
            + new Vector2(borderThickness.Left, borderThickness.Top)
            + new Vector2(Padding.Left, Padding.Top);
        var found = FindFocusableDescendant(position - offset, direction);
        if (found is not null)
        {
            return new(found.View, found.Position + offset);
        }
        if (IsFocusable && (
            (direction == Direction.East && position.X < 0)
            || (direction == Direction.West && position.X >= ActualSize.X)
            || (direction == Direction.South && position.Y < 0)
            || (direction == Direction.North && position.Y >= ActualSize.Y)))
        {
            return new(this, Vector2.Zero);
        }
        return null;
    }

    public IEnumerable<ViewChild> GetChildren()
    {
        var borderThickness = GetBorderThickness();
        var offset = new Vector2(Margin.Left, Margin.Top)
            + new Vector2(borderThickness.Left, borderThickness.Top)
            + new Vector2(Padding.Left, Padding.Top);
        return GetLocalChildren().Select(viewChild => new ViewChild(viewChild.View, viewChild.Position + offset));
    }

    public bool IsDirty()
    {
        return layout.IsDirty || margin.IsDirty || padding.IsDirty || IsContentDirty();
    }

    public bool Measure(Vector2 availableSize)
    {
        if (!IsDirty() && availableSize == LastAvailableSize)
        {
            return false;
        }
        var adjustedSize = availableSize - Margin.Total - Padding.Total - GetBorderThickness().Total;
        OnMeasure(Vector2.Max(adjustedSize, Vector2.Zero));
        LastAvailableSize = availableSize;
        layout.ResetDirty();
        margin.ResetDirty();
        padding.ResetDirty();
        ResetDirty();
        return true;
    }

    /// <summary>
    /// Searches for a focusable child within this view and returns it if it can be reached in the specified
    /// <paramref name="direction"/>.
    /// </summary>
    /// <remarks>
    /// This is the same as <see cref="FocusSearch"/> but in pre-transformed content coordinates, and does not require
    /// checking for "self-focus" as <see cref="FocusSearch"/> already does this. The default implementation simply
    /// returns <c>null</c> as most views do not have children; subclasses with children must override this.
    /// </remarks>
    protected virtual ViewChild? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        return null;
    }

    /// <summary>
    /// Measures the thickness of each edge of the border, if the view has a border.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Used only by views that will implement a border via <see cref="OnDrawBorder"/>. The border thickness is
    /// considered during layout, and generally treated as additional <see cref="Padding"/> for the purposes of setting
    /// allowed content size.
    /// </para>
    /// <para>
    /// Borders usually have a static size, but if the thickness can change, then implementations must account for it in
    /// their dirty checking (<see cref="IsContentDirty"/>).
    /// </para>
    /// </remarks>
    /// <returns>The border edge thicknesses.</returns>
    protected virtual Edges GetBorderThickness()
    {
        return Edges.NONE;
    }

    /// <summary>
    /// Gets the view's children with positions relative to the content area.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This has the same signature as <see cref="GetChildren"/> but assumes that coordinates are in the same space as
    /// those used in <see cref="OnDrawContent(ISpriteBatch)"/>, i.e. not accounting for margin/border/padding. These
    /// coordinates are automatically adjusted in the <see cref="GetChildren"/> to be relative to the entire view.
    /// </para>
    /// <para>
    /// The default implementation returns an empty sequence. Composite views must override this method in order for
    /// user interactions to behave correctly.
    /// </para>
    /// </remarks>
    /// <returns></returns>
    protected virtual IEnumerable<ViewChild> GetLocalChildren()
    {
        return [];
    }

    /// <summary>
    /// Checks whether or not the internal content/layout has changed.
    /// </summary>
    /// <remarks>
    /// The base implementation of <see cref="IsDirty"/> only checks if the base layout attributes have changed, i.e.
    /// <see cref="Layout"/>, <see cref="Margin"/>, <see cref="Padding"/>, etc. It does not know about content/data in
    /// any subclasses; those that accept content parameters (like text) will typically use
    /// <see cref="DirtyTracker{T}"/> to hold that content and should implement this method to check their
    /// <see cref="DirtyTracker{T}.IsDirty"/> states.
    /// </remarks>
    /// <returns><c>true</c> if content has changed; otherwise <c>false</c>.</returns>
    protected virtual bool IsContentDirty()
    {
        return false;
    }

    /// <inheritdoc/>
    public void OnClick(ClickEventArgs e)
    {
        foreach (var child in GetChildren())
        {
            if (!child.ContainsPoint(e.Position))
            {
                continue;
            }
            var childCursorPosition = e.Position - child.Position;
            var childArgs = new ClickEventArgs(childCursorPosition, e.Button);
            child.View.OnClick(childArgs);
            if (childArgs.Handled)
            {
                e.Handled = true;
                break;
            }
        }
        if (!e.Handled)
        {
            Click?.Invoke(this, e);
        }
    }

    /// <summary>
    /// Draws the view's border, if it has one.
    /// </summary>
    /// <remarks>
    /// This is called from <see cref="Draw"/> after applying <see cref="Margin"/> but before <see cref="Padding"/>.
    /// </remarks>
    /// <param name="b">Sprite batch to hold the drawing output.</param>
    protected virtual void OnDrawBorder(ISpriteBatch b) { }

    /// <summary>
    /// Draws the inner content of this view.
    /// </summary>
    /// <remarks>
    /// This is called from <see cref="Draw"/> after applying both <see cref="Margin"/> and <see cref="Padding"/>.
    /// </remarks>
    /// <param name="b">Sprite batch to hold the drawing output.</param>
    protected abstract void OnDrawContent(ISpriteBatch b);

    /// <summary>
    /// Performs the internal layout.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is called from <see cref="Measure"/> only when the layout is dirty (layout parameters or content changed)
    /// and a new layout is actually required. Subclasses can override this if the layout follows complex rules,
    /// although in most cases it is sufficient to use the default implementation and only override
    /// <see cref="GetContentSize"/>.
    /// </para>
    /// <para>
    /// The <paramref name="availableSize"/> provided to the method is pre-adjusted for <see cref="Margin"/>,
    /// <see cref="Padding"/>, and any border determined by <see cref="GetBorderThickness"/>.
    /// </para>
    /// </remarks>
    /// <param name="availableSize">Size available in the container, after applying padding, margin and borders.</param>
    protected abstract void OnMeasure(Vector2 availableSize);

    /// <summary>
    /// Resets any dirty state associated with this view.
    /// </summary>
    /// <remarks>
    /// This is called at the end of <see cref="Measure"/>, so that on the next pass, all state appears clean unless it
    /// was marked dirty after the last pass completed. The default implementation is a no-op; subclasses should use it
    /// to clear any private dirty state, e.g. via <see cref="DirtyTracker{T}.ResetDirty"/>.
    /// </remarks>
    protected virtual void ResetDirty()
    { 
    }
}
