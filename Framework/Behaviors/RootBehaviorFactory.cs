namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Behavior factory for built-in behavior types.
/// </summary>
/// <param name="addonFactories">Behavior factories registered by add-ons, in order of priority. All add-on factories
/// are considered after the standard behavior names.</param>
internal class RootBehaviorFactory(IEnumerable<IBehaviorFactory> addonFactories) : IBehaviorFactory
{
    private readonly IBehaviorFactory[] defaultFactories =
    [
        // Order matters here; StateBehaviorFactory needs to handle the "state:" behavior name when the argument has
        // a separator before the standard factory kicks in and creates a ConditionalFlagBehavior.
        new StateBehaviorFactory(),
        CreateStandardBehaviorFactory(),
    ];

    /// <inheritdoc />
    public bool CanCreateBehavior(string name, string argument)
    {
        return true;
    }

    /// <inheritdoc />
    public IViewBehavior CreateBehavior(Type viewType, string name, string argument)
    {
        return defaultFactories
                .Concat(addonFactories)
                .FirstOrDefault(factory => factory.CanCreateBehavior(name, argument))
                ?.CreateBehavior(viewType, name, argument)
            ?? throw new ArgumentException($"Unsupported behavior name: {name}", nameof(name));
    }

    private static IBehaviorFactory CreateStandardBehaviorFactory()
    {
        var factory = new BehaviorFactory();
        factory.Register("state", name => new ConditionalFlagBehavior(name));
        return factory;
    }
}
