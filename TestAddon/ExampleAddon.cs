using StardewUI.Framework.Addons;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Converters;

namespace StardewUITestAddon;

internal class ExampleAddon(string id) : IAddon
{
    public string Id { get; } = id;

    public IValueConverterFactory ValueConverterFactory => valueConverterFactory.Value;

    public IViewFactory ViewFactory => viewFactory.Value;

    private readonly Lazy<IValueConverterFactory> valueConverterFactory =
        new(() =>
        {
            var factory = new ValueConverterFactory();
            factory.TryRegister(new ItemIdToSpriteConverter());
            factory.TryRegister(new StringToKeySplineConverter());
            return factory;
        });

    private readonly Lazy<IViewFactory> viewFactory =
        new(() =>
        {
            var factory = new ViewFactory();
            factory.Register<Carousel>("carousel");
            return factory;
        });
}
