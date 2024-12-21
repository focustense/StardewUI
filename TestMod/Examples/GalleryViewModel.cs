using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace StardewUITest.Examples;

internal class GalleryViewModel(IReadOnlyList<GalleryExampleViewModel> examples)
{
    public IReadOnlyList<GalleryExampleViewModel> Examples { get; } = examples;
}

internal partial class GalleryExampleViewModel(
    string title,
    string description,
    string thumbnailItemId,
    Action openAction
)
{
    public string Title { get; } = title;
    public string Description { get; } = description;
    public GalleryThumbnail Thumbnail { get; } = GalleryThumbnail.ForItem(thumbnailItemId);

    public void Open()
    {
        openAction();
    }
}

internal record GalleryThumbnail(Texture2D Texture, Rectangle SourceRect, SliceSettings SliceSettings)
{
    public static GalleryThumbnail ForItem(string itemId)
    {
        var itemData = ItemRegistry.GetDataOrErrorItem(itemId);
        return new(itemData.GetTexture(), itemData.GetSourceRect(), new(4f));
    }
}

internal record SliceSettings(float Scale);
