namespace StardewUI.Framework.Api;

/// <summary>
/// Associates a SMAPI/XNA asset prefix with a directory on disk.
/// </summary>
/// <remarks>
/// Used internally by classes such as <see cref="AssetRegistry"/> and <see cref="AssetPreloader"/>.
/// </remarks>
/// <param name="AssetPrefix">The prefix for assets loaded from this directory, e.g. <c>Mods/ModName/Views</c>.</param>
/// <param name="ModDirectory">The directory on disk where the assets are located.</param>
internal record DirectoryMapping(string AssetPrefix, string ModDirectory);
