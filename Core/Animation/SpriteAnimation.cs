using StardewUI.Graphics;
using StardewUI.Widgets;

namespace StardewUI.Animation;

/// <summary>
/// Animates the sprite of an <see cref="Image"/>, using equal duration for all frames in a list.
/// </summary>
public class SpriteAnimator : IAnimator
{
    /// <summary>
    /// Duration of each frame.
    /// </summary>
    public TimeSpan FrameDuration { get; set; }

    /// <summary>
    /// Frames to animate through.
    /// </summary>
    public IReadOnlyList<Sprite> Frames { get; set; } = [];

    /// <summary>
    /// Whether or not to pause animation. If <c>true</c>, the animator will hold at the current position and not
    /// progress until set to <c>false</c> again.
    /// </summary>
    public bool Paused { get; set; }

    /// <summary>
    /// Delay before advancing from the first frame to the next frames.
    /// </summary>
    /// <remarks>
    /// Repeats on every loop, but only applies to the first frame of each loop.
    /// </remarks>
    public TimeSpan StartDelay { get; set; }

    private readonly WeakReference<Image> imageRef;

    private TimeSpan delayElapsed;
    private TimeSpan elapsed;

    /// <summary>
    /// Initializes a new instance of <see cref="SpriteAnimator"/> that animates the sprite on a specified image.
    /// </summary>
    /// <param name="image">The image to animate.</param>
    public SpriteAnimator(Image image)
    {
        imageRef = new(image);
        AnimationRunner.Register(this);
    }

    /// <summary>
    /// Resets the animation to the first frame, and waits any <see cref="StartDelay"/> required again.
    /// </summary>
    public void Reset()
    {
        if (!imageRef.TryGetTarget(out var image))
        {
            return;
        }
        image.Sprite = Frames[0];
        delayElapsed = TimeSpan.Zero;
        elapsed = TimeSpan.Zero;
    }

    /// <inheritdoc />
    public void Tick(TimeSpan elapsed)
    {
        if (Paused || FrameDuration == TimeSpan.Zero || Frames.Count == 0 || !imageRef.TryGetTarget(out var image))
        {
            return;
        }
        if (this.elapsed == TimeSpan.Zero && delayElapsed < StartDelay)
        {
            delayElapsed += elapsed;
            if (Frames.Count > 0)
            {
                image.Sprite = Frames[0];
            }
            if (delayElapsed < StartDelay)
            {
                return;
            }
        }
        delayElapsed = TimeSpan.Zero;
        var totalDuration = FrameDuration * Frames.Count;
        this.elapsed += elapsed;
        if (this.elapsed > totalDuration)
        {
            this.elapsed =
                StartDelay > TimeSpan.Zero
                    ? TimeSpan.Zero
                    : TimeSpan.FromTicks(this.elapsed.Ticks % totalDuration.Ticks);
        }
        var frameIndex = (int)(this.elapsed.TotalMilliseconds / FrameDuration.TotalMilliseconds);
        image.Sprite = Frames[frameIndex];
    }

    bool IAnimator.IsValid()
    {
        return imageRef.TryGetTarget(out _);
    }
}
