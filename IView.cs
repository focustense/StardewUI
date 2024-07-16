using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Represents some arbitrary UI element or layout.
/// </summary>
public interface IView
{
    /// <summary>
    /// Event raised when the view receives a click.
    /// </summary>
    event EventHandler<ClickEventArgs> Click;

    /// <summary>
    /// Event raised when the view is being dragged using the mouse.
    /// </summary>
    event EventHandler<PointerEventArgs> Drag;

    /// <summary>
    /// Event raised when mouse dragging is stopped, i.e. when the button is released. Always raised after the last
    /// <see cref="Drag"/>, and only once per drag operation.
    /// </summary>
    event EventHandler<PointerEventArgs> DragEnd;

    /// <summary>
    /// Event raised when mouse dragging is first activated. Always raised before the first <see cref="Drag"/>, and only
    /// once per drag operation.
    /// </summary>
    event EventHandler<PointerEventArgs> DragStart;

    /// <summary>
    /// Event raised when the pointer enters the view.
    /// </summary>
    event EventHandler<PointerEventArgs> PointerEnter;

    /// <summary>
    /// Event raised when the pointer exits the view.
    /// </summary>
    event EventHandler<PointerEventArgs> PointerLeave;

    /// <summary>
    /// Event raised when the scroll wheel moves.
    /// </summary>
    event EventHandler<WheelEventArgs> Wheel;

    /// <summary>
    /// The bounds of this view relative to the origin (0, 0).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Typically, a view's bounds is the rectangle from (0, 0) having size of <see cref="OuterSize"/>, but there may be
    /// a difference especially in the case of negative margins. The various sizes affect layout flow and can even be
    /// negative - for example, in a left-to-right layout, a view with left margin -100, right margin 20 and inner width
    /// 30 (no padding) has an X size of -50, indicating that it actually (correctly) causes adjacent views to be pulled
    /// left along with it. However, <c>ActualBounds</c> always has a positive <see cref="Bounds.Size"/>, and if an
    /// implicit content offset is being applied (e.g. because of negative margins) then it will be reflected in
    /// <see cref="Bounds.Position"/> and not affect the <see cref="Bounds.Size"/>; the previous example would have
    /// position X = -100 and size X = 50 (30 content + 20 right margin).
    /// </para>
    /// <para>
    /// In terms of usage, <see cref="OuterSize"/> is generally used for the layout itself (<see cref="Measure"/> and
    /// <see cref="View.OnMeasure"/> of parent views) whereas <see cref="ActualBounds"/> is preferred for click and
    /// focus targeting.
    /// </para>
    /// </remarks>
    Bounds ActualBounds { get; }

    /// <summary>
    /// Whether or not the view can receive controller focus, i.e. the stick/d-pad controlled cursor can move to this
    /// view. Not generally applicable for mouse controls.
    /// </summary>
    /// <remarks>
    /// In other game UI code this is more typically referred to as "snap", since there is no true input focus. However,
    /// focus is the more general term and better explains what is happening with e.g. a text box.
    /// </remarks>
    bool IsFocusable { get; }

    /// <summary>
    /// The current layout parameters, which determine how <see cref="Measure"/> will behave.
    /// </summary>
    LayoutParameters Layout { get; set; }

    /// <summary>
    /// Simple name for this view, used in log/debug output; does not affect behavior.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// The true computed layout size resulting from a single <see cref="Measure"/> pass.
    /// </summary>
    Vector2 OuterSize { get; }

    /// <summary>
    /// The user-defined tags for this view.
    /// </summary>
    Tags Tags { get; }

    /// <summary>
    /// Localized tooltip to display on hover, if any.
    /// </summary>
    string Tooltip { get; set; }

    /// <summary>
    /// Drawing visibility for this view.
    /// </summary>
    Visibility Visibility { get; set; }

    /// <summary>
    /// Z order for this view within its direct parent. Higher indices draw later (on top).
    /// </summary>
    int ZIndex { get; set; }

    /// <summary>
    /// Draws the content for this view.
    /// </summary>
    /// <remarks>
    /// Drawing always happens after the measure pass, so <see cref="ContentSize"/> should be known and stable at this
    /// time, as long as the implementation itself is stable. Note that a position is not provided because the
    /// <see cref="ISpriteBatch"/> is pre-transformed; the top-left coordinates of this view are always (0, 0).
    /// </remarks>
    /// <param name="b">Sprite batch to hold the drawing output.</param>
    void Draw(ISpriteBatch b);

    /// <summary>
    /// Finds the next focusable component in a given direction that does <i>not</i> overlap with a current position.
    /// </summary>
    /// <remarks>
    /// If <paramref name="position"/> is out of bounds, it does not necessarily mean that the view should return
    /// <c>null</c>; the expected result depends on the <paramref name="direction"/> also. The base case is when the
    /// focus position is already in bounds, and in this case a view should return whichever view can be reached by
    /// moving from the edge of that view along a straight line in the specified <c>direction</c>. However, focus search
    /// is recursive and the result should reflect the "best" candidate for focus if the cursor were to move <i>into</i>
    /// this view's bounds. For example, in a 1D horizontal layout the rules might be:
    /// <list type="bullet">
    /// <item>If the <paramref name="direction"/> is <see cref="Direction.East"/>, and the position's X value is
    /// negative, then the result should the leftmost focusable child, regardless of Y value.</item>
    /// <item>If the direction is <see cref="Direction.South"/>, and the X position is within the view's horizontal
    /// bounds, and the Y value is negative or greater than the view's height, then result should be whichever child
    /// intersects with that X position.</item>
    /// <item>If the direction is <see cref="Direction.West"/> and the X position is negative, or the direction is
    /// <see cref="Direction.East"/> and the X position is greater than the view's width, then the result should be
    /// <c>null</c> as there is literally nothing the view knows about in that direction.</item>
    /// </list>
    /// There are no strict rules for how a view performs focus search, but in general it is assumed that a view
    /// implementation understands its own layout and can accommodate accordingly; for example, a grid would follow
    /// essentially the same rules as our "list" example above, with additional considerations for navigating rows.
    /// "Ragged" 2D layouts might have complex rules requiring explicit neighbors, and therefore are typically easier
    /// to implement as nested lanes.
    /// </remarks>
    /// <param name="position">The current cursor position, relative to this view. May have dimensions that are negative
    /// or outside the view bounds, indicating that the cursor is not currently within the view.</param>
    /// <param name="direction">The direction of cursor movement.</param>
    /// <returns>The next focusable view reached by moving in the specified <paramref name="direction"/>, or <c>null</c>
    /// if there are no focusable descendants that are possible to reach in that direction.</returns>
    FocusSearchResult? FocusSearch(Vector2 position, Direction direction);

    /// <summary>
    /// Finds the child at a given position.
    /// </summary>
    /// <param name="position">The search position, relative to the view's top-left coordinate.</param>
    /// <returns>The view at <paramref name="position"/>, or <c>null</c> if there is no match.</returns>
    ViewChild? GetChildAt(Vector2 position);

    /// <summary>
    /// Computes or retrieves the position of a given direct child.
    /// </summary>
    /// <remarks>
    /// Implementation of this may be O(N) and therefore it should not be called every frame; it is intended for use in
    /// directional movement and other user-initiated events.
    /// </remarks>
    /// <param name="childView">The child of this view.</param>
    /// <returns>The local coordinates of the <paramref name="childView"/>, or <c>null</c> if the
    /// <paramref name="childView"/> is not a current or direct child.</returns>
    Vector2? GetChildPosition(IView childView);

    /// <summary>
    /// Gets the current children of this view.
    /// </summary>
    IEnumerable<ViewChild> GetChildren();

    /// <summary>
    /// Checks whether or not the view is dirty - i.e. requires a new layout with a full <see cref="Measure"/>.
    /// </summary>
    /// <remarks>
    /// Typically, a view will be considered dirty if and only if one of the following are true:
    /// <list type="bullet">
    /// <item>The <see cref="Layout"/> has changed</item>
    /// <item>The content has changed in a way that could affect layout, e.g. the text has changed in a
    /// <see cref="LengthType.Content"/> configuration</item>
    /// <item>The <c>availableSize</c> is not the same as the previously-seen value (see remarks in
    /// <see cref="Measure"/>)</item>
    /// </list>
    /// A correct implementation is important for performance, as full layout can be very expensive to run on every
    /// frame.
    /// </remarks>
    /// <returns><c>true</c> if the view must be measured again; otherwise <c>false</c>.</returns>
    bool IsDirty();

    /// <summary>
    /// Computes the size of this view in the current pass, and stores the result in <see cref="ContentSize"/>.
    /// </summary>
    /// <remarks>
    /// Most views should save the value of <paramref name="availableSize"/> for use in <see cref="IsDirty"/> checks.
    /// </remarks>
    /// <param name="availableSize">The width/height that is still available in the container/parent.</param>
    /// <returns>Whether or not any layout was performed as a result of this pass. Callers may use this to propagate
    /// layout back up the tree, or perform expensive follow-up actions.</returns>
    bool Measure(Vector2 availableSize);

    /// <summary>
    /// Called when a click is received within this view's bounds.
    /// </summary>
    /// <param name="e">The event data.</param>
    void OnClick(ClickEventArgs e);

    /// <summary>
    /// Called when the view is being dragged (mouse moved while left button held).
    /// </summary>
    /// <param name="e">The event data.</param>
    void OnDrag(PointerEventArgs e);

    /// <summary>
    /// Called when the mouse button is released after at least one <see cref="OnDrag"/>.
    /// </summary>
    /// <param name="e">The event data.</param>
    void OnDrop(PointerEventArgs e);

    /// <summary>
    /// Called when a pointer movement related to this view occurs.
    /// </summary>
    /// <remarks>
    /// This can either be the pointer entering the view, leaving the view, or moving within the view. The method is
    /// used to trigger events such as <see cref="View.PointerEnter"/> and <see cref="View.PointerLeave"/>.
    /// </remarks>
    /// <param name="e">The event data.</param>
    void OnPointerMove(PointerMoveEventArgs e);

    /// <summary>
    /// Called when a wheel event is received within this view's bounds.
    /// </summary>
    /// <param name="e">The event data.</param>
    void OnWheel(WheelEventArgs e);

    /// <summary>
    /// Attempts to scroll the specified target into view, including all of its ancestors, if not fully in view.
    /// </summary>
    /// <param name="path">The path to the view that should be visible, starting from (and not including) this view;
    /// each element has the local position within its own parent, so the algorithm can run recursively. This is a slice
    /// of the same path returned in a <see cref="FocusSearchResult"/>.</param>
    /// <param name="distance">The total distance that was scrolled, including distance scrolled by descendants.</param>
    /// <returns>Whether or not the scroll was successful; <c>false</c> prevents the request from bubbling.</returns>
    bool ScrollIntoView(IEnumerable<ViewChild> path, out Vector2 distance);
}
