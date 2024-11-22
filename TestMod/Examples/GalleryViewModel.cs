using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PropertyChanged.SourceGenerator;
using StardewValley;

namespace StardewUITest.Examples;

internal partial class GalleryViewModel(IReadOnlyList<GalleryExampleViewModel> examples)
{
    private const int fadeInDurationMs = 400;
    private const int fadeInStartMs = 200;
    private const int scaleInDurationMs = 500;

    public float BackgroundScale => EaseOutElastic((float)TimeSinceOpened.TotalMilliseconds / scaleInDurationMs);
    public float ContentOpacity =>
        Math.Clamp(((float)TimeSinceOpened.TotalMilliseconds - fadeInStartMs) / fadeInDurationMs, 0f, 1f);
    public IReadOnlyList<GalleryExampleViewModel> Examples { get; } = examples;

    [Notify(Getter.Private)]
    private TimeSpan timeSinceOpened;

    private bool isAnimating = true;

    public void Update(TimeSpan elapsed)
    {
        if (!isAnimating)
        {
            return;
        }
        TimeSinceOpened += elapsed;
        if (timeSinceOpened.TotalMilliseconds > Math.Max(fadeInStartMs + fadeInDurationMs, scaleInDurationMs))
        {
            isAnimating = false;
        }
    }

    private static float EaseOutElastic(float x)
    {
        const float c4 = 2 * MathF.PI / 3;

        if (x <= 0 || x >= 1)
        {
            return Math.Clamp(x, 0, 1);
        }
        return MathF.Pow(2, -10 * x) * MathF.Sin((x * 10 - 0.75f) * c4) + 1;
    }
}

internal partial class GalleryExampleViewModel(
    string title,
    string description,
    string thumbnailItemId,
    Action openAction
)
{
    public string Title { get; } = title;
    public string Description { get; } = description;
    public GalleryThumbnail Thumbnail { get; } = GalleryThumbnail.ForItem(thumbnailItemId);

    private static readonly Color hoverBackgroundTint = new(0x11, 0x44, 0xcc, 0x88);
    private static readonly TimeSpan hoverDuration = TimeSpan.FromMilliseconds(350);

    [Notify]
    private Color backgroundTint;

    private TimeSpan elapsedAnimationTime;
    private bool isHovering;

    public void EndHover()
    {
        isHovering = false;
    }

    public void Open()
    {
        openAction();
    }

    public void StartHover()
    {
        isHovering = true;
    }

    public void Update(TimeSpan elapsed)
    {
        bool isAnimating = false;
        if (isHovering && elapsedAnimationTime < hoverDuration)
        {
            elapsedAnimationTime += elapsed;
            if (elapsedAnimationTime > hoverDuration)
            {
                elapsedAnimationTime = hoverDuration;
            }
            isAnimating = true;
        }
        else if (!isHovering && elapsedAnimationTime > TimeSpan.Zero)
        {
            elapsedAnimationTime -= elapsed < elapsedAnimationTime ? elapsed : elapsedAnimationTime;
            isAnimating = true;
        }
        if (isAnimating)
        {
            BackgroundTint = hoverBackgroundTint * (float)(elapsedAnimationTime / hoverDuration);
        }
    }
}

internal record GalleryThumbnail(Texture2D Texture, Rectangle SourceRect, SliceSettings SliceSettings)
{
    public static GalleryThumbnail ForItem(string itemId)
    {
        var itemData = ItemRegistry.GetDataOrErrorItem(itemId);
        return new(itemData.GetTexture(), itemData.GetSourceRect(), new(4f));
    }
}

internal record SliceSettings(float Scale);
