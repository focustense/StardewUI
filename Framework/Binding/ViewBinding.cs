using System.ComponentModel;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Represents the binding state of an entire view; provides a single method to perform a once-per-frame update.
/// </summary>
public interface IViewBinding : IDisposable
{
    /// <summary>
    /// The specific attributes bound for the attached view.
    /// </summary>
    /// <remarks>
    /// Per-attribute updates are encapsulated in the <see cref="Update"/> method, so this is normally only needed for
    /// inspecting the state of bindings, e.g. to build a <see cref="BoundViewDefaults"/> instance.
    /// </remarks>
    IReadOnlyList<IAttributeBinding> Attributes { get; }

    /// <summary>
    /// Updates the view, including all bound attributes.
    /// </summary>
    /// <returns><c>true</c> if any updates were performed; <c>false</c> if there was no update due to having no
    /// underlying changes in the bound data or assets.</returns>
    bool Update();
}

/// <summary>
/// A <see cref="ViewBinding"/> that delegates its updates to a list of <see cref="IAttributeBinding"/> instances per
/// bound attribute.
/// </summary>
internal class ViewBinding : IViewBinding
{
    public IReadOnlyList<IAttributeBinding> Attributes => attributeBindings;

    private readonly IReadOnlyList<IAttributeBinding> attributeBindings;
    private readonly IReadOnlyList<IEventBinding> eventBindings;
    private readonly HashSet<string> viewPropertiesChanged = [];
    private readonly WeakReference<IView> viewRef;

    private bool completedFirstUpdate;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of <see cref="ViewBinding"/> with the specified view and attribute bindings.
    /// </summary>
    /// <param name="view">The bound view.</param>
    /// <param name="attributeBindings">The attribute bindings for the <paramref name="view"/>.</param>
    /// <param name="eventBindings">The event bindings for the <paramref name="view"/>.</param>
    public ViewBinding(
        IView view,
        IReadOnlyList<IAttributeBinding> attributeBindings,
        IReadOnlyList<IEventBinding> eventBindings
    )
    {
        viewRef = new(view);
        this.attributeBindings = attributeBindings;
        this.eventBindings = eventBindings;
        if (view is INotifyPropertyChanged viewNpc)
        {
            viewNpc.PropertyChanged += View_PropertyChanged;
        }
    }

    public void Dispose()
    {
        foreach (var binding in eventBindings)
        {
            binding.Dispose();
        }
        foreach (var binding in attributeBindings)
        {
            binding.Dispose();
        }
        if (viewRef.TryGetTarget(out var view) && view is INotifyPropertyChanged viewNpc)
        {
            viewNpc.PropertyChanged -= View_PropertyChanged;
        }
        isDisposed = true;
        GC.SuppressFinalize(this);
    }

    public bool Update()
    {
        using var _ = Trace.Begin(this, nameof(Update));
        if (isDisposed)
        {
            throw new ObjectDisposedException(nameof(ViewBinding));
        }
        if (!viewRef.TryGetTarget(out var view))
        {
            return false;
        }
        bool anyChanged = false;
        foreach (var binding in attributeBindings)
        {
            if (binding.Direction.IsIn())
            {
                anyChanged |= binding.UpdateView(view);
            }
        }
        foreach (var binding in attributeBindings)
        {
            if (
                binding.Direction.IsOut()
                && (!completedFirstUpdate || viewPropertiesChanged.Contains(binding.DestinationPropertyName))
            )
            {
                binding.UpdateSource(view);
            }
        }
        viewPropertiesChanged.Clear();
        completedFirstUpdate = true;
        return anyChanged;
    }

    private void View_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not null)
        {
            viewPropertiesChanged.Add(e.PropertyName);
        }
    }
}
