using StardewModdingAPI;

namespace StardewUI.Widgets.Keybinding;

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

    public Sprite Get(SButton key, out bool isPlaceholder)
    {
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
