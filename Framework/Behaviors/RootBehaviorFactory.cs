namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Behavior factory for built-in behavior types.
/// </summary>
/// <param name="addonFactories">Behavior factories registered by add-ons, in order of priority. All add-on factories
/// are considered after the standard behavior names.</param>
internal class RootBehaviorFactory(IEnumerable<IBehaviorFactory> addonFactories) : IBehaviorFactory
{
    /// <inheritdoc />
    public IViewBehavior CreateBehavior(string name, string argument)
    {
        return addonFactories.FirstOrDefault(factory => factory.SupportsName(name))?.CreateBehavior(name, argument)
            ?? throw new ArgumentException($"Unsupported behavior name: {name}", nameof(name));
    }

    /// <inheritdoc />
    public bool SupportsName(string name)
    {
        return true;
    }
}
