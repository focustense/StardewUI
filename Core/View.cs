﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewUI.Data;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;

namespace StardewUI;

/// <summary>
/// Base class for typical widgets wanting to implement <see cref="IView"/>.
/// </summary>
/// <remarks>
/// Use of this class isn't required, but provides some useful behaviors so that view types don't need to keep
/// re-implementing them, such as a standard <see cref="Measure"/> implementation that skips unnecessary layouts.
/// </remarks>
public abstract class View : IView, IFloatContainer
{
    /// <summary>
    /// Event raised when any button on any input device is pressed.
    /// </summary>
    /// <remarks>
    /// Only the views in the current focus path should receive these events.
    /// </remarks>
    public event EventHandler<ButtonEventArgs>? ButtonPress;

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
    public event EventHandler<ButtonEventArgs>? ButtonRepeat;

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
    /// Event raised when the pointer moves within the view.
    /// </summary>
    public event EventHandler<PointerMoveEventArgs>? PointerMove;

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Event raised when the view receives a click initiated from the right mouse button, or the controller's tool-use
    /// button (X).
    /// </summary>
    public event EventHandler<ClickEventArgs>? RightClick;

    /// <summary>
    /// Event raised when the scroll wheel moves.
    /// </summary>
    public event EventHandler<WheelEventArgs>? Wheel;

    /// <summary>
    /// Indicates whether a UI-initiated drawing operation is in progress, from any view.
    /// </summary>
    /// <remarks>
    /// This state is primarily used by Framework patches as a way to limit their effective scope and avoid interfering
    /// with vanilla or 3P draws.
    /// </remarks>
    internal static bool IsDrawing { get; private set; }

    /// <inheritdoc/>
    public Bounds ActualBounds => GetActualBounds();

    /// <summary>
    /// The layout size (not edge thickness) of the entire drawn area including the border, i.e. the
    /// <see cref="InnerSize"/> plus any borders defined in <see cref="GetBorderThickness"/>. Does not include the
    /// <see cref="Margin"/>.
    /// </summary>
    public Vector2 BorderSize => InnerSize + GetBorderThickness().Total;

    /// <inheritdoc />
    public NineGridPlacement? ClipOrigin
    {
        get => clipOrigin;
        set
        {
            if (value != clipOrigin)
            {
                clipOrigin = value;
                clipBounds = null; // Recalculate on next draw
                OnPropertyChanged(nameof(ClipOrigin));
            }
        }
    }

    /// <inheritdoc />
    public LayoutParameters? ClipSize
    {
        get => clipSize;
        set
        {
            if (value != clipSize)
            {
                clipSize = value;
                clipBounds = null; // Recalculate on next draw
                OnPropertyChanged(nameof(ClipSize));
            }
        }
    }

    /// <inheritdoc/>
    public Bounds ContentBounds => GetContentBounds();

    /// <summary>
    /// The size of the view's content, which is drawn inside the padding. Subclasses set this in their
    /// <see cref="OnMeasure"/> method and padding, margins, etc. are handled automatically.
    /// </summary>
    public Vector2 ContentSize
    {
        get => contentSize;
        protected set
        {
            if (value != contentSize)
            {
                contentSize = value;
                OnPropertyChanged(nameof(ContentSize));
            }
        }
    }

    /// <summary>
    /// Whether or not this view should fire drag events such as <see cref="DragStart"/> and <see cref="Drag"/>.
    /// </summary>
    public bool Draggable
    {
        get => draggable;
        set
        {
            if (value != draggable)
            {
                draggable = value;
                OnPropertyChanged(nameof(Draggable));
            }
        }
    }

    /// <inheritdoc/>
    public IEnumerable<Bounds> FloatingBounds => GetFloatingBounds();

    /// <inheritdoc />
    public IList<FloatingElement> FloatingElements
    {
        get => floatingElements;
        set
        {
            if (!value.SequenceEqual(floatingElements))
            {
                floatingElements = new(value);
                OnPropertyChanged(nameof(FloatingElements));
            }
        }
    }

    /// <summary>
    /// Whether or not the view should be able to receive focus. Applies only to this specific view, not its children.
    /// </summary>
    /// <remarks>
    /// All views are non-focusable by default and must have their focus enabled explicitly. Subclasses may choose to
    /// override the default value if they should always be focusable.
    /// </remarks>
    public virtual bool Focusable
    {
        get => isFocusable;
        set
        {
            if (value != isFocusable)
            {
                isFocusable = value;
                OnPropertyChanged(nameof(Focusable));
                OnPropertyChanged(nameof(IView.IsFocusable));
            }
        }
    }

    /// <summary>
    /// The size allocated to the entire area inside the border, i.e. <see cref="ContentSize"/> plus any
    /// <see cref="Padding"/>. Does not include border or <see cref="Margin"/>.
    /// </summary>
    public Vector2 InnerSize => ContentSize + Padding.Total;

    /// <inheritdoc />
    [Obsolete("Use Focusable instead of IsFocusable when interacting directly with a concrete View.")]
    public bool IsFocusable => Focusable;

    /// <summary>
    /// Layout settings for this view; determines how its dimensions will be computed.
    /// </summary>
    public LayoutParameters Layout
    {
        get => layout.Value;
        set
        {
            if (layout.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Layout));
            }
        }
    }

    /// <summary>
    /// Margins (whitespace outside border) for this view.
    /// </summary>
    public Edges Margin
    {
        get => margin.Value;
        set
        {
            if (margin.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Margin));
            }
        }
    }

    /// <summary>
    /// Simple name for this view, used in log/debug output; does not affect behavior.
    /// </summary>
    public string Name
    {
        get => name;
        set
        {
            if (value != name)
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    /// <summary>
    /// Opacity (alpha level) of the view.
    /// </summary>
    /// <remarks>
    /// Affects this view and all descendants; used to control opacity of an entire control or layout area.
    /// </remarks>
    public float Opacity
    {
        get => opacity;
        set
        {
            var newOpacity = Math.Clamp(value, 0f, 1f);
            if (newOpacity != opacity)
            {
                opacity = newOpacity;
                OnPropertyChanged(nameof(Opacity));
            }
        }
    }

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
        set
        {
            if (padding.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Padding));
            }
        }
    }

    /// <summary>
    /// Whether this view should receive pointer events like <see cref="Click"/> or <see cref="Drag"/>.
    /// </summary>
    /// <remarks>
    /// By default, all views receive pointer events; this may be disabled for views that intentionally overlap other
    /// views but shouldn't block their input, such as local non-modal overlays.
    /// </remarks>
    public bool PointerEventsEnabled
    {
        get => pointerEventsEnabled;
        set
        {
            if (value != pointerEventsEnabled)
            {
                pointerEventsEnabled = value;
                OnPropertyChanged(nameof(PointerEventsEnabled));
            }
        }
    }

    /// <inheritdoc />
    public PointerStyle PointerStyle
    {
        get => pointerStyle;
        set
        {
            if (value != pointerStyle)
            {
                pointerStyle = value;
                OnPropertyChanged(nameof(PointerStyle));
            }
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// If set to an axis, specifies that when any child of the view is scrolled into view (using
    /// <see cref="ScrollIntoView"/>), then this entire view should be scrolled along with it.
    /// </summary>
    public Orientation? ScrollWithChildren
    {
        get => scrollWithChildren;
        set
        {
            if (value != scrollWithChildren)
            {
                scrollWithChildren = value;
                OnPropertyChanged(nameof(ScrollWithChildren));
            }
        }
    }

    /// <summary>
    /// The user-defined tags for this view.
    /// </summary>
    public Tags Tags
    {
        get => tags;
        set
        {
            if (!value.Equals(tags))
            {
                tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }
    }

    /// <summary>
    /// Local transformation to apply to this view, including any children and floating elements.
    /// </summary>
    public Transform? Transform
    {
        get => transform;
        set
        {
            if (value != transform)
            {
                transform = value;
                OnPropertyChanged(nameof(Transform));
            }
        }
    }

    /// <inheritdoc />
    public Vector2? TransformOrigin
    {
        get => transformOrigin;
        set
        {
            if (value != transformOrigin)
            {
                transformOrigin = value;
                OnPropertyChanged(nameof(TransformOrigin));
            }
        }
    }

    /// <summary>
    /// Localized tooltip to display on hover, if any.
    /// </summary>
    public TooltipData? Tooltip
    {
        get => tooltip;
        set
        {
            if (value != tooltip)
            {
                tooltip = value;
                OnPropertyChanged(nameof(Tooltip));
            }
        }
    }

    /// <summary>
    /// Visibility for this view.
    /// </summary>
    public Visibility Visibility
    {
        get => visibility;
        set
        {
            if (value != visibility)
            {
                visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }
    }

    /// <summary>
    /// Z order for this view within its direct parent. Higher indices draw later (on top).
    /// </summary>
    public int ZIndex
    {
        get => zIndex;
        set
        {
            if (value != zIndex)
            {
                zIndex = value;
                OnPropertyChanged(nameof(ZIndex));
            }
        }
    }

    /// <summary>
    /// Whether the specific view type handles its own opacity.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Subclasses can override this to provide their own, typically better optimized version of opacity; i.e. a basic
    /// text or image view could simply multiply its own background/foreground colors without requiring multiple render
    /// targets to handle the blending.
    /// </para>
    /// <para>
    /// Any <see cref="FloatingElements"/> will still use the default opacity implementation.
    /// </para>
    /// </remarks>
    protected virtual bool HandlesOpacity => false;

    /// <summary>
    /// Pixel offset of the view's content, which is applied to all pointer events and child queries.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A non-zero offset means that the nominal positions of any view children (e.g. as obtained from
    /// <see cref="GetChildren"/>) are different from their actual drawing positions on screen, for example in the case
    /// of a <see cref="Widgets.ScrollContainer"/> that is not at the default scroll position.
    /// </para>
    /// <para>
    /// If a view will internally shift content in this way without affecting layout, it should update the
    /// <see cref="LayoutOffset"/> property to ensure correctness of pointer events and coordinate-related queries such
    /// as <see cref="GetLocalChildrenAt(Vector2)"/>, <b>instead of</b> attempting to correct for that offset locally.
    /// </para>
    /// </remarks>
    protected virtual Vector2 LayoutOffset => Vector2.Zero;

    /// <summary>
    /// The most recent size used in a <see cref="Measure"/> pass. Used for additional dirty checks.
    /// </summary>
    protected Vector2 LastAvailableSize { get; private set; } = Vector2.Zero;

    private readonly DirtyTracker<LayoutParameters> layout = new(new());
    private readonly DirtyTracker<Edges> margin = new(Edges.NONE);
    private readonly DirtyTracker<Edges> padding = new(Edges.NONE);

    private Bounds? clipBounds;
    private NineGridPlacement? clipOrigin;
    private LayoutParameters? clipSize;
    private Vector2 contentSize;
    private bool draggable;
    private IView? draggingView;
    private ObservableCollection<FloatingElement> floatingElements = [];
    private bool hasChildrenWithOutOfBoundsContent;
    private bool isDisposed;
    private bool isDragging;
    private bool isFocusable;
    private string name;
    private float opacity = 1f;
    private RenderTarget2D? opacitySourceTarget;
    private RenderTarget2D? opacityDestinationTarget;
    private bool pointerEventsEnabled = true;
    private PointerStyle pointerStyle = PointerStyle.Default;
    private Vector2 previousLayoutOffset;
    private WeakViewChild? previousPointerTarget;
    private Orientation? scrollWithChildren;
    private Tags tags = new();
    private TooltipData? tooltip = null;
    private Transform? transform;
    private Vector2? transformOrigin;
    private Visibility visibility;
    private bool wasPointerInBounds;
    private int zIndex;

    /// <summary>
    /// Initializes a new instance of <see cref="View"/>.
    /// </summary>
    /// <remarks>
    /// The view's <see cref="Name"/> will default to the simple name of its most derived <see cref="Type"/>.
    /// </remarks>
    public View()
    {
        name = GetType().Name;
    }

    /// <inheritdoc />
    public bool ContainsPoint(Vector2 point)
    {
        return ActualBounds.ContainsPoint(point)
            || FloatingBounds.Any(bounds => bounds.ContainsPoint(point))
            || (hasChildrenWithOutOfBoundsContent && GetChildren().Any(c => c.ContainsPoint(point)));
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }
        isDisposed = true;
        foreach (var child in GetLocalChildren())
        {
            child.View.Dispose();
        }
        opacitySourceTarget?.Dispose();
        opacitySourceTarget = null;
        opacityDestinationTarget?.Dispose();
        opacityDestinationTarget = null;
        OnDispose();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Drawing always happens after the measure pass, so <see cref="ContentSize"/> should be known and stable at this
    /// time, as long as the implementation itself is stable.
    /// </remarks>
    public void Draw(ISpriteBatch b)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(Draw));
        if (Visibility != Visibility.Visible || Opacity == 0)
        {
            return;
        }

        using var _drawing = new DrawingState();

        if (clipBounds is null && clipSize is not null)
        {
            UpdateClipBounds();
        }
        using var _clip = clipBounds is not null ? b.Clip(clipBounds.Truncate()) : null;

        // Local transforms can be applied before or after creating the render target, it makes no difference. However,
        // doing it before makes it simpler to separate the main content logic from floating element logic.
        b.Translate(Margin.Left, Margin.Top);
        if (Transform is not null)
        {
            var origin = TransformOrigin.HasValue
                ? new TransformOrigin(TransformOrigin.Value, TransformOrigin.Value * OuterSize)
                : null;
            b.Transform(Transform, origin);
        }

        if (Opacity == 1)
        {
            DrawContent();
            return;
        }

        // When the view type wants to use its own opacity implementation, we can skip the render target shenanigans
        // below for the main content and MAY be able to skip them entirely, but it depends on the presence of floating
        // elements, as nothing prevents e.g. a Label or Image from having them, unusual as that might be.
        if (HandlesOpacity)
        {
            DrawContent(includeFloatingElements: false);
            if (FloatingElements.Count == 0)
            {
                return;
            }
        }

        // The extremely limited and frankly obtuse blending system in XNA/MonoGame seems to make this effectively
        // impossible to do in one draw, and in fact, probably impossible or at least lacking any obvious method to do
        // in two draws when considering nested transparencies.
        //
        // The reason is that there is no blend mode that combines a "blend factor" AND source alpha, it is either one
        // or the other. Any mechanism that tries to blend directly on the back buffer will therefore either have to
        // ignore the opacity value (making this all moot) or lose the original source alpha.
        //
        // So we need to use TWO temporary render targets and three steps:
        //
        // 1. Draw to a "source" target, preserving the source alpha;
        // 2. Draw that source to a "destination" target, multiplying everything (including alpha) by blend factor;
        // 3. Draw the intermediate "destination" to the actual back buffer or parent render target, again using normal
        //    alpha blending that preserves the source alpha.
        //
        // The intermediate step is necessary because we are effectively having to apply the same "math" on the
        // destination side, i.e. we cannot specify that the destination should be the product of both inverse blend
        // factor AND inverse source alpha, we instead need one intermediate target that has both already applied.
        SetupRenderTarget(b, ref opacitySourceTarget);
        SetupRenderTarget(b, ref opacityDestinationTarget);
        using (b.SetRenderTarget(opacitySourceTarget, Color.Transparent))
        {
            DrawContent(includeMainContent: !HandlesOpacity);
        }
        using (b.SetRenderTarget(opacityDestinationTarget, Color.Transparent))
        {
            using var _blend = b.Blend(
                new BlendState()
                {
                    ColorSourceBlend = Blend.BlendFactor,
                    AlphaSourceBlend = Blend.BlendFactor,
                    ColorDestinationBlend = Blend.Zero,
                    AlphaDestinationBlend = Blend.Zero,
                    BlendFactor = Color.White * Opacity,
                }
            );
            b.Draw(opacitySourceTarget, Vector2.Zero, null);
        }
        b.Draw(opacityDestinationTarget, Vector2.Zero, null);

        void DrawContent(bool includeMainContent = true, bool includeFloatingElements = true)
        {
            if (includeMainContent)
            {
                using (b.SaveTransform())
                {
                    OnDrawBorder(b);
                    var borderThickness = GetBorderThickness();
                    b.Translate(borderThickness.Left + Padding.Left, borderThickness.Top + Padding.Top);
                    OnDrawContent(b);
                }
            }
            if (includeFloatingElements)
            {
                foreach (var floatingElement in FloatingElements)
                {
                    floatingElement.Draw(b);
                }
            }
        }
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// This will first call <see cref="FindFocusableDescendant"/> to see if the specific view type wants to implement
    /// its own focus search. If there is no focusable descendant, then this will return a reference to the current view
    /// if <see cref="Focusable"/> is <c>true</c> and the position is <i>not</i> already within the view's bounds -
    /// meaning, any focusable view can accept focus from any direction, but will not consider itself a result if it is
    /// already focused (since we are trying to "move" focus).
    /// </remarks>
    public FocusSearchResult? FocusSearch(Vector2 position, Direction direction)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(FocusSearch));
        if (!PointerEventsEnabled || Visibility != Visibility.Visible)
        {
            return null;
        }
        var floatingOffset = GetFloatingOffset();
        foreach (var floatingElement in FloatingElements)
        {
            var floatingChild = floatingElement.AsViewChild().Offset(floatingOffset);
            if (!floatingChild.ContainsPoint(position))
            {
                continue;
            }
            var floatingResult = floatingChild.FocusSearch(position, direction);
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
            Focusable
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
            var floatingResult = floatingElement.AsViewChild().Offset(floatingOffset).FocusSearch(position, direction);
            if (floatingResult is not null)
            {
                return floatingResult;
            }
        }
        LogFocusSearch($"View '{Name}' found no focusable descendants matching the query.");
        return null;
    }

    /// <inheritdoc />
    public ViewChild? GetChildAt(Vector2 position, bool preferFocusable = false, bool requirePointerEvents = false)
    {
        return GetChildrenAt(position)
            .Where(child => !requirePointerEvents || child.View.PointerEventsEnabled)
            .ZOrderDescending(preferFocusable)
            .FirstOrDefault();
    }

    /// <inheritdoc />
    public Vector2? GetChildPosition(IView childView)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(GetChildPosition));
        return GetChildren()
            .Concat(FloatingElements.Select(fe => fe.AsViewChild()))
            .Where(child => child.View == childView)
            .Select(child => child.Position as Vector2?)
            .FirstOrDefault();
    }

    /// <inheritdoc />
    public IEnumerable<ViewChild> GetChildren(bool includeFloatingElements = true)
    {
        var offset = GetContentOffset();
        var children = GetLocalChildren()
            .Select(viewChild => viewChild with { Position = viewChild.Position + offset });
        if (includeFloatingElements)
        {
            children = children.Concat(FloatingElements.Select(fe => fe.AsViewChild()));
        }
        return children;
    }

    /// <inheritdoc />
    public IEnumerable<ViewChild> GetChildrenAt(Vector2 position)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(GetChildrenAt));
        var contentOffset = GetContentOffset();
        var directChildren = GetLocalChildrenAt(position - contentOffset)
            .Where(child => child.View.Visibility == Visibility.Visible);
        foreach (var child in directChildren)
        {
            yield return child.Offset(contentOffset);
        }
        var floatingOffset = GetFloatingOffset();
        foreach (var floatingElement in FloatingElements)
        {
            var floatingChild = floatingElement.AsViewChild().Offset(floatingOffset);
            if (floatingChild.ContainsPoint(position))
            {
                yield return floatingChild;
            }
        }
    }

    /// <inheritdoc />
    public virtual ViewChild? GetDefaultFocusChild()
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(GetDefaultFocusChild));
        if (PointerEventsEnabled && Focusable)
        {
            return new(this, Vector2.Zero);
        }
        return GetChildren()
            .Where(child => child.View.PointerEventsEnabled && child.View.GetDefaultFocusChild() is not null)
            .FirstOrDefault();
    }

    /// <inheritdoc />
    public bool HasOutOfBoundsContent()
    {
        return hasChildrenWithOutOfBoundsContent
            || FloatingElements.Any(fe => !ActualBounds.ContainsBounds(fe.AsViewChild().GetActualBounds()));
    }

    /// <inheritdoc />
    public bool IsDirty()
    {
        return layout.IsDirty || margin.IsDirty || padding.IsDirty || IsContentDirty();
    }

    /// <inheritdoc />
    public bool IsVisible(Vector2? position = null)
    {
        if (Visibility == Visibility.Hidden || Opacity <= 0)
        {
            return false;
        }
        if (HasOwnContent() && (position is null || ActualBounds.ContainsPoint(position.Value)))
        {
            return true;
        }
        return position is not null
            ? GetChildrenAt(position.Value).Any(child => child.IsVisible(position.Value))
            : GetChildren().Any(child => child.View.IsVisible());
    }

    /// <inheritdoc />
    public bool Measure(Vector2 availableSize)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(Measure));
        if (!IsDirty() && availableSize == LastAvailableSize)
        {
            foreach (var floatingElement in FloatingElements)
            {
                floatingElement.MeasureAndPosition(this, wasParentDirty: false);
            }
            // We don't need to reposition floating views deeper in the hierarchy, since they are positioned relative
            // to non-floating parents, but we do need to give them a chance to run layout again if they are dirty.
            // Since our own floating elements were already handled above, this only applies to descendants.
            foreach (var child in GetLocalChildren())
            {
                MeasureDirtyFloatingElements(child.View);
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
        UpdateClipBounds();
        return true;
    }

    /// <inheritdoc/>
    public virtual void OnButtonPress(ButtonEventArgs e)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(OnButtonPress));
        if (Visibility != Visibility.Visible)
        {
            return;
        }
        DispatchPointerEvent(e, (view, args) => view.OnButtonPress(args));
        if (!e.Handled)
        {
            ButtonPress?.Invoke(this, e);
        }
    }

    /// <inheritdoc/>
    public virtual void OnButtonRepeat(ButtonEventArgs e)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(OnButtonRepeat));
        if (Visibility != Visibility.Visible)
        {
            return;
        }
        DispatchPointerEvent(e, (view, args) => view.OnButtonRepeat(args));
        if (!e.Handled)
        {
            ButtonRepeat?.Invoke(this, e);
        }
    }

    /// <inheritdoc/>
    public virtual void OnClick(ClickEventArgs e)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(OnClick));
        if (Visibility != Visibility.Visible)
        {
            return;
        }
        DispatchPointerEvent(e, (view, args) => view.OnClick(args));
        if (!e.Handled)
        {
            Click?.Invoke(this, e);
        }
        if (!e.Handled && e.IsPrimaryButton())
        {
            LeftClick?.Invoke(this, e);
        }
        else if (!e.Handled && e.IsSecondaryButton())
        {
            RightClick?.Invoke(this, e);
        }
    }

    /// <summary>
    /// Performs additional cleanup when <see cref="Dispose"/> is called.
    /// </summary>
    /// <remarks>
    /// The default implementation is a stub. Subclasses may override this if they require separate cleanup.
    /// </remarks>
    protected virtual void OnDispose() { }

    /// <inheritdoc/>
    public virtual void OnDrag(PointerEventArgs e)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(OnDrag));
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
        using var _ = Diagnostics.Trace.Begin(this, nameof(OnDrop));
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
        using var _ = Diagnostics.Trace.Begin(this, nameof(OnPointerMove));
        if (Visibility != Visibility.Visible)
        {
            return;
        }
        var dispatchArgs =
            LayoutOffset != previousLayoutOffset
                ? new(e.PreviousPosition - previousLayoutOffset + LayoutOffset, e.Position)
                : e;
        previousLayoutOffset = LayoutOffset;
        var currentTarget = GetChildAt(e.Position, requirePointerEvents: true);
        ViewChild? previousTarget = null;
        previousPointerTarget?.TryResolve(out previousTarget);
        previousPointerTarget = currentTarget?.AsWeak();
        if (currentTarget != previousTarget && previousTarget is not null)
        {
            DispatchPointerEvent(previousTarget, dispatchArgs, (view, args) => view.OnPointerMove(args));
            if (e.Handled)
            {
                return;
            }
        }

        if (currentTarget is not null)
        {
            DispatchPointerEvent(currentTarget, dispatchArgs, (view, args) => view.OnPointerMove(args));
            if (e.Handled)
            {
                return;
            }
        }

        // For self checks, don't adjust previous position, as offset should only apply to inner content.
        var isPointerInBounds = ContainsPoint(e.Position);
        if (isPointerInBounds)
        {
            if (wasPointerInBounds)
            {
                PointerMove?.Invoke(this, e);
            }
            else
            {
                PointerEnter?.Invoke(this, e);
            }
        }
        else if (!isPointerInBounds && wasPointerInBounds)
        {
            PointerLeave?.Invoke(this, e);
        }
        wasPointerInBounds = isPointerInBounds;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// When overriding <see cref="View.OnUpdate"/>, be sure to call <c>base.OnUpdate()</c> to ensure that any view
    /// children also receive their updates.
    /// </remarks>
    public virtual void OnUpdate(TimeSpan elapsed)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(OnUpdate));
        foreach (var child in GetChildren())
        {
            child.View.OnUpdate(elapsed);
        }
    }

    /// <inheritdoc/>
    public virtual void OnWheel(WheelEventArgs e)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(OnWheel));
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
        using var _ = Diagnostics.Trace.Begin(this, nameof(ScrollIntoView));
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

    /// <inheritdoc />
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
    /// specified <paramref name="contentPosition"/>. Views can override this to provide optimized implementations for
    /// their layout, or handle overlapping views.
    /// </remarks>
    /// <param name="contentPosition">The search position, relative to where this view's content starts (after applying
    /// margin, borders and padding).</param>
    /// <returns>The views at the specified <paramref name="contentPosition"/>, in original layout order.</returns>
    protected virtual IEnumerable<ViewChild> GetLocalChildrenAt(Vector2 contentPosition)
    {
        return GetLocalChildren().Where(child => child.ContainsPoint(contentPosition));
    }

    /// <summary>
    /// Checks if this view displays its own content, independent of any floating elements or children.
    /// </summary>
    /// <remarks>
    /// This is used by <see cref="IsVisible"/> to determine whether children need to be searched. If a view provides
    /// its own content, e.g. a label or image displaying text or a sprite, or a frame displaying a background/border,
    /// then the entire view's bounds are understood to have visible content. Otherwise, the view is only considered
    /// visible as a whole if at least one child is visible, and is only visible at any given point if there is an
    /// intersecting child at that point.
    /// </remarks>
    protected virtual bool HasOwnContent()
    {
        return true;
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

    /// <summary>
    /// Outputs a debug log entry with the current view type, name and specified message.
    /// </summary>
    /// <remarks>
    /// Used primarily for debugging focus searches and requires the <c>DEBUG_FOCUS_SEARCH</c> compiler flag.
    /// </remarks>
    /// <param name="message">The message to log in addition to the view type and name.</param>
    [Conditional("DEBUG_FOCUS_SEARCH")]
    protected void LogFocusSearch(string message)
    {
        Logger.Log($"[{GetType().Name}:{Name}] {message}", LogLevel.Debug);
    }

    private void MeasureDirtyFloatingElements(IView root)
    {
        if (root is IFloatContainer floatContainer)
        {
            foreach (var floatingElement in floatContainer.FloatingElements)
            {
                if (floatingElement.View.IsDirty())
                {
                    floatingElement.MeasureAndPosition(root, wasParentDirty: false);
                }
            }
        }
        foreach (var child in root.GetChildren(includeFloatingElements: false))
        {
            MeasureDirtyFloatingElements(child.View);
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
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="args">The event arguments.</param>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        PropertyChanged?.Invoke(this, args);

        // Dependent properties.
        //
        // We don't always need this, e.g. for a settable property like ContentSize we could simply have the setter call
        // OnPropertyChanged twice; but for multiple levels of read-only properties, or properties that depend on
        // multiple other properties, it gets more complicated.
        //
        // Ideally this nonsense could be handled by an IL weaver or source generator, but we're very limited in a
        // shared project.
        //
        // Only need to handle one at a time since the call is recursive, i.e. OnPropertyChanged("InnerSize") will call
        // OnPropertyChanged("BorderSize") which itself will call OnPropertyChanged("OuterSize").
        switch (args.PropertyName)
        {
            case nameof(Margin):
                OnPropertyChanged(nameof(OuterSize));
                OnPropertyChanged(nameof(ActualBounds));
                break;
            case nameof(Padding):
                OnPropertyChanged(nameof(InnerSize));
                break;
            case nameof(ContentSize):
                OnPropertyChanged(nameof(InnerSize));
                break;
            case nameof(InnerSize):
                OnPropertyChanged(nameof(BorderSize));
                break;
            case nameof(BorderSize):
                OnPropertyChanged(nameof(OuterSize));
                OnPropertyChanged(nameof(ActualBounds));
                break;
            case nameof(ActualBounds):
                OnPropertyChanged(nameof(ContentBounds));
                break;
            case nameof(FloatingElements):
                OnPropertyChanged(nameof(FloatingBounds));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that was changed.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

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
        foreach (var child in GetChildrenAt(eventArgs.Position).ZOrderDescending())
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
        return LayoutOffset
            + new Vector2(Margin.Left, Margin.Top)
            + new Vector2(borderThickness.Left, borderThickness.Top)
            + new Vector2(Padding.Left, Padding.Top);
    }

    private IEnumerable<Bounds> GetFloatingBounds()
    {
        var localFloatingOffset = GetFloatingOffset();
        return FloatingElements
            .SelectMany(GetFloatingElementBounds)
            .Select(bounds => bounds.Offset(localFloatingOffset))
            .Concat(GetChildren().SelectMany(child => child.GetFloatingBounds()));
    }

    private static IEnumerable<Bounds> GetFloatingElementBounds(FloatingElement fe)
    {
        var floatingChild = fe.AsViewChild();
        return floatingChild.GetFloatingBounds().Prepend(floatingChild.GetActualBounds());
    }

    private Vector2 GetFloatingOffset()
    {
        return new Vector2(Margin.Left, Margin.Top);
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

        foreach (var child in GetChildrenAt(position).ZOrderDescending())
        {
            if (child.View.PointerEventsEnabled)
            {
                draggingView = child.View;
                return child;
            }
        }
        return null;
    }

    private void SetupRenderTarget(ISpriteBatch batch, [NotNull] ref RenderTarget2D? target)
    {
        var bounds = FloatingBounds.Aggregate(ActualBounds, (acc, bounds) => acc.Union(bounds));
        int width = (int)MathF.Ceiling(bounds.Size.X);
        int height = (int)MathF.Ceiling(bounds.Size.Y);
        batch.InitializeRenderTarget(ref target, width, height);
    }

    private void UpdateClipBounds()
    {
        if (this.clipSize is not LayoutParameters clipSize)
        {
            clipBounds = null;
            return;
        }
        var actualSize = clipSize.Resolve(OuterSize, () => OuterSize);
        var clipPosition = this.clipOrigin is { } clipOrigin
            ? clipOrigin.GetPosition(actualSize, OuterSize)
            : Vector2.Zero;
        clipBounds = new(clipPosition, actualSize);
    }

    private readonly ref struct DrawingState : IDisposable
    {
        private readonly bool wasDrawing;

        public DrawingState()
        {
            wasDrawing = IsDrawing;
            IsDrawing = true;
        }

        public readonly void Dispose()
        {
            IsDrawing = wasDrawing;
        }
    }
}
