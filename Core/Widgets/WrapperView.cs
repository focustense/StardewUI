using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace StardewUI;

/// <inheritdoc cref="WrapperView{T}"/>
public abstract class WrapperView : WrapperView<IView> { }

/// <summary>
/// Base class for "app views" with potentially complex hierarchy using a single root view.
/// </summary>
/// <remarks>
/// <para>
/// This implements all the boilerplate of an <see cref="IView"/> without having to actually implement a totally custom
/// <see cref="View"/>; instead, it delegates all functionality to the inner (root) <see cref="IView"/>.
/// </para>
/// <para>
/// The typical use case is for what is often called "Components", "Layouts", "User Controls", etc., in which a class
/// defines both the view hierarchy and an API for interacting with the view and underlying data at the same time. The
/// top-level layout is created in <see cref="CreateView"/>, and child views can be added on creation or at any later
/// time. More importantly, since the subclass decides what children to create, it can also store references to those
/// children for the purposes of updating the UI, responding to events, etc.
/// </para>
/// <para>
/// Wrapper views can be composed like any other views, or used in a <see cref="ViewMenu{T}"/>.
/// </para>
/// </remarks>
/// <typeparam name="T">Type of view being wrapped.</typeparam>
public abstract class WrapperView<T> : IView
    where T : IView
{
    /// <inheritdoc />
    public event EventHandler<ClickEventArgs>? Click;
    /// <inheritdoc />
    public event EventHandler<PointerEventArgs>? Drag;
    /// <inheritdoc />
    public event EventHandler<PointerEventArgs>? DragStart;
    /// <inheritdoc />
    public event EventHandler<PointerEventArgs>? DragEnd;
    /// <inheritdoc />
    public event EventHandler<ClickEventArgs>? LeftClick;
    /// <inheritdoc />
    public event EventHandler<PointerEventArgs>? PointerEnter;
    /// <inheritdoc />
    public event EventHandler<PointerEventArgs>? PointerLeave;
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;
    /// <inheritdoc />
    public event EventHandler<ClickEventArgs>? RightClick;
    /// <inheritdoc />
    public event EventHandler<WheelEventArgs>? Wheel;

    /// <inheritdoc />
    public Bounds ActualBounds => Root.ActualBounds;

    /// <inheritdoc />
    public Bounds ContentBounds => Root.ContentBounds;

    /// <inheritdoc />
    public bool IsFocusable => Root.IsFocusable;

    /// <inheritdoc />
    public LayoutParameters Layout
    {
        get => Root.Layout;
        set => Root.Layout = value;
    }

    /// <inheritdoc />
    public string Name
    {
        get => Root.Name;
        set => Root.Name = value;
    }

    /// <inheritdoc />
    public Vector2 OuterSize => Root.OuterSize;

    /// <inheritdoc />
    public bool PointerEventsEnabled
    {
        get => Root.PointerEventsEnabled;
        set => Root.PointerEventsEnabled = value;
    }

    /// <inheritdoc />
    public Orientation? ScrollWithChildren
    {
        get => Root.ScrollWithChildren;
        set => Root.ScrollWithChildren = value;
    }

    /// <inheritdoc />
    public Tags Tags { get; set; } = new();

    /// <inheritdoc />
    public string Tooltip
    {
        get => Root.Tooltip;
        set => Root.Tooltip = value;
    }

    /// <inheritdoc />
    public Visibility Visibility
    {
        get => Root.Visibility;
        set => Root.Visibility = value;
    }

    /// <inheritdoc />
    public int ZIndex
    {
        get => Root.ZIndex;
        set => Root.ZIndex = value;
    }

    /// <summary>
    /// Whether or not the <see cref="Root"/> view has been created.
    /// </summary>
    /// <remarks>
    /// This is only <c>false</c> within the constructor, but may often need to be checked because of the constructor
    /// sharing logic with property setters or needing to set properties of its own. If the value is <c>false</c> then
    /// <see cref="OptionalRoot"/> will be <c>null</c>.
    /// </remarks>
    protected bool IsViewCreated => root.IsValueCreated;

    /// <summary>
    /// Gets the root view if it has been created, otherwise <c>null</c>.
    /// </summary>
    /// <remarks>
    /// This is similar to <see cref="Root"/> but is guaranteed safe to reference in property getters and from within
    /// <see cref="CreateView"/>.
    /// </remarks>
    protected T? OptionalRoot => root.IsValueCreated ? root.Value : default;

    /// <summary>
    /// Gets the root view.
    /// </summary>
    /// <remarks>
    /// This invokes the lazy loading and calls <see cref="CreateView"/>, and therefore must never be invoked from
    /// within <see cref="CreateView"/> otherwise it will result in a stack overflow.
    /// </remarks>
    protected T Root => root.Value;

    private readonly Lazy<T> root;

    /// <summary>
    /// Initializes a new instance of <see cref="WrapperView{T}"/>.
    /// </summary>
    public WrapperView()
    {
        root = new(() =>
        {
            var view = CreateView();
            view.Click += View_Click;
            view.Drag += View_Drag;
            view.DragEnd += View_DragEnd;
            view.DragStart += View_DragStart;
            view.LeftClick += View_LeftClick;
            view.PointerEnter += View_PointerEnter;
            view.PointerLeave += View_PointerLeave;
            view.PropertyChanged += View_PropertyChanged;
            view.RightClick += View_RightClick;
            view.Wheel += View_Wheel;
            return view;
        });
    }

    // View methods

    /// <inheritdoc />
    public virtual bool ContainsPoint(Vector2 point)
    {
        return Root.ContainsPoint(point);
    }

    /// <inheritdoc />
    public virtual void Draw(ISpriteBatch b)
    {
        Root.Draw(b);
    }

    /// <inheritdoc />
    public virtual FocusSearchResult? FocusSearch(Vector2 position, Direction direction)
    {
        return Root.FocusSearch(position, direction);
    }

    /// <inheritdoc />
    public virtual ViewChild? GetChildAt(Vector2 position)
    {
        return Root.GetChildAt(position);
    }

    /// <inheritdoc />
    public virtual Vector2? GetChildPosition(IView childView)
    {
        return Root.GetChildPosition(childView);
    }

    /// <inheritdoc />
    public virtual IEnumerable<ViewChild> GetChildren()
    {
        return Root.GetChildren();
    }

    /// <inheritdoc />
    public virtual IEnumerable<ViewChild> GetChildrenAt(Vector2 position)
    {
        return Root.GetChildrenAt(position);
    }

    /// <inheritdoc />
    public virtual ViewChild? GetDefaultFocusChild()
    {
        return Root.GetDefaultFocusChild();
    }

    /// <inheritdoc />
    public virtual bool HasOutOfBoundsContent()
    {
        return Root.HasOutOfBoundsContent();
    }

    /// <inheritdoc />
    public virtual bool IsDirty()
    {
        return Root.IsDirty();
    }

    /// <inheritdoc />
    public virtual bool Measure(Vector2 availableSize)
    {
        var wasDirty = Root.Measure(availableSize);
        if (wasDirty)
        {
            OnLayout();
        }
        return wasDirty;
    }

    /// <inheritdoc />
    public virtual void OnClick(ClickEventArgs e)
    {
        Root.OnClick(e);
    }

    /// <inheritdoc />
    public virtual void OnDrag(PointerEventArgs e)
    {
        Root.OnDrag(e);
    }

    /// <inheritdoc />
    public virtual void OnDrop(PointerEventArgs e)
    {
        Root.OnDrop(e);
    }

    /// <inheritdoc />
    public virtual void OnPointerMove(PointerMoveEventArgs e)
    {
        Root.OnPointerMove(e);
    }

    /// <inheritdoc />
    /// <remarks>
    /// When overriding <see cref="View.OnUpdate"/>, be sure to call <c>base.OnUpdate()</c> to ensure that any
    /// view children also receive their updates.
    /// </remarks>
    public virtual void OnUpdate(TimeSpan elapsed)
    {
        Root.OnUpdate(elapsed);
    }

    /// <inheritdoc />
    public virtual void OnWheel(WheelEventArgs e)
    {
        Root.OnWheel(e);
    }

    /// <inheritdoc />
    public virtual bool ScrollIntoView(IEnumerable<ViewChild> path, out Vector2 distance)
    {
        return Root.ScrollIntoView(path, out distance);
    }

    /// <summary>
    /// Creates and returns the root view.
    /// </summary>
    protected abstract T CreateView();

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="args">The event arguments.</param>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        PropertyChanged?.Invoke(this, args);
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that was changed.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }

    /// <summary>
    /// Runs whenever layout occurs as a result of the UI elements changing.
    /// </summary>
    protected virtual void OnLayout() { }

    /// <summary>
    /// Ensures that the root view is created before attempting to access a child view.
    /// </summary>
    /// <remarks>
    /// This is syntactic sugar over accessing <see cref="Root"/> first to force lazy loading.
    /// </remarks>
    /// <typeparam name="TChild">Type of child view to access.</typeparam>
    /// <param name="viewSelector">Function to retrieve the inner view.</param>
    /// <returns>The inner view.</returns>
    protected TChild RequireView<TChild>(Func<TChild> viewSelector)
        where TChild : IView
    {
        _ = Root;
        return viewSelector();
    }

    private void View_Click(object? sender, ClickEventArgs e)
    {
        Click?.Invoke(this, e);
    }

    private void View_Drag(object? sender, PointerEventArgs e)
    {
        Drag?.Invoke(this, e);
    }

    private void View_DragEnd(object? sender, PointerEventArgs e)
    {
        DragEnd?.Invoke(this, e);
    }

    private void View_DragStart(object? sender, PointerEventArgs e)
    {
        DragStart?.Invoke(this, e);
    }

    private void View_LeftClick(object? sender, ClickEventArgs e)
    {
        LeftClick?.Invoke(this, e);
    }

    private void View_PointerEnter(object? sender, PointerEventArgs e)
    {
        PointerEnter?.Invoke(this, e);
    }

    private void View_PointerLeave(object? sender, PointerEventArgs e)
    {
        PointerLeave?.Invoke(this, e);
    }

    private void View_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }

    private void View_RightClick(object? sender, ClickEventArgs e)
    {
        RightClick?.Invoke(this, e);
    }

    private void View_Wheel(object? sender, WheelEventArgs e)
    {
        Wheel?.Invoke(this, e);
    }
}
