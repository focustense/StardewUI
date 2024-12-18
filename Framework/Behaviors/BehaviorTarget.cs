namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Encapsulates the target of an <see cref="IViewBehavior"/>.
/// </summary>
/// <param name="View">The view that will receive the behavior.</param>
/// <param name="ViewState">State overrides for the <paramref name="View"/>.</param>
public record BehaviorTarget(IView View, IViewState ViewState);
