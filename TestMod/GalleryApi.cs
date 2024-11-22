using StardewUI.Framework;
using StardewUITest.Examples;

namespace StardewUITest;

/// <summary>
/// Implementation of the gallery API. Must be public for Pintail.
/// </summary>
public class GalleryApi : IGalleryApi
{
    internal List<Func<GalleryExampleViewModel>> Registrations { get; } = [];

    /// <inheritdoc />
    public void RegisterExample(Func<string> title, Func<string> description, string thumbnailItemId, Func<IMenuController> create)
    {
        Registrations.Add(() => new(title(), description(), thumbnailItemId, () => create().Launch()));
    }
}
