using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace StardewUI;

/// <summary>
/// Event raised when a UI element is clicked by any source (mouse, gamepad, etc.).
/// </summary>
/// <inheritdoc cref="PointerEventArgs(Vector2)" path="/param[@name='position']"/>
/// <param name="button">The specific button that triggered the click.</param>
public class ClickEventArgs(Vector2 position, SButton button) : PointerEventArgs(position), IOffsettable<ClickEventArgs>
{
    /// <summary>
    /// The specific button that triggered the click.
    /// </summary>
    public SButton Button { get; } = button;

    /// <summary>
    /// Gets whether the pressed <see cref="Button"/> is the default for primary actions.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the pressed <see cref="Button"/> is either <see cref="SButton.MouseLeft"/> or the configured
    /// gamepad action button; otherwise, <c>false</c>.
    /// </returns>
    public bool IsPrimaryButton()
    {
        return Button == SButton.MouseLeft || Button.IsActionButton();
    }

    /// <summary>
    /// Gets whether the pressed <see cref="Button"/> is the default for secondary (context) actions.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the pressed <see cref="Button"/> is either <see cref="SButton.MouseRight"/> or the configured
    /// gamepad tool-use button; otherwise, <c>false</c>.
    /// </returns>
    public bool IsSecondaryButton()
    {
        return Button == SButton.MouseRight || Button.IsUseToolButton();
    }

    /// <inheritdoc/>
    public new ClickEventArgs Offset(Vector2 distance)
    {
        return new(Position + distance, Button);
    }
}
