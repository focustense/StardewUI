using System.ComponentModel;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Api;

/// <summary>
/// A view based on a <see cref="Document"/>.
/// </summary>
/// <remarks>
/// This view type mainly exists as glue for the API, to be used in a <see cref="ViewMenu{T}"/>.
/// </remarks>
/// <param name="viewNodeFactory">Factory for creating and binding <see cref="IViewNode"/>s.</param>
/// <param name="documentSource">Source providing the StarML document describing the view.</param>
internal class DocumentView(IViewNodeFactory viewNodeFactory, IValueSource<Document> documentSource)
    : IView,
        IDisposable
{
    public Bounds ActualBounds => rootView?.ActualBounds ?? Bounds.Empty;

    public Bounds ContentBounds => rootView?.ContentBounds ?? Bounds.Empty;

    /// <summary>
    /// The data context (model) to provide to the root node.
    /// </summary>
    public BindingContext? Context
    {
        get => context;
        set
        {
            if (value != context)
            {
                context = value;
                if (rootNode is not null)
                {
                    rootNode.Context = value;
                }
            }
        }
    }

    public bool IsFocusable => rootView?.IsFocusable ?? false;

    public LayoutParameters Layout
    {
        get => layout;
        set => SetOnRootView(ref layout, value, view => view.Layout = value);
    }

    public string Name
    {
        get => name;
        set => SetOnRootView(ref name, value, view => view.Name = value);
    }

    public Vector2 OuterSize => rootView?.OuterSize ?? Vector2.Zero;

    public bool PointerEventsEnabled
    {
        get => pointerEventsEnabled;
        set => SetOnRootView(ref pointerEventsEnabled, value, view => view.PointerEventsEnabled = value);
    }
    public Orientation? ScrollWithChildren
    {
        get => scrollWithChildren;
        set => SetOnRootView(ref scrollWithChildren, value, view => view.ScrollWithChildren = value);
    }

    public Tags Tags => rootView?.Tags ?? Tags.Empty;

    public string Tooltip
    {
        get => tooltip;
        set => SetOnRootView(ref tooltip, value, view => view.Tooltip = value);
    }
    public Visibility Visibility
    {
        get => visibility;
        set => SetOnRootView(ref visibility, value, view => view.Visibility = value);
    }
    public int ZIndex
    {
        get => zIndex;
        set => SetOnRootView(ref zIndex, value, view => view.ZIndex = value);
    }

    public event EventHandler<ClickEventArgs>? Click;
    public event EventHandler<PointerEventArgs>? Drag;
    public event EventHandler<PointerEventArgs>? DragEnd;
    public event EventHandler<PointerEventArgs>? DragStart;
    public event EventHandler<ClickEventArgs>? LeftClick;
    public event EventHandler<PointerEventArgs>? PointerEnter;
    public event EventHandler<PointerEventArgs>? PointerLeave;
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<ClickEventArgs>? RightClick;
    public event EventHandler<WheelEventArgs>? Wheel;

    private BindingContext? context;
    private LayoutParameters layout = new();
    private string name = "";
    private bool pointerEventsEnabled;
    private IViewNode? rootNode;
    private IView? rootView;
    private Orientation? scrollWithChildren;
    private string tooltip = "";
    private Visibility visibility;
    private int zIndex;

    public bool ContainsPoint(Vector2 point)
    {
        return rootView?.ContainsPoint(point) ?? false;
    }

    public void Dispose()
    {
        DetachHandlers();
        rootView = null;
        rootNode?.Dispose();
        rootNode = null;
        if (documentSource is IDisposable sourceDisposable)
        {
            sourceDisposable.Dispose();
        }
        GC.SuppressFinalize(this);
    }

    public void Draw(ISpriteBatch b)
    {
        rootView?.Draw(b);
    }

    public FocusSearchResult? FocusSearch(Vector2 position, Direction direction)
    {
        return rootView?.FocusSearch(position, direction);
    }

    public ViewChild? GetChildAt(Vector2 position)
    {
        return rootView?.GetChildAt(position);
    }

    public Vector2? GetChildPosition(IView childView)
    {
        return rootView?.GetChildPosition(childView);
    }

    public IEnumerable<ViewChild> GetChildren()
    {
        return rootView?.GetChildren() ?? [];
    }

    public IEnumerable<ViewChild> GetChildrenAt(Vector2 position)
    {
        return rootView?.GetChildrenAt(position) ?? [];
    }

    public ViewChild? GetDefaultFocusChild()
    {
        return rootView?.GetDefaultFocusChild();
    }

    public bool HasOutOfBoundsContent()
    {
        return rootView?.HasOutOfBoundsContent() ?? false;
    }

    public bool IsDirty()
    {
        return rootView?.IsDirty() ?? false;
    }

    public bool Measure(Vector2 availableSize)
    {
        return rootView?.Measure(availableSize) ?? false;
    }

    public void OnClick(ClickEventArgs e)
    {
        rootView?.OnClick(e);
    }

    public void OnDrag(PointerEventArgs e)
    {
        rootView?.OnDrag(e);
    }

    public void OnDrop(PointerEventArgs e)
    {
        rootView?.OnDrop(e);
    }

    public void OnPointerMove(PointerMoveEventArgs e)
    {
        rootView?.OnPointerMove(e);
    }

    public void OnUpdate(TimeSpan elapsed)
    {
        if (documentSource.Update())
        {
            CreateViewNode();
        }
        if (rootNode is null)
        {
            return;
        }
        if (rootNode.Update(elapsed))
        {
            var nextRootView = rootNode.Views.FirstOrDefault();
            if (nextRootView != rootView)
            {
                DetachHandlers();
                rootView = nextRootView;
                if (rootView is not null)
                {
                    rootView.Layout = Layout;
                    rootView.Name = Name;
                    rootView.PointerEventsEnabled = PointerEventsEnabled;
                    rootView.ScrollWithChildren = ScrollWithChildren;
                    rootView.Tooltip = Tooltip;
                    rootView.Visibility = Visibility;
                    rootView.ZIndex = ZIndex;
                }
                AttachHandlers();
            }
        }
        rootView?.OnUpdate(elapsed);
    }

    public void OnWheel(WheelEventArgs e)
    {
        rootView?.OnWheel(e);
    }

    public bool ScrollIntoView(IEnumerable<ViewChild> path, out Vector2 distance)
    {
        distance = default;
        return rootView?.ScrollIntoView(path, out distance) ?? false;
    }

    private void AttachHandlers()
    {
        if (rootView is null)
        {
            return;
        }
        rootView.Click += RootView_Click;
        rootView.Drag += RootView_Drag;
        rootView.DragEnd += RootView_DragEnd;
        rootView.DragStart += RootView_DragStart;
        rootView.LeftClick += RootView_LeftClick;
        rootView.PointerEnter += RootView_PointerEnter;
        rootView.PointerLeave += RootView_PointerLeave;
        rootView.PropertyChanged += RootView_PropertyChanged;
        rootView.RightClick += RootView_RightClick;
        rootView.Wheel += RootView_Wheel;
    }

    private void CreateViewNode()
    {
        rootNode?.Dispose();
        if (documentSource.Value is null)
        {
            return;
        }
        rootNode = viewNodeFactory.CreateNode(documentSource.Value.Root);
        rootNode.Context = Context;
    }

    private void DetachHandlers()
    {
        if (rootView is null)
        {
            return;
        }
        rootView.Click -= RootView_Click;
        rootView.Drag -= RootView_Drag;
        rootView.DragEnd -= RootView_DragEnd;
        rootView.DragStart -= RootView_DragStart;
        rootView.LeftClick -= RootView_LeftClick;
        rootView.PointerEnter -= RootView_PointerEnter;
        rootView.PointerLeave -= RootView_PointerLeave;
        rootView.PropertyChanged -= RootView_PropertyChanged;
        rootView.RightClick -= RootView_RightClick;
        rootView.Wheel -= RootView_Wheel;
    }

    private void RootView_Click(object? _, ClickEventArgs e) => Click?.Invoke(this, e);

    private void RootView_Drag(object? _, PointerEventArgs e) => Drag?.Invoke(this, e);

    private void RootView_DragEnd(object? _, PointerEventArgs e) => DragEnd?.Invoke(this, e);

    private void RootView_DragStart(object? _, PointerEventArgs e) => DragStart?.Invoke(this, e);

    private void RootView_LeftClick(object? _, ClickEventArgs e) => LeftClick?.Invoke(this, e);

    private void RootView_PointerEnter(object? _, PointerEventArgs e) => PointerEnter?.Invoke(this, e);

    private void RootView_PointerLeave(object? _, PointerEventArgs e) => PointerLeave?.Invoke(this, e);

    private void RootView_PropertyChanged(object? _, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

    private void RootView_RightClick(object? _, ClickEventArgs e) => RightClick?.Invoke(this, e);

    private void RootView_Wheel(object? _, WheelEventArgs e) => Wheel?.Invoke(this, e);

    // N.B. In theory Action<IView> for setValue should take a T param as well, but in practice this is only ever called
    // from property setters that already have the property value, so the extra param is just clutter.
    private void SetOnRootView<T>(ref T thisValue, T newValue, Action<IView> setValue)
    {
        if (EqualityComparer<T>.Default.Equals(thisValue, newValue))
        {
            return;
        }
        thisValue = newValue;
        if (rootView is not null)
        {
            setValue(rootView);
        }
    }
}
