namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Arguments for events relating to a flag included in an <see cref="IViewState"/>.
/// </summary>
/// <param name="name">The name of the affected flag.</param>
public class FlagEventArgs(string name) : EventArgs
{
    /// <summary>
    /// The name of the affected flag.
    /// </summary>
    public string Name { get; } = name;
}
