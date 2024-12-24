namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Behavior factory for built-in behavior types.
/// </summary>
/// <param name="addonFactories">Behavior factories registered by add-ons, in order of priority. All add-on factories
/// are considered after the standard behavior names.</param>
internal class RootBehaviorFactory(IEnumerable<IBehaviorFactory> addonFactories) : IBehaviorFactory
{
    private readonly IBehaviorFactory[] defaultFactories = [new StateBehaviorFactory()];

    /// <inheritdoc />
    public IViewBehavior CreateBehavior(Type viewType, string name, string argument)
    {
        return defaultFactories
                .Concat(addonFactories)
                .FirstOrDefault(factory => factory.SupportsName(name))
                ?.CreateBehavior(viewType, name, argument)
            ?? throw new ArgumentException($"Unsupported behavior name: {name}", nameof(name));
    }

    /// <inheritdoc />
    public bool SupportsName(string name)
    {
        return true;
    }
}
