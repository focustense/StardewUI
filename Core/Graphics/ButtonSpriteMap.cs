using StardewModdingAPI;

namespace StardewUI.Graphics;

/// <summary>
/// Base class for a <see cref="ISpriteMap{T}"/> for controller/keyboard bindings.
/// </summary>
public abstract class ButtonSpriteMap : ISpriteMap<SButton>
{
    /// <summary>
    /// A blank controller button upon which the specific button label can be drawn.
    /// </summary>
    /// <remarks>
    /// If the sprite specifies non-zero <see cref="Sprite.FixedEdges"/> then they will be added to the label's margin.
    /// </remarks>
    protected abstract Sprite ControllerBlank { get; }

    /// <summary>
    /// A blank keyboard key upon which the specific key name can be drawn.
    /// </summary>
    /// /// <remarks>
    /// If the sprite specifies non-zero <see cref="Sprite.FixedEdges"/> then they will be added to the label's margin.
    /// </remarks>
    protected abstract Sprite KeyboardBlank { get; }

    /// <summary>
    /// The mouse with left button pressed.
    /// </summary>
    protected abstract Sprite MouseLeft { get; }

    /// <summary>
    /// The mouse with middle button pressed.
    /// </summary>
    protected abstract Sprite MouseMiddle { get; }

    /// <summary>
    /// The mouse with right button pressed.
    /// </summary>
    protected abstract Sprite MouseRight { get; }

    /// <inheritdoc />
    public Sprite Get(SButton key, out bool isPlaceholder)
    {
        var mouseSprite = key switch
        {
            SButton.MouseLeft => MouseLeft,
            SButton.MouseMiddle => MouseMiddle,
            SButton.MouseRight => MouseRight,
            _ => null,
        };
        if (mouseSprite is not null)
        {
            isPlaceholder = false;
            return mouseSprite;
        }
        var exactSprite = Get(key);
        if (exactSprite is not null)
        {
            isPlaceholder = false;
            return exactSprite;
        }
        isPlaceholder = true;
        return key.TryGetController(out _) ? ControllerBlank : KeyboardBlank;
    }

    /// <summary>
    /// Gets the specific sprite for a particular button.
    /// </summary>
    /// <param name="button">The button for which to retrieve a sprite.</param>
    /// <returns>The precise <see cref="Sprite"/> representing the given <paramref name="button"/>, or <c>null</c> if
    /// the button does not have a special sprite and could/should use a generic background + text.</returns>
    protected abstract Sprite? Get(SButton button);
}
