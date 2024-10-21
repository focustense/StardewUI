namespace StardewUI.Widgets;

/// <inheritdoc cref="ComponentView{T}"/>
public abstract class ComponentView : ComponentView<IView> { }

/// <summary>
/// Base class for custom widgets and "app views" with potentially complex hierarchy using a single root view.
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
/// Component views can be composed like any other views, or used in a <see cref="ViewMenu{T}"/>.
/// </para>
/// </remarks>
/// <typeparam name="T">Type of view used for the root.</typeparam>
public abstract class ComponentView<T> : DecoratorView<T>
    where T : class, IView
{
    /// <inheritdoc />
    protected new T View
    {
        get => base.View!;
        private set => base.View = value;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ComponentView{T}"/>.
    /// </summary>
    public ComponentView()
    {
        View = CreateView();
    }

    /// <summary>
    /// Creates and returns the root view.
    /// </summary>
    protected abstract T CreateView();
}
