using Microsoft.Xna.Framework;
using StardewUI.Graphics;

namespace StardewUI.Widgets;

/// <summary>
/// An empty view whose sole purpose is to separate other elements.
/// </summary>
/// <remarks>
/// The size of the view is entirely controlled by its <see cref="View.Layout"/>; that is, it is considered to have no
/// content and setting both dimensions to <see cref="Layout.Length.Content"/> will cause it to disappear.
/// </remarks>
public class Spacer : View
{
    /// <inheritdoc />
    protected override void OnDrawContent(ISpriteBatch b) { }

    /// <inheritdoc />
    protected override void OnMeasure(Vector2 availableSize)
    {
        var limits = Layout.GetLimits(availableSize);
        ContentSize = Layout.Resolve(availableSize, () => Vector2.Zero);
    }
}
