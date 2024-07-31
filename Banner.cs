using Microsoft.Xna.Framework;
using StardewValley.BellsAndWhistles;

namespace StardewUI;

/// <summary>
/// Draws banner-style text with an optional background.
/// </summary>
/// <remarks>
/// This is very similar to a <see cref="Label"/> inside a <see cref="Frame"/>, but uses the special
/// <see cref="SpriteText"/> font which is more prominent than any of the game's available
/// <see cref="Microsoft.Xna.Framework.Graphics.SpriteFont"/>s and often used for top-level headings/menu titles.
/// </remarks>
public class Banner : View
{
    /// <summary>
    /// Background sprite (including border) to draw underneath the text.
    /// </summary>
    public Sprite? Background { get; set; }

    /// <summary>
    /// The thickness of the border edges within the <see cref="Background"/>. sprite.
    /// </summary>
    /// <remarks>
    /// This property has no effect on the appearance of the <see cref="Background"/>, but affects how content is
    /// positioned inside the border. It is often correct to set it to the same value as the
    /// <see cref="Sprite.FixedEdges"/> of the <see cref="Background"/> sprite, but the values are considered
    /// independent.
    /// </remarks>
    public Edges BackgroundBorderThickness
    {
        get => backgroundBorderThickness.Value;
        set => backgroundBorderThickness.Value = value;
    }

    /// <summary>
    /// The text to display within the banner.
    /// </summary>
    public string Text
    {
        get => text.Value;
        set => text.Value = value;
    }

    private readonly DirtyTracker<Edges> backgroundBorderThickness = new(Edges.NONE);
    private readonly DirtyTracker<string> text = new("");

    private NineSlice? backgroundSlice;
    private Vector2 textSize;

    protected override Edges GetBorderThickness()
    {
        return BackgroundBorderThickness;
    }

    protected override bool IsContentDirty()
    {
        return backgroundBorderThickness.IsDirty || text.IsDirty;
    }

    protected override void OnDrawBorder(ISpriteBatch b)
    {
        backgroundSlice?.Draw(b);
    }

    protected override void OnDrawContent(ISpriteBatch b)
    {
        var centerX = ContentSize.X / 2;
        b.DelegateDraw(
            (wb, origin) => SpriteText.drawStringHorizontallyCenteredAt(
                wb, Text, (int)(origin.X + centerX), (int)origin.Y));
    }

    protected override void OnMeasure(Vector2 availableSize)
    {
        var width = SpriteText.getWidthOfString(Text);
        var height = SpriteText.getHeightOfString(Text);
        textSize = new(width, height);
        ContentSize = Layout.Resolve(availableSize, () => textSize);

        if (backgroundSlice?.Sprite != Background)
        {
            backgroundSlice = Background is not null ? new(Background) : null;
        }
        backgroundSlice?.Layout(new(Point.Zero, BorderSize.ToPoint()));
    }

    protected override void ResetDirty()
    {
        backgroundBorderThickness.ResetDirty();
        text.ResetDirty();
    }
}
