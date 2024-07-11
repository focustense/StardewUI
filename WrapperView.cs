using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

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
/// Wrapper views can be composed like any other views, or used in a <see cref="ViewMenu"/>.
/// </para>
/// </remarks>
public abstract class WrapperView : IView
{
    public event EventHandler<ClickEventArgs>? Click;

    public Bounds ActualBounds => Root.ActualBounds;
    public bool IsFocusable => Root.IsFocusable;
    public LayoutParameters Layout { get => Root.Layout; set => Root.Layout = value; }
    public string Name { get => Root.Name; set => Root.Name = value; }
    public Vector2 OuterSize => Root.OuterSize;
    public Tags Tags { get; set; } = new();
    public string Tooltip { get => Root.Tooltip; set => Root.Tooltip = value; }
    public int ZIndex { get => Root.ZIndex; set => Root.ZIndex = value; }

    protected bool IsViewCreated => root.IsValueCreated;
    protected IView Root => root.Value;

    private readonly Lazy<IView> root;

    public WrapperView()
    {
        root = new(() =>
        {
            var view = CreateView();
            view.Click += View_Click;
            return view;
        });
    }

    public virtual void Draw(ISpriteBatch b)
    {
        Root.Draw(b);
    }

    public virtual ViewChild? FocusSearch(Vector2 position, Direction direction)
    {
        return Root.FocusSearch(position, direction);
    }

    public virtual ViewChild? GetChildAt(Vector2 position)
    {
        return Root.GetChildAt(position);
    }

    public virtual IEnumerable<ViewChild> GetChildren()
    {
        return Root.GetChildren();
    }

    public virtual bool IsDirty()
    {
        return Root.IsDirty();
    }

    public virtual bool Measure(Vector2 availableSize)
    {
        return Root.Measure(availableSize);
    }

    public void OnClick(ClickEventArgs e)
    {
        Root.OnClick(e);
    }

    /// <summary>
    /// Creates and returns the root view.
    /// </summary>
    protected abstract IView CreateView();

    private void View_Click(object? sender, ClickEventArgs e)
    {
        Click?.Invoke(this, e);
    }
}