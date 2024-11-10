namespace StardewUI.Framework.Binding;

/// <summary>
/// Factory for creating views from tags.
/// </summary>
/// <remarks>
/// This is a simple, low-level abstraction that simply maps tags to view types. It does not perform any reflection or
/// participate in view binding.
/// </remarks>
public interface IViewFactory
{
    /// <summary>
    /// Creates a new view.
    /// </summary>
    /// <param name="tagName">The markup tag that specifies the type of view.</param>
    /// <returns>A new view of a type corresponding to the <paramref name="tagName"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="tagName"/> does not correspond to any
    /// supported view type.</exception>
    IView CreateView(string tagName);

    /// <summary>
    /// Checks if the factory can create views corresponding to a specific tag.
    /// </summary>
    /// <param name="tagName">The markup tag that specifies the type of view.</param>
    /// <returns><c>true</c> if this factory should handle the specified <paramref name="tagName"/>, otherwise
    /// <c>false</c>.</returns>
    bool SupportsTag(string tagName);
}

/// <summary>
/// A view factory based on per-tag delegates. Can be used as a base class for other view factories.
/// </summary>
public class ViewFactory : IViewFactory
{
    private readonly Dictionary<string, Func<IView>> tagFactories = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public virtual IView CreateView(string tagName)
    {
        return tagFactories.TryGetValue(tagName, out var factory)
            ? factory()
            : throw new ArgumentException($"Unsupported view type: {tagName}", nameof(tagName));
    }

    /// <summary>
    /// Registers a view for a given tag using the view's default parameterless constructor.
    /// </summary>
    /// <typeparam name="TView">The view type.</typeparam>
    /// <param name="tagName">The markup tag corresponding to the <typeparamref name="TView"/> type.</param>
    public void Register<TView>(string tagName)
        where TView : IView, new()
    {
        tagFactories.Add(tagName, () => new TView());
    }

    /// <summary>
    /// Registers a view for a given tag using a delegate function.
    /// </summary>
    /// <param name="tagName">The markup tag to handle.</param>
    /// <param name="tagFactory">Delegate function to create the view corresponding to the
    /// <paramref name="tagName"/>.</param>
    public void Register(string tagName, Func<IView> tagFactory)
    {
        tagFactories.Add(tagName, tagFactory);
    }

    /// <inheritdoc />
    public virtual bool SupportsTag(string tagName)
    {
        return tagFactories.ContainsKey(tagName);
    }
}
