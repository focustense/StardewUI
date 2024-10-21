using StardewUI.Events;
using StardewUI.Widgets;

namespace StardewUI.Animation;

/// <summary>
/// Standalone scaling behavior that can be attached to any <see cref="Image"/>, causing it to scale up to a specified
/// amount when hovered by the pointer.
/// </summary>
public class HoverScale
{
    /// <summary>
    /// Attaches a new hover behavior to an image.
    /// </summary>
    /// <param name="image">The image that will receive the hover behavior.</param>
    /// <param name="maxScale">Target scale at the end of the animation; generally a number > 1.</param>
    /// <param name="duration">Duration of the animation; if not specified, defaults to 80 ms.</param>
    public static void Attach(Image image, float maxScale, TimeSpan? duration = null)
    {
        var animator = Animator.On(image, i => i.Scale, (i, scale) => i.Scale = scale);
        var instance = new HoverScale(animator, maxScale, duration ?? TimeSpan.FromMilliseconds(80));
        image.PointerEnter += instance.Image_PointerEnter;
        image.PointerLeave += instance.Image_PointerLeave;
    }

    private readonly Animator<Image, float> animator;
    private readonly float maxScale;
    private readonly TimeSpan duration;

    private HoverScale(Animator<Image, float> animator, float maxScale, TimeSpan duration)
    {
        this.maxScale = maxScale;
        this.duration = duration;
        this.animator = animator;
    }

    private void Image_PointerEnter(object? sender, PointerEventArgs e)
    {
        if (animator.CurrentAnimation is null)
        {
            animator.Start(1, maxScale, duration);
        }
        else
        {
            animator.Forward();
        }
    }

    private void Image_PointerLeave(object? sender, PointerEventArgs e)
    {
        animator.Reverse();
    }
}
