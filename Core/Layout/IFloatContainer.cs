namespace StardewUI.Layout;

/// <summary>
/// Provides access to a UI element's floating elements.
/// </summary>
/// <remarks>
/// Every subclass of <see cref="View"/> has built-in behavior to hold, track, layout and display floating elements, but
/// some custom <see cref="IView"/> implementations (including <see cref="Widgets.DecoratorView"/> subclasses) may not,
/// and must implement the interface themselves if they wish to support floats.
/// </remarks>
public interface IFloatContainer
{
    /// <summary>
    /// The floating elements to display relative to this view.
    /// </summary>
    IList<FloatingElement> FloatingElements { get; set; }
}
