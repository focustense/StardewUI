using Microsoft.Xna.Framework.Graphics;
using StardewUI.Graphics;

namespace StardewUI.Framework.Content;

/// <summary>
/// Provides access to the custom sprites owned by StardewUI and not present in the base game.
/// </summary>
/// <remarks>
/// This is the counterpart to <see cref="UiSpriteProvider"/> which is for the base game (statically-cached) assets.
/// </remarks>
/// <param name="helper">Helper for the mod that owns the assets, i.e. for StardewUI.</param>
internal class CommonSprites(IModHelper helper)
{
    /// <summary>
    /// 240x240 px color circle, used for selecting a hue.
    /// </summary>
    public Sprite ColorCircle240 => new(colorCircle240Texture.Asset);

    /// <summary>
    /// 480x480 px color circle, used for selecting a hue.
    /// </summary>
    public Sprite ColorCircle480 => new(colorCircle480Texture.Asset);

    private readonly LazyAsset<Texture2D> colorCircle240Texture = new(
        helper,
        $"Mods/{helper.ModContent.ModID}/Textures/ColorCircle240",
        "assets/ColorCircle240.png"
    );

    private readonly LazyAsset<Texture2D> colorCircle480Texture = new(
        helper,
        $"Mods/{helper.ModContent.ModID}/Textures/ColorCircle480",
        "assets/ColorCircle480.png"
    );
}
