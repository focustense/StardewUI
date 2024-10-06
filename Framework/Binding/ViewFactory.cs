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
}

/// <summary>
/// View factory for built-in view types.
/// </summary>
public class ViewFactory : IViewFactory
{
    public IView CreateView(string tagName)
    {
        return tagName.ToLowerInvariant() switch
        {
            "banner" => new Banner(),
            "button" => new Button(),
            "checkbox" => new CheckBox(),
            "dropdownlist" => new DropDownList<object>(),
            "expander" => new Expander(),
            "frame" => new Frame(),
            "grid" => new Grid(),
            "image" => new Image(),
            "label" => new Label(),
            "lane" => new Lane(),
            "marquee" => new Marquee(),
            "panel" => new Panel(),
            "scrollableview" => new ScrollableView(),
            "slider" => new Slider(),
            "spacer" => new Spacer(),
            "textinput" => new TextInput(),
            "tinynumberlabel" => new TinyNumberLabel(),
            _ => throw new ArgumentException($"Unsupported view type: {tagName}", nameof(tagName)),
        };
    }
}
