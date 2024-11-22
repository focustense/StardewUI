using StardewUI.Framework;

namespace StardewUITest;

/// <summary>
/// API for registering additional gallery examples.
/// </summary>
/// <remarks>
/// This is used to include examples from other test/example mods, like the Add-On example.
/// </remarks>
public interface IGalleryApi
{
    /// <summary>
    /// Registers an example to be shown in the example gallery.
    /// </summary>
    /// <param name="title">Short title for the example.</param>
    /// <param name="description">Full description of what the example does or demonstrates.</param>
    /// <param name="thumbnailItemId">ID of an in-game item to show as the example's thumbnail or preview image.</param>
    /// <param name="create">Function to create the example's menu controller.</param>
    void RegisterExample(Func<string> title, Func<string> description, string thumbnailItemId, Func<IMenuController> create);
}
