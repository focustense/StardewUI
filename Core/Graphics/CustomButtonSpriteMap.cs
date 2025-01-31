using StardewUI.Data;

namespace StardewUI.Graphics;

/// <summary>
/// Controller/keyboard/mouse sprite map using custom/configured asset data.
/// </summary>
/// <param name="content">Helper for retrieving main game assets.</param>
/// <param name="data">Configuration data for this map.</param>
public class CustomButtonSpriteMap(IGameContentHelper content, ButtonSpriteMapData data) : ButtonSpriteMap
{
    private static readonly Lazy<Sprite> DefaultBackground = new(() => new(Game1.staminaRect));

    private readonly Dictionary<SButton, Sprite?> cache = [];
    private readonly Lazy<Sprite> controllerBlank = new(
        () =>
            !string.IsNullOrEmpty(data.ControllerBlank)
                ? content.Load<Sprite>(data.ControllerBlank)
                : DefaultBackground.Value
    );
    private readonly Lazy<Sprite> errorSprite = new(() =>
    {
        var errorData = ItemRegistry.GetDataOrErrorItem("!!!invalid!!!");
        return new(errorData.GetTexture(), errorData.GetSourceRect());
    });
    private readonly Lazy<Sprite> keyboardBlank = new(
        () =>
            !string.IsNullOrEmpty(data.KeyboardBlank)
                ? content.Load<Sprite>(data.KeyboardBlank)
                : DefaultBackground.Value
    );

    /// <inheritdoc />
    protected override Sprite ControllerBlank => controllerBlank.Value;

    /// <inheritdoc />
    protected override Sprite KeyboardBlank => keyboardBlank.Value;

    /// <inheritdoc />
    protected override Sprite MouseLeft => Get(SButton.MouseLeft) ?? errorSprite.Value;

    /// <inheritdoc />
    protected override Sprite MouseMiddle => Get(SButton.MouseMiddle) ?? errorSprite.Value;

    /// <inheritdoc />
    protected override Sprite MouseRight => Get(SButton.MouseRight) ?? errorSprite.Value;

    /// <inheritdoc />
    protected override Sprite? Get(SButton button)
    {
        if (!cache.TryGetValue(button, out var sprite))
        {
            sprite = data.Buttons.TryGetValue(button, out var assetName) ? content.Load<Sprite>(assetName) : null;
            cache.Add(button, sprite);
        }
        return sprite;
    }
}
