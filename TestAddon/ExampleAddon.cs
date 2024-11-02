using StardewUI.Framework.Addons;
using StardewUI.Framework.Converters;

namespace StardewUITestAddon;

internal class ExampleAddon(string id) : IAddon
{
    public string Id { get; } = id;

    public IValueConverterFactory ValueConverterFactory => valueConverterFactory.Value;

    private readonly Lazy<IValueConverterFactory> valueConverterFactory = new(() =>
    {
        var factory = new RootValueConverterFactory();
        factory.TryRegister(new ItemIdToSpriteConverter());
        return factory;
    });
}