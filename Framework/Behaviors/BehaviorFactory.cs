namespace StardewUI.Framework.Behaviors;

/// <summary>
/// A behavior factory based on per-name delegates. Can be used as a base class for other factories.
/// </summary>
public class BehaviorFactory : IBehaviorFactory
{
    private readonly List<IBehaviorFactory> delegateFactories = [];

    private readonly Dictionary<string, Func<string, IViewBehavior>> nameFactories = new(
        StringComparer.OrdinalIgnoreCase
    );

    /// <summary>
    /// Adds a new delegate factory which this factory will be allowed to use as a fallback for any behavior names not
    /// handled directly.
    /// </summary>
    /// <param name="factory">The delegate factory.</param>
    public void Add(IBehaviorFactory factory)
    {
        delegateFactories.Add(factory);
    }

    /// <inheritdoc />
    public IViewBehavior CreateBehavior(Type viewType, string name, string argument)
    {
        if (nameFactories.TryGetValue(name, out var factory))
        {
            return factory(argument);
        }
        return delegateFactories
                .Where(factory => factory.SupportsName(name))
                .Select(factory => factory.CreateBehavior(viewType, name, argument))
                .FirstOrDefault() ?? throw new ArgumentException($"Unsupported behavior type: {name}", nameof(name));
    }

    /// <summary>
    /// Registers a behavior for a given name using the behavior's default parameterless constructor.
    /// </summary>
    /// <remarks>
    /// Used for behaviors that do not take arguments, only data.
    /// </remarks>
    /// <typeparam name="TBehavior">The behavior type.</typeparam>
    /// <param name="name">The markup name used to create to the <typeparamref name="TBehavior"/> type.</param>
    public void Register<TBehavior>(string name)
        where TBehavior : IViewBehavior, new()
    {
        nameFactories.Add(name, _ => new TBehavior());
    }

    /// <summary>
    /// Registers a behavior for a given name using a delegate function.
    /// </summary>
    /// <param name="name">The markup name used to create this type of behavior.</param>
    /// <param name="factory">Delegate function that accepts the construction argument (if any) and creates the
    /// corresponding behavior.</param>
    public void Register(string name, Func<string, IViewBehavior> factory)
    {
        nameFactories.Add(name, factory);
    }

    /// <inheritdoc />
    public bool SupportsName(string name)
    {
        return nameFactories.ContainsKey(name) || delegateFactories.Any(factory => factory.SupportsName(name));
    }
}
