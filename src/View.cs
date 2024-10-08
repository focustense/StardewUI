﻿using System.Diagnostics;
using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace StardewUI;

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
    /// Event raised when the view is being dragged using the mouse.
    /// </summary>
    public event EventHandler<PointerEventArgs>? Drag;

    /// <summary>
    /// Event raised when mouse dragging is stopped, i.e. when the button is released. Always raised after the last
    /// <see cref="Drag"/>, and only once per drag operation.
    /// </summary>
    public event EventHandler<PointerEventArgs>? DragEnd;

    /// <summary>
    /// Event raised when mouse dragging is first activated. Always raised before the first <see cref="Drag"/>, and only
    /// once per drag operation.
    /// </summary>
    public event EventHandler<PointerEventArgs>? DragStart;

    /// <summary>
    /// Event raised when the view receives a click initiated from the left mouse button, or the controller's action
    /// button (A).
    /// </summary>
    public event EventHandler<ClickEventArgs>? LeftClick;

    /// <summary>
    /// Event raised when the pointer enters the view.
    /// </summary>
    public event EventHandler<PointerEventArgs>? PointerEnter;

    /// <summary>
    /// Event raised when the pointer exits the view.
    /// </summary>
    public event EventHandler<PointerEventArgs>? PointerLeave;

    /// <summary>
    /// Event raised when the view receives a click initiated from the right mouse button, or the controller's tool-use
    /// button (X).
    /// </summary>
    public event EventHandler<ClickEventArgs>? RightClick;

    /// <summary>
    /// Event raised when the scroll wheel moves.
    /// </summary>
    public event EventHandler<WheelEventArgs>? Wheel;

    /// <inheritdoc/>
    public Bounds ActualBounds => GetActualBounds();

    /// <inheritdoc/>
    public Bounds ContentBounds => GetContentBounds();

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
    /// Whether or not this view should fire drag events such as <see cref="DragStart"/> and <see cref="Drag"/>.
    /// </summary>
    public bool Draggable { get; set; }

    /// <summary>
    /// The floating elements to display relative to this view.
    /// </summary>
    public IList<FloatingElement> FloatingElements { get; set; } = [];

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
    /// Simple name for this view, used in log/debug output; does not affect behavior.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The size of the entire area occupied by this view including margins, border and padding.
    /// </summary>
    public Vector2 OuterSize => BorderSize + Margin.Total;

    /// <summary>
    /// Padding (whitespace inside border) for this view.
    /// </summary>
    public Edges Padding
    {
        get => padding.Value;
        set => padding.Value = value;
    }

    /// <summary>
    /// Whether this view should receive pointer events like <see cref="Click"/> or <see cref="Drag"/>.
    /// </summary>
    /// <remarks>
    /// By default, all views receive pointer events; this may be disabled for views that intentionally overlap other
    /// views but shouldn't block their input, such as local non-modal overlays.
    /// </remarks>
    public bool PointerEventsEnabled { get; set; } = true;

    /// <inheritdoc />
    /// <summary>
    /// If set to an axis, specifies that when any child of the view is scrolled into view (using
    /// <see cref="ScrollIntoView"/>), then this entire view should be scrolled along with it.
    /// </summary>
    public Orientation? ScrollWithChildren { get; set; }

    /// <summary>
    /// The user-defined tags for this view.
    /// </summary>
    public Tags Tags { get; set; } = new();

    /// <summary>
    /// Localized tooltip to display on hover, if any.
    /// </summary>
    public string Tooltip { get; set; } = "";

    /// <summary>
    /// Visibility for this view.
    /// </summary>
    public Visibility Visibility { get; set; }

    /// <summary>
    /// Z order for this view within its direct parent. Higher indices draw later (on top).
    /// </summary>
    public int ZIndex { get; set; }

    /// <summary>
    /// The most recent size used in a <see cref="Measure"/> pass. Used for additional dirty checks.
    /// </summary>
    protected Vector2 LastAvailableSize { get; private set; } = Vector2.Zero;

    private readonly DirtyTracker<LayoutParameters> layout = new(new());
    private readonly DirtyTracker<Edges> margin = new(Edges.NONE);
    private readonly DirtyTracker<Edges> padding = new(Edges.NONE);

    private IView? draggingView;
    private bool hasChildrenWithOutOfBoundsContent;
    private bool isDragging;

    public View()
    {
        Name = GetType().Name;
    }

    public bool ContainsPoint(Vector2 point)
    {
        return ActualBounds.ContainsPoint(point)
            || FloatingElements.Any(fe => fe.AsViewChild().ContainsPoint(point))
            || (hasChildrenWithOutOfBoundsContent && GetChildren().Any(c => c.ContainsPoint(point)));
    }

    public void Draw(ISpriteBatch b)
    {
        if (Visibility != Visibility.Visible)
        {
            return;
        }
        b.Translate(Margin.Left, Margin.Top);
        using (b.SaveTransform())
        {
            OnDrawBorder(b);
            var borderThickness = GetBorderThickness();
            b.Translate(borderThickness.Left + Padding.Left, borderThickness.Top + Padding.Top);
            OnDrawContent(b);
        }
        foreach (var floatingElement in FloatingElements)
        {
            floatingElement.Draw(b);
        }
    }

    /// <inheritdoc/>
    /// This will first call <see cref="FindFocusableDescendant"/> to see if the specific view type wants to implement
    /// its own focus search. If there is no focusable descendant, then this will return a reference to the current view
    /// if <see cref="IsFocusable"/> is <c>true</c> and the position is <i>not</i> already within the view's bounds -
    /// meaning, any focusable view can accept focus from any direction, but will not consider itself a result if it is
    /// already focused (since we are trying to "move" focus).
    public FocusSearchResult? FocusSearch(Vector2 position, Direction direction)
    {
        if (Visibility != Visibility.Visible)
        {
            return null;
        }
        foreach (var floatingElement in FloatingElements)
        {
            var floatingChild = floatingElement.AsViewChild();
            if (!floatingChild.ContainsPoint(position))
            {
                continue;
            }
            var floatingResult = floatingElement.AsViewChild().FocusSearch(position, direction);
            if (floatingResult is not null)
            {
                return floatingResult;
            }
        }
        var offset = GetContentOffset();
        LogFocusSearch($"{Name} starting focus search: {position - offset}, {direction}");
        var found = FindFocusableDescendant(position - offset, direction);
        if (found is not null)
        {
            LogFocusSearch(
                $"{Name} found focusable descendant '{found.Target.View.Name}' with bounds "
                    + $"[{found.Target.Position}, {found.Target.View.OuterSize}]"
            );
            return found.AsChild(this, offset);
        }
        if (
            IsFocusable
            && (
                (direction == Direction.East && position.X < 0)
                || (direction == Direction.West && position.X >= OuterSize.X)
                || (direction == Direction.South && position.Y < 0)
                || (direction == Direction.North && position.Y >= OuterSize.Y)
            )
        )
        {
            LogFocusSearch(
                $"{Name} found no focusable descendants but matched itself: " + $"[{Vector2.Zero}, {OuterSize}]"
            );
            return new(new(this, Vector2.Zero), []);
        }
        // Second floating-element search is done on purpose.
        //
        // The first one needs to be prioritized, but only if the cursor is already inside it, in order to be
        // able to continue navigating inside that element.
        //
        // This second iteration is to be able to move the focus INTO a floating element from the main view.
        foreach (var floatingElement in FloatingElements)
        {
            var floatingResult = floatingElement.AsViewChild().FocusSearch(position, direction);
            if (floatingResult is not null)
            {
                return floatingResult;
            }
        }
        LogFocusSearch($"View '{Name}' found no focusable descendants matching the query.");
        return null;
    }

    public ViewChild? GetChildAt(Vector2 position)
    {
        return GetChildrenAt(position).FirstOrDefault();
    }

    public Vector2? GetChildPosition(IView childView)
    {
        return GetChildren()
            .Concat(FloatingElements.Select(fe => fe.AsViewChild()))
            .Where(child => child.View == childView)
            .Select(child => child.Position as Vector2?)
            .FirstOrDefault();
    }

    public IEnumerable<ViewChild> GetChildren()
    {
        var offset = GetContentOffset();
        return GetLocalChildren()
            .Select(viewChild => new ViewChild(viewChild.View, viewChild.Position + offset))
            .Concat(FloatingElements.Select(fe => fe.AsViewChild()));
    }

    public IEnumerable<ViewChild> GetChildrenAt(Vector2 position)
    {
        var offset = GetContentOffset();
        var directChildren = GetLocalChildrenAt(position - offset)
            .Where(child => child.View.Visibility == Visibility.Visible);
        foreach (var child in directChildren)
        {
            yield return child.Offset(offset);
        }
        foreach (var floatingElement in FloatingElements)
        {
            var floatingChild = floatingElement.AsViewChild();
            if (floatingChild.ContainsPoint(position))
            {
                yield return floatingChild;
            }
        }
    }

    public virtual ViewChild? GetDefaultFocusChild()
    {
        if (IsFocusable)
        {
            return new(this, Vector2.Zero);
        }
        return GetChildren().Where(child => child.View.GetDefaultFocusChild() is not null).FirstOrDefault();
    }

    public bool HasOutOfBoundsContent()
    {
        return hasChildrenWithOutOfBoundsContent
            || FloatingElements.Any(fe => !ActualBounds.ContainsBounds(fe.AsViewChild().GetActualBounds()));
    }

    public bool IsDirty()
    {
        return layout.IsDirty || margin.IsDirty || padding.IsDirty || IsContentDirty();
    }

    public bool Measure(Vector2 availableSize)
    {
        if (!IsDirty() && availableSize == LastAvailableSize)
        {
            foreach (var floatingElement in FloatingElements)
            {
                floatingElement.MeasureAndPosition(this, wasParentDirty: false);
            }
            return false;
        }
        var adjustedSize = availableSize - Margin.Total - Padding.Total - GetBorderThickness().Total;
        OnMeasure(Vector2.Max(adjustedSize, Vector2.Zero));
        LastAvailableSize = availableSize;
        layout.ResetDirty();
        margin.ResetDirty();
        padding.ResetDirty();
        ResetDirty();
        hasChildrenWithOutOfBoundsContent = GetChildren().Any(child => child.View.HasOutOfBoundsContent());
        foreach (var floatingElement in FloatingElements)
        {
            floatingElement.MeasureAndPosition(this, wasParentDirty: true);
        }
        return true;
    }

    /// <inheritdoc/>
    public virtual void OnClick(ClickEventArgs e)
    {
        if (Visibility != Visibility.Visible)
        {
            return;
        }
        DispatchPointerEvent(e, (view, args) => view.OnClick(args));
        if (!e.Handled)
        {
            Click?.Invoke(this, e);
            if (e.IsPrimaryButton())
            {
                LeftClick?.Invoke(this, e);
            }
            else if (e.IsSecondaryButton())
            {
                RightClick?.Invoke(this, e);
            }
        }
    }

    private ViewChild? GetOrUpdateDraggingChild(Vector2 position)
    {
        // Since the effect of dragging is usually to move some view, we can't rely on the current cursor position to
        // accurately tell us which view to drag; instead, we need to track which is view is dragging, and re-read its
        // current position on each movement.
        if (draggingView is not null)
        {
            var childPosition = GetChildPosition(draggingView);
            return childPosition is not null ? new(draggingView, childPosition.Value) : null;
        }

        foreach (var child in GetChildrenAt(position))
        {
            if (child.View.PointerEventsEnabled)
            {
                draggingView = child.View;
                return child;
            }
        }
        return null;
    }

    /// <inheritdoc/>
    public virtual void OnDrag(PointerEventArgs e)
    {
        if (Visibility != Visibility.Visible)
        {
            return;
        }
        // HACK: The original design assumed only one child at a position, and at this stage the framework is firmly
        // in the territory of having intentionally overlapping children all the time. Most pointer events could easily
        // be refactored, but dragging is a little trickier because dragging is *disabled* by default, meaning unlike
        // pointer events (where we just skip children who have it disabled, and all their descendants), we would have
        // to do a recursive search every time to find a draggable child.
        // The current workaround is just to disable pointer events on any "front" views that shouldn't block the drag
        // of any views underneath, which should be possible a majority of the time since these views are likely to be
        // non-interactive overlay views.
        var draggingChild = GetOrUpdateDraggingChild(e.Position);
        if (draggingChild is not null)
        {
            DispatchPointerEvent(draggingChild, e, (view, args) => view.OnDrag(args));
        }
        if (e.Handled || !Draggable)
        {
            return;
        }
        if (!isDragging)
        {
            var startArgs = e.Clone();
            DragStart?.Invoke(this, startArgs);
            e.Handled |= startArgs.Handled;
        }
        isDragging = true;
        var dragArgs = e.Clone();
        Drag?.Invoke(this, dragArgs);
        e.Handled |= dragArgs.Handled;
    }

    /// <inheritdoc/>
    public virtual void OnDrop(PointerEventArgs e)
    {
        if (Visibility != Visibility.Visible)
        {
            return;
        }
        var draggingChild = GetOrUpdateDraggingChild(e.Position);
        if (draggingChild is not null)
        {
            DispatchPointerEvent(draggingChild, e, (view, args) => view.OnDrop(args));
        }
        draggingView = null;
        if (e.Handled || !isDragging)
        {
            return;
        }
        isDragging = false;
        DragEnd?.Invoke(this, e);
    }

    /// <inheritdoc/>
    public virtual void OnPointerMove(PointerMoveEventArgs e)
    {
        if (Visibility != Visibility.Visible)
        {
            return;
        }
        var previousTarget = GetChildAt(e.PreviousPosition);
        var currentTarget = GetChildAt(e.Position);
        if (currentTarget != previousTarget && previousTarget is not null)
        {
            DispatchPointerEvent(previousTarget, e, (view, args) => view.OnPointerMove(args));
            if (e.Handled)
            {
                return;
            }
        }

        if (currentTarget is not null)
        {
            DispatchPointerEvent(currentTarget, e, (view, args) => view.OnPointerMove(args));
            if (e.Handled)
            {
                return;
            }
        }

        var wasPointerInBounds = ContainsPoint(e.PreviousPosition);
        var isPointerInBounds = ContainsPoint(e.Position);
        if (isPointerInBounds && !wasPointerInBounds)
        {
            PointerEnter?.Invoke(this, e);
        }
        else if (!isPointerInBounds && wasPointerInBounds)
        {
            PointerLeave?.Invoke(this, e);
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// When overriding <see cref="View.OnUpdate"/>, be sure to call <c>base.OnUpdate()</c> to ensure that any view
    /// children also receive their updates.
    /// </remarks>
    public virtual void OnUpdate(TimeSpan elapsed)
    {
        foreach (var child in GetChildren())
        {
            child.View.OnUpdate(elapsed);
        }
    }

    /// <inheritdoc/>
    public virtual void OnWheel(WheelEventArgs e)
    {
        if (Visibility != Visibility.Visible)
        {
            return;
        }
        DispatchPointerEvent(e, (view, args) => view.OnWheel(args));
        if (!e.Handled)
        {
            Wheel?.Invoke(this, e);
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// The default implementation does no scrolling of its own, only passes the request down to the child and aborts if
    /// the child returns <c>true</c>. Scrollable views must override this to provide scrolling behavior.
    /// </remarks>
    public virtual bool ScrollIntoView(IEnumerable<ViewChild> path, out Vector2 distance)
    {
        distance = Vector2.Zero;
        var (parent, children) = path.SplitFirst();
        if (parent?.View == this)
        {
            // Generally should only encounter this condition when called on the root view, using the verbatim path of a
            // FocusSearchResult. Handling it here is a little more convenient than requiring the caller to remember to
            // exclude the first element.
            return ScrollIntoView(children, out distance);
        }
        return parent?.View.ScrollIntoView(children, out distance) ?? false;
    }

    public override string ToString()
    {
        return $"{GetType().Name}('{Name}')";
    }

    /// <summary>
    /// Searches for a focusable child within this view that is reachable in the specified <paramref name="direction"/>,
    /// and returns a result containing the view and search path if found.
    /// </summary>
    /// <param name="contentPosition">The search position, relative to where this view's content starts (after applying
    /// margin, borders and padding).</param>
    /// <param name="direction">The search direction.</param>
    /// <remarks>
    /// This is the same as <see cref="FocusSearch"/> but in pre-transformed content coordinates, and does not require
    /// checking for "self-focus" as <see cref="FocusSearch"/> already does this. The default implementation simply
    /// returns <c>null</c> as most views do not have children; subclasses with children must override this.
    /// </remarks>
    protected virtual FocusSearchResult? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
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
    /// Searches for all views at a given position relative to the content area.
    /// </summary>
    /// <remarks>
    /// The default implementation performs a linear search on all children and returns all whose bounds overlap the
    /// specified <paramref name="position"/>. Views can override this to provide optimized implementations for their
    /// layout, or handle overlapping views.
    /// </remarks>
    /// <param name="contentPosition">The search position, relative to where this view's content starts (after applying
    /// margin, borders and padding).</param>
    /// <returns>The views at the specified <paramref name="contentPosition"/>, sorted in reverse order of their
    /// <see cref="IView.ZIndex"/>.</returns>
    protected virtual IEnumerable<ViewChild> GetLocalChildrenAt(Vector2 contentPosition)
    {
        return GetLocalChildren()
            .Where(child => child.ContainsPoint(contentPosition))
            .OrderByDescending(child => child.View.ZIndex);
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

    [Conditional("DEBUG_FOCUS_SEARCH")]
    protected void LogFocusSearch(string message)
    {
        Logger.Log($"[{GetType().Name}:{Name}] {message}", LogLevel.Debug);
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
    /// and a new layout is actually required. Subclasses must implement this and set <see cref="ContentSize"/> once
    /// layout is complete. Typically, <see cref="LayoutParameters.Resolve"/> should be used in order to ensure that
    /// the original <see cref="LayoutParameters"/> are respected (e.g. if the actual content size is smaller than the
    /// configured size).
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
    protected virtual void ResetDirty() { }

    private void DispatchPointerEvent<T>(T eventArgs, Action<IView, T> dispatch)
        where T : PointerEventArgs, IOffsettable<T>
    {
        foreach (var child in GetChildrenAt(eventArgs.Position))
        {
            if (child.View.PointerEventsEnabled)
            {
                DispatchPointerEvent(child, eventArgs, dispatch);
                if (eventArgs.Handled)
                {
                    break;
                }
            }
        }
    }

    private static void DispatchPointerEvent<T>(ViewChild child, T eventArgs, Action<IView, T> dispatch)
        where T : PointerEventArgs, IOffsettable<T>
    {
        T childArgs = (eventArgs as IOffsettable<T>).Offset(-child.Position);
        dispatch(child.View, childArgs);
        if (childArgs.Handled)
        {
            eventArgs.Handled = true;
        }
    }

    private Bounds GetActualBounds()
    {
        // Only the top/left margins affect drawing positions; the others are incorporated into layout via OuterSize.
        // For example, a view with margin left = 0, margin right = -20 and content size = 50 will have an outer size of
        // 30. If aligned on the right side of a parent of size 100, it will be assigned a left position of 70, and
        // therefore be allowed to draw between 70 and 120. In this case, the "actual bounds" start where the view
        // starts.
        //
        // It's only negative top/left margins where this breaks down, since the canvas is translated during draw. So
        // the same view of content size 50, but with a *left* margin of -20 and no right margin, aligned left, actually
        // starts its draw at X=0 but then moves to X=-20. In our implementation, the view itself is internally offset,
        // as opposed to being given an offset layout position by the parent.
        var x = MathF.Min(Margin.Left, 0);
        var y = MathF.Min(Margin.Top, 0);
        var position = new Vector2(x, y);
        // Similarly, the size used for layout combines the left/right and top/bottom edges, but we have to separate
        // them here; each individual positive edge contributes positive to the total size but each individual negative
        // edge contributes nothing. For our width=50 view, having a left margin of -20, the width is still 50. If we
        // add a right margin of 30, the width is now 80. Negative top/left margins affect the outer layout explicitly,
        // and negative right/bottom margins affect it implicitly (via size calculations), but in terms of the actual
        // size of the view as drawn on screen, none of these matter.
        //
        // We'll use the border size as a starting point, on the assumption that negative borders and negative padding
        // are essentially incoherent (i.e. it's unclear what the "bounds" should really be if a view decides to draw
        // outside its own border).
        var width = BorderSize.X + MathF.Max(Margin.Left, 0) + MathF.Max(Margin.Right, 0);
        var height = BorderSize.Y + MathF.Max(Margin.Top, 0) + MathF.Max(Margin.Bottom, 0);
        var size = new Vector2(width, height);

        return new(position, size);
    }

    private Bounds GetContentBounds()
    {
        var boundsWithMargin = GetActualBounds();
        var position = boundsWithMargin.Position + new Vector2(Math.Max(Margin.Left, 0), Math.Max(Margin.Top, 0));
        return new(position, ContentSize);
    }

    private Vector2 GetContentOffset()
    {
        var borderThickness = GetBorderThickness();
        return new Vector2(Margin.Left, Margin.Top)
            + new Vector2(borderThickness.Left, borderThickness.Top)
            + new Vector2(Padding.Left, Padding.Top);
    }
}
