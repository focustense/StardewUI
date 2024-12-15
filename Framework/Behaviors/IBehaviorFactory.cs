namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Factory for creating <see cref="IViewBehavior"/> instances from markup data.
/// </summary>
public interface IBehaviorFactory
{
    /// <summary>
    /// Creates a new behavior.
    /// </summary>
    /// <param name="name">The behavior name that specifies the type of behavior.</param>
    /// <param name="argument">Additional argument provided in the markup, distinct from the behavior's
    /// <see cref="IViewBehavior.DataType"/>. Enables prefixed behaviors such as <c>tween:opacity</c></param>.
    /// <returns>A new behavior of a type corresponding to the <paramref name="name"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> does not correspond to any
    /// supported behavior type.</exception>
    IViewBehavior CreateBehavior(string name, string argument);

    /// <summary>
    /// Checks if the factory can create behaviors with a specified name.
    /// </summary>
    /// <param name="name">The behavior name.</param>
    /// <returns><c>true</c> if this factory should handle the specified <paramref name="name"/>, otherwise
    /// <c>false</c>.</returns>
    bool SupportsName(string name);
}
