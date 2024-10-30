using Microsoft.Xna.Framework;
using StardewValley;

namespace StardewUI;

/// <summary>
/// Utilities relating to the game's UI viewport.
/// </summary>
public static class UiViewport
{
    /// <summary>
    /// Gets the maximum size for the entire viewport.
    /// </summary>
    /// <returns>The game's <see cref="Game1.uiViewport"/>, constrained to the viewport of the current
    /// <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/>.</returns>
    public static Point GetMaxSize()
    {
        var maxViewport = Game1.graphics.GraphicsDevice.Viewport;
        return Game1.uiViewport.Width <= maxViewport.Width
            ? new(Game1.uiViewport.Width, Game1.uiViewport.Height)
            : new(maxViewport.Width, maxViewport.Height);
    }
}
