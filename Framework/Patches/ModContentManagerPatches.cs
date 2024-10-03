using Microsoft.Xna.Framework.Content;
using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Patches;

/// <summary>
/// Patches for SMAPI's <c>ModContentManager</c> (internal).
/// </summary>
internal static class ModContentManagerPatches
{
    public static bool HandleUnknownFileType_Prefix(
        IAssetName assetName,
        FileInfo file,
        Type assetType,
        ref object __result
    )
    {
        var ext = file.Extension.ToLower();
        if (ext == ".sml" || ext == ".starml")
        {
            if (assetType != typeof(Document) && assetType != typeof(object))
            {
                throw new ContentLoadException(
                    $"Failed loading asset '{assetName}' from {file.Name}: the target asset type is incorrect "
                        + $"(expected {typeof(Document).FullName}, but got {assetType.FullName})."
                );
            }
            __result = LoadDocument(file);
            return false;
        }
        return true;
    }

    private static Document LoadDocument(FileInfo file)
    {
        string markup = File.ReadAllText(file.FullName);
        return Document.Parse(markup);
    }
}
