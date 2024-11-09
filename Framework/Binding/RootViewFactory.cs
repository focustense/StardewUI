using StardewUI.Framework.Views;
using StardewUI.Widgets;
using StardewUI.Widgets.Keybinding;

namespace StardewUI.Framework.Binding;

/// <summary>
/// View factory for built-in view types.
/// </summary>
/// <param name="addonFactories">View factories registered by add-ons, in order of priority. All add-on factories are
/// considered after the standard tags.</param>
internal class RootViewFactory(IEnumerable<IViewFactory> addonFactories) : IViewFactory
{
    /// <inheritdoc />
    public IView CreateView(string tagName)
    {
        return tagName.ToLowerInvariant() switch
        {
            "banner" => new Banner(),
            "button" => new Button(),
            "checkbox" => new CheckBox(),
            "digits" => new TinyNumberLabel(),
            "dropdown" => new DynamicDropDownList(),
            "expander" => new Expander(),
            "frame" => new Frame(),
            "grid" => new Grid(),
            "image" => new Image(),
            "keybind" => new KeybindView(),
            "keybind-editor" => new KeybindListEditor(),
            "label" => new Label(),
            "lane" => new Lane(),
            "marquee" => new Marquee(),
            "nine-grid-editor" => new NineGridPlacementEditor(),
            "panel" => new Panel(),
            "scrollable" => new ScrollableView(),
            "slider" => new Slider(),
            "spacer" => new Spacer(),
            "tab" => new Tab(),
            "textinput" => new TextInput(),
            _ => addonFactories.FirstOrDefault(factory => factory.SupportsTag(tagName))?.CreateView(tagName)
                ?? throw new ArgumentException($"Unsupported view type: {tagName}", nameof(tagName)),
        };
    }

    /// <inheritdoc />
    /// <remarks>
    /// Unlike add-on <see cref="IViewFactory"/> implementations, the root factory always returns <c>true</c> regardless
    /// of whether the tag is really implemented, because it must handle requests for all views.
    /// </remarks>
    public bool SupportsTag(string tagName)
    {
        return true;
    }
}
