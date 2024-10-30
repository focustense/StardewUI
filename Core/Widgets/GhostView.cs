using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewUI.Graphics;

namespace StardewUI.Widgets;

/// <summary>
/// A view that draws an exact copy of another view, generally with a tint and transparency to indicate that it is not
/// the original view. Can be used for dragging, indicating target snap positions, etc.
/// </summary>
/// <remarks>
/// The <see cref="RealView"/> must be part of a real layout in order for the ghosting to work correctly;
/// <see cref="GhostView"/> does no layout or layout-forwarding of its own.
/// </remarks>
public class GhostView : View
{
    /// <summary>
    /// The view for which a copy will be drawn.
    /// </summary>
    public IView? RealView
    {
        get => realView;
        set
        {
            if (value != realView)
            {
                realView = value;
                OnPropertyChanged(nameof(RealView));
            }
        }
    }

    /// <summary>
    /// Color of the ghost.
    /// </summary>
    /// <remarks>
    /// This tint is multiplied against the <see cref="RealView"/>'s pixel values and acts as a recolor; for example,
    /// specifying <see cref="Color.Red"/> here will result in the ghost being entirely red and black.
    /// </remarks>
    public Color TintColor
    {
        get => tintColor;
        set
        {
            if (value != tintColor)
            {
                tintColor = value;
                OnPropertyChanged(nameof(TintColor));
            }
        }
    }

    private IView? realView;
    private Color tintColor = Color.White;

    /// <inheritdoc />
    protected override void OnDrawContent(ISpriteBatch b)
    {
        if (RealView is null)
        {
            return;
        }
        using var _ = b.Blend(
            new BlendState()
            {
                AlphaSourceBlend = Blend.One,
                ColorSourceBlend = Blend.BlendFactor,
                BlendFactor = TintColor,
                AlphaDestinationBlend = Blend.InverseSourceAlpha,
                ColorDestinationBlend = Blend.InverseSourceAlpha,
            }
        );
        RealView.Draw(b);
    }

    /// <inheritdoc />
    protected override void OnMeasure(Vector2 availableSize)
    {
        ContentSize = RealView?.ContentBounds.Size ?? Vector2.Zero;
    }
}
