﻿using System.ComponentModel;
using Microsoft.Xna.Framework;
using StardewUI.Data;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;

namespace StardewUI;

/// <summary>
/// Represents some arbitrary UI element or layout.
/// </summary>
public interface IView : IDisposable, INotifyPropertyChanged
{
    /// <summary>
    /// Event raised when any button on any input device is pressed.
    /// </summary>
    /// <remarks>
    /// Only the views in the current focus path should receive these events.
    /// </remarks>
    event EventHandler<ButtonEventArgs> ButtonPress;

    /// <summary>
    /// Event raised when a button is being held while the view is in focus, and has been held long enough since the
    /// initial <see cref="ButtonPress"/> or the previous <c>ButtonRepeat</c> to trigger a repeated press.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Because the game has its own logic to repeat key presses, which would cause <see cref="ButtonPress"/> to fire
    /// repeatedly, this event generally applies only to the controller; that is, it exists to allow callers to decide
    /// whether they want the handler to repeat while the button is held or to only fire when first pressed, providing
    /// slightly more control than keyboard events whose repetition is up to the whims of the vanilla game.
    /// </para>
    /// <para>
    /// Only the views in the current focus path should receive these events.
    /// </para>
    /// </remarks>
    event EventHandler<ButtonEventArgs> ButtonRepeat;

    /// <summary>
    /// Event raised when the view receives a click initiated from any button.
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
    /// Event raised when the view receives a click initiated from the left mouse button, or the controller's action
    /// button (A).
    /// </summary>
    /// <remarks>
    /// Using this event is a shortcut for handling <see cref="Click"/> and checking for
    /// <see cref="ClickEventArgs.IsPrimaryButton"/>.
    /// </remarks>
    event EventHandler<ClickEventArgs> LeftClick;

    /// <summary>
    /// Event raised when the pointer enters the view.
    /// </summary>
    event EventHandler<PointerEventArgs> PointerEnter;

    /// <summary>
    /// Event raised when the pointer exits the view.
    /// </summary>
    event EventHandler<PointerEventArgs> PointerLeave;

    /// <summary>
    /// Event raised when the pointer moves within the view.
    /// </summary>
    event EventHandler<PointerMoveEventArgs> PointerMove;

    /// <summary>
    /// Event raised when the view receives a click initiated from the right mouse button, or the controller's tool-use
    /// button (X).
    /// </summary>
    /// <remarks>
    /// Using this event is a shortcut for handling <see cref="Click"/> and checking for
    /// <see cref="ClickEventArgs.IsSecondaryButton"/>.
    /// </remarks>
    event EventHandler<ClickEventArgs> RightClick;

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
    /// Origin position for the <see cref="ClipSize"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If clipping is enabled by specifying a <see cref="ClipSize"/>, and the computed size of the clipping rectangle
    /// is not exactly equal to the view's <see cref="OuterSize"/>, then this determines how it will be aligned relative
    /// to this view's boundaries.
    /// </para>
    /// <para>
    /// The default origin is the view's top-left corner (0, 0). This property has no effect unless the view's
    /// <see cref="ClipSize"/> is also defined.
    /// </para>
    /// </remarks>
    NineGridPlacement? ClipOrigin { get; set; }

    /// <summary>
    /// Size of the clipping rectangle, outside which content will not be displayed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is defined as a layout, but unlike the view's <see cref="Layout"/>, it is not computed against the
    /// available size provided by the parent; instead, its reference size is the view's <see cref="OuterSize"/>.
    /// </para>
    /// <para>
    /// A common scenario is to set this to <see cref="LayoutParameters.Fill"/> in order to prevent drawing outside the
    /// view's own boundaries, i.e. as an equivalent to CSS <c>overflow: hidden</c>. Note however that clipping occurs
    /// during the drawing phase, so a smaller clip region does not result in a smaller layout; the view will still have
    /// the same size it would have had without any clipping, but only part of it will actually get drawn. This can also
    /// be used intentionally to create some animated visual effects such as slides and wipes.
    /// </para>
    /// </remarks>
    LayoutParameters? ClipSize { get; set; }

    /// <summary>
    /// The true bounds of this view's content; i.e. <see cref="ActualBounds"/> excluding margins.
    /// </summary>
    Bounds ContentBounds { get; }

    /// <summary>
    /// Contains the bounds of all floating elements in this view tree, including the current view and all descendants.
    /// </summary>
    IEnumerable<Bounds> FloatingBounds { get; }

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
    /// Opacity (alpha level) of the view.
    /// </summary>
    /// <remarks>
    /// Affects this view and all descendants; used to control opacity of an entire control or layout area.
    /// </remarks>
    float Opacity { get; set; }

    /// <summary>
    /// The true computed layout size resulting from a single <see cref="Measure"/> pass.
    /// </summary>
    Vector2 OuterSize { get; }

    /// <summary>
    /// Whether this view should receive pointer events like <see cref="Click"/> or <see cref="Drag"/>.
    /// </summary>
    /// <remarks>
    /// By default, all views receive pointer events; this may be disabled for views that intentionally overlap other
    /// views but shouldn't block their input, such as local non-modal overlays.
    /// </remarks>
    bool PointerEventsEnabled { get; set; }

    /// <summary>
    /// Pointer style to use when this view is hovered.
    /// </summary>
    /// <remarks>
    /// As with <see cref="Tooltip"/>, the lowest-level view takes precedence over any higher-level views.
    /// </remarks>
    PointerStyle PointerStyle { get; set; }

    /// <summary>
    /// If set to an axis, specifies that when any child of the view is scrolled into view (using
    /// <see cref="ScrollIntoView"/>), then this entire view should be scrolled along with it.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For example, if the view lays out children horizontally, and some children may occupy only a very small amount
    /// of space near the top while others are much taller vertically or positioned near the bottom, it might be
    /// desirable to configure this with <see cref="Orientation.Vertical"/>, so that the entire "row" is positioned
    /// within the scrollable viewport.
    /// </para>
    /// <para>
    /// In other words, "if any part of me is made visible via scrolling, then all of me should be visible".
    /// </para>
    /// </remarks>
    Orientation? ScrollWithChildren { get; set; }

    /// <summary>
    /// The user-defined tags for this view.
    /// </summary>
    Tags Tags { get; }

    /// <summary>
    /// Tooltip data to display on hover, if any.
    /// </summary>
    TooltipData? Tooltip { get; set; }

    /// <summary>
    /// Local transformation to apply to this view, including any children and floating elements.
    /// </summary>
    Transform? Transform { get; set; }

    /// <summary>
    /// Relative origin position for any <see cref="Transform"/> on this view.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Expects a value that represents the fraction of the view's computed layout size. For example, <c>(0, 0)</c> is
    /// the top left, <c>(0.5, 0.5)</c> is the center, and <c>1, 1</c> is the bottom right. <c>null</c> values are
    /// equivalent to <see cref="Vector2.Zero"/>.
    /// </para>
    /// <para>
    /// Origins are not inherited; each view defines its own origin for its specific transform.
    /// </para>
    /// </remarks>
    Vector2? TransformOrigin { get; set; }

    /// <summary>
    /// Drawing visibility for this view.
    /// </summary>
    Visibility Visibility { get; set; }

    /// <summary>
    /// Z order for this view within its direct parent. Higher indices draw later (on top).
    /// </summary>
    int ZIndex { get; set; }

    /// <summary>
    /// Checks if a given point, relative to the view's origin, is within its bounds.
    /// </summary>
    /// <param name="point">The point to test.</param>
    /// <returns><c>true</c> if <paramref name="point"/> is within the view bounds; otherwise <c>false</c>.</returns>
    bool ContainsPoint(Vector2 point);

    /// <summary>
    /// Draws the content for this view.
    /// </summary>
    /// <remarks>
    /// No positional argument is included because <see cref="ISpriteBatch"/> handles its own transformations; the
    /// top-left coordinates of this view are always (0, 0).
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
    /// <remarks>
    /// If multiple children overlap the same position, then this returns the topmost child, i.e. with the highest
    /// <see cref="ZIndex"/>.
    /// </remarks>
    /// <param name="position">The search position, relative to the view's top-left coordinate.</param>
    /// <param name="preferFocusable"><c>true</c> to prioritize a focusable child over a non-focusable child with a higher
    /// z-index in case of overlap; <c>false</c> to always use the topmost child.</param>
    /// <param name="requirePointerEvents">Whether to exclude views whose <see cref="PointerEventsEnabled"/> is
    /// currently <c>false</c>.</param>
    /// <returns>The view at <paramref name="position"/>, or <c>null</c> if there is no match.</returns>
    ViewChild? GetChildAt(Vector2 position, bool preferFocusable = false, bool requirePointerEvents = false);

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
    /// <param name="includeFloatingElements">Whether to include views that are not direct children, but instead
    /// members of the floating elements collection of an <see cref="IFloatContainer"/> implementation.</param>
    IEnumerable<ViewChild> GetChildren(bool includeFloatingElements = true);

    /// <summary>
    /// Finds all children at a given position.
    /// </summary>
    /// <param name="position">The search position, relative to the view's top-left coordinate.</param>
    /// <returns>A sequence of views at the specified <paramref name="position"/>, in front-to-back (reverse
    /// <see cref="ZIndex"/>) order.</returns>
    IEnumerable<ViewChild> GetChildrenAt(Vector2 position);

    /// <summary>
    /// Gets the direct child that should contain cursor focus when a menu or overlay containing this view is first
    /// opened.
    /// </summary>
    /// <returns>The child view and position where initial focus should be, either directly or indirectly as a result of
    /// a descendant receiving focus. If this <see cref="IView"/> is itself <see cref="IsFocusable"/>, then the result
    /// may be a <see cref="ViewChild"/> referencing this view.</returns>
    ViewChild? GetDefaultFocusChild();

    /// <summary>
    /// Checks if the view has content or elements that are all or partially outside the <see cref="ActualBounds"/>.
    /// </summary>
    /// <remarks>
    /// This may be the case for e.g. floating elements, and covers not only the view's immediate content/children but
    /// also that of any descendants.
    /// </remarks>
    bool HasOutOfBoundsContent();

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
    /// Checks if the view is effectively visible, i.e. if it has anything to draw.
    /// </summary>
    /// <remarks>
    /// While <see cref="Visibility"/> acts as a master on/off switch, there may be many other reasons for a view not
    /// to have any visible content, such as views with zero <see cref="Opacity"/>, layout views with no visible
    /// children, or labels or images with no current text or sprite.
    /// </remarks>
    /// <param name="position">Optional position at which to test for visibility. If not specified, the result indicates
    /// whether any part of the view is visible.</param>
    bool IsVisible(Vector2? position = null);

    /// <summary>
    /// Performs layout on this view, updating its <see cref="OuterSize"/>, <see cref="ActualBounds"/> and
    /// <see cref="ContentBounds"/>, and arranging any children in their respective positions.
    /// </summary>
    /// <remarks>
    /// Most views should save the value of <paramref name="availableSize"/> for use in <see cref="IsDirty"/> checks.
    /// </remarks>
    /// <param name="availableSize">The width/height that is still available in the container/parent.</param>
    /// <returns>Whether or not any layout was performed as a result of this pass. Callers may use this to propagate
    /// layout back up the tree, or perform expensive follow-up actions.</returns>
    bool Measure(Vector2 availableSize);

    /// <summary>
    /// Called when a button press is received while this view is in the focus path.
    /// </summary>
    /// <param name="e">The event data.</param>
    void OnButtonPress(ButtonEventArgs e);

    /// <summary>
    /// Called when a button press is first received, and at recurring intervals thereafter, for as long as the button
    /// is held and this view remains in the focus path.
    /// </summary>
    /// <param name="e">The event data.</param>
    void OnButtonRepeat(ButtonEventArgs e);

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
    /// Runs on every update tick.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Provided as an escape hatch for very unusual scenarios like responding to flips in the game's
    /// <see cref="Options.gamepadControls"/> state.
    /// </para>
    /// <para>
    /// <b>Override this at your own extreme peril.</b> Frequently performing any layout-affecting logic in this
    /// function can negate the performance benefits of a retained-mode UI and cause the UI to become sluggish or even
    /// completely unresponsive.  Do not use it for animation; use <see cref="Animation.Animator"/> instead.
    /// </para>
    /// </remarks>
    /// <param name="elapsed">Time elapsed since last game tick.</param>
    void OnUpdate(TimeSpan elapsed);

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
