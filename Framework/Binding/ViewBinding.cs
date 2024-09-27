namespace StardewUI.Framework.Binding;

/// <summary>
/// Represents the binding state of an entire view; provides a single method to perform a once-per-frame update.
/// </summary>
public interface IViewBinding : IDisposable
{
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
/// <param name="view">The bound view.</param>
/// <param name="attributeBindings">The attribute bindings for the <paramref name="view"/>.</param>
internal class ViewBinding(IView view, IReadOnlyList<IAttributeBinding> attributeBindings) : IViewBinding
{
    private readonly WeakReference<IView> viewRef = new(view);

    private bool isDisposed;

    public void Dispose()
    {
        foreach (var binding in attributeBindings)
        {
            binding.Dispose();
        }
        isDisposed = true;
        GC.SuppressFinalize(this);
    }

    public bool Update()
    {
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
            anyChanged |= binding.Update(view);
        }
        return anyChanged;
    }
}
