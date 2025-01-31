namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Factory for creating <see cref="IViewBehavior"/> instances from markup data.
/// </summary>
public interface IBehaviorFactory
{
    /// <summary>
    /// Checks if the factory can create behaviors with a specified name and argument.
    /// </summary>
    /// <param name="name">The behavior name.</param>
    /// <param name="argument">The argument for the behavior, if any. Most implementations can ignore this parameter,
    /// but in some cases it is used for disambiguation.</param>
    /// <returns><c>true</c> if this factory should handle the specified <paramref name="name"/>, when given the
    /// specified <paramref name="argument"/>, otherwise <c>false</c>.</returns>
    bool CanCreateBehavior(string name, string argument);

    /// <summary>
    /// Creates a new behavior.
    /// </summary>
    /// <param name="viewType">The specific type of <see cref="IView"/> that will receive the behavior.</param>
    /// <param name="name">The behavior name that specifies the type of behavior.</param>
    /// <param name="argument">Additional argument provided in the markup, distinct from the behavior's
    /// <see cref="IViewBehavior.DataType"/>. Enables prefixed behaviors such as <c>tween:opacity</c></param>.
    /// <returns>A new behavior of a type corresponding to the <paramref name="name"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> does not correspond to any
    /// supported behavior type.</exception>
    IViewBehavior CreateBehavior(Type viewType, string name, string argument);
}
