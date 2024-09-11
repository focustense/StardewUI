using Microsoft.Xna.Framework;

namespace StardewUI;

/// <summary>
/// Performs linear interpolation between two values.
/// </summary>
/// <typeparam name="T">The type of value.</typeparam>
/// <param name="value1">The first, or "start" value to use at <paramref name="amount"/> = <c>0.0</c>.</param>
/// <param name="value2">The second, or "end" value to use at <paramref name="amount"/> = <c>1.0</c>.</param>
/// <param name="amount">The interpolation amount between <c>0.0</c> and <c>1.0</c>.</param>
/// <returns>The interpolated value.</returns>
public delegate T Lerp<T>(T value1, T value2, float amount);

/// <summary>
/// Internal animator abstraction used for driving animations; does not need to know the target or value types, only to
/// have the ability to accept time ticks.
/// </summary>
internal interface IAnimator
{
    bool IsValid();
    void Tick(TimeSpan elapsed);
}

/// <summary>
/// Helpers for creating typed <see cref="Animator{T, V}"/> instances.
/// </summary>
public static class Animator
{
    /// <inheritdoc cref="Animator{T, V}.Animator(T, Func{T, V}, Lerp{V}, Action{T, V})"/>.
    /// <summary>
    /// Creates a new <see cref="Animator{T, V}"/>.
    /// </summary>
    /// <remarks>
    /// Calling this is the same as calling the constructor, but typically does not require explicit type arguments.
    /// </remarks>
    public static Animator<T, V> On<T, V>(T target, Func<T, V> getValue, Lerp<V> lerpValue, Action<T, V> setValue)
        where T : class
    {
        return new(target, getValue, lerpValue, setValue);
    }

    /// <inheritdoc cref="Animator{T, V}.Animator(T, Func{T, V}, Lerp{V}, Action{T, V})"/>.
    /// <summary>
    /// Creates a new <see cref="Animator{T, V}"/> that animates a standard <see cref="float"/> property.
    /// </summary>
    /// <remarks>
    /// Calling this is the same as calling the constructor, but typically does not require explicit type arguments.
    /// </remarks>
    public static Animator<T, float> On<T>(T target, Func<T, float> getValue, Action<T, float> setValue)
        where T : class
    {
        return On(target, getValue, MathHelper.Lerp, setValue);
    }
}

/// <summary>
/// Animates a single property of a single class.
/// </summary>
/// <typeparam name="T">The target class that will receive the animation.</typeparam>
/// <typeparam name="V">The type of value belonging to <typeparamref name="T"/> that should be animated.</typeparam>
public class Animator<T, V> : IAnimator
    where T : class
{
    /// <summary>
    /// The current animation, started by <see cref="Start"/>, if any.
    /// </summary>
    public Animation<V>? CurrentAnimation { get; private set; }

    /// <summary>
    /// Gets whether or not the animator is currently animating in <see cref="Reverse">.
    /// </summary>
    public bool IsReversing { get; private set; }

    /// <summary>
    /// Whether or not the animation should automatically loop back to the beginning when finished.
    /// </summary>
    public bool Loop { get; set; } = false;

    /// <summary>
    /// Whether or not to pause animation. If <c>true</c>, the animator will hold at the current position and not
    /// progress until set to <c>false</c> again. Does not affect the <see cref="CurrentAnimation"/>.
    /// </summary>
    public bool Paused { get; set; }

    private readonly WeakReference<T> targetRef;
    private readonly Func<T, V> getValue;
    private readonly Lerp<V> lerpValue;
    private readonly Action<T, V> setValue;

    private TimeSpan elapsed;

    /// <summary>
    /// Initializes a new <see cref="Animator{T, V}"/>.
    /// </summary>
    /// <param name="target">The object whose property will be animated.</param>
    /// <param name="getValue">Function to get the current value. Used for animations that don't explicit specify a
    /// start value, e.g. when using the <see cref="Start(T, TimeSpan)"/> overload.</param>
    /// <param name="lerpValue">Function to linearly interpolate between the start and end values.</param>
    /// <param name="setValue">Delegate to set the value on the <paramref name="target"/>.</param>
    public Animator(T target, Func<T, V> getValue, Lerp<V> lerpValue, Action<T, V> setValue)
    {
        this.targetRef = new(target);
        this.getValue = getValue;
        this.lerpValue = lerpValue;
        this.setValue = setValue;
        AnimationRunner.Register(this);
    }

    /// <summary>
    /// Causes the animator to animate in the forward direction toward animation's <see cref="Animation{T}.EndValue"/>.
    /// </summary>
    /// <remarks>
    /// Does not restart the animation; if the animator is not reversed, then calling this has no effect.
    /// </remarks>
    public void Forward()
    {
        IsReversing = false;
    }

    /// <summary>
    /// Jumps to the first frame of the current animation, or the last frame if <see cref="IsReversing"/> is
    /// <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Has no effect unless <see cref="CurrentAnimation"/> has been set by a previous call to one of the
    /// <see cref="Start"/> overloads.
    /// </remarks>
    public void Reset()
    {
        if (CurrentAnimation is null || !targetRef.TryGetTarget(out var target))
        {
            return;
        }
        setValue(target, IsReversing ? CurrentAnimation.EndValue : CurrentAnimation.StartValue);
    }

    /// <summary>
    /// Reverses the current animation, so that it gradually returns to the animation's
    /// <see cref="Animation{T}.StartValue"/>.
    /// </summary>
    /// <remarks>
    /// Calling <see cref="Reverse"/> is different from starting a new animation with reversed start and end values;
    /// specifically, it will follow the timeline/curve backward from the current progress. If only 1/4 second of a
    /// 1-second animation elapsed in the forward direction, then the reverse animation will also only take 1/4 second.
    /// </remarks>
    public void Reverse()
    {
        IsReversing = true;
    }

    /// <summary>
    /// Starts a new animation.
    /// </summary>
    /// <param name="animation">The animation settings.</param>
    public void Start(Animation<V> animation)
    {
        if (!targetRef.TryGetTarget(out var target))
        {
            return;
        }
        setValue(target, animation.StartValue);
        CurrentAnimation = animation;
        elapsed = IsReversing ? animation.Duration : TimeSpan.Zero;
    }

    /// <summary>
    /// Starts a new animation using the specified start/end values and duration.
    /// </summary>
    /// <param name="startValue">The initial value of the animation property. This will take effect immediately, even if
    /// it is far away from the current value; i.e. it may cause "jumps".</param>
    /// <param name="endValue">The final value to be reached once the <paramref name="duration"/> ends.</param>
    /// <param name="duration">Duration of the animation; defaults to 1 second if not specified.</param>
    public void Start(V startValue, V endValue, TimeSpan? duration = null)
    {
        Start(new(startValue, endValue, duration ?? TimeSpan.FromSeconds(1)));
    }

    /// <summary>
    /// Starts a new animation that begins at the current value and ends at the specified value after the specified
    /// duration.
    /// </summary>
    /// <param name="endValue">The final value to be reached once the <paramref name="duration"/> ends.</param>
    /// <param name="duration">Duration of the animation; defaults to 1 second if not specified.</param>
    public void Start(V endValue, TimeSpan duration)
    {
        if (!targetRef.TryGetTarget(out var target))
        {
            return;
        }
        Start(new(getValue(target), endValue, duration));
    }

    /// <summary>
    /// Completely stops animating, removing the <see cref="CurrentAnimation"/> and resetting animation state such as
    /// <see cref="Reverse"/> and <see cref="Paused"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This tries to put the animator in the same state it was in when first created. To preserve the current animation
    /// but pause progress and be able to resume later, set <see cref="Paused"/> instead.
    /// </para>
    /// <para>
    /// Calling this does <b>not</b> reset the animated object to the animation's starting value. To do this, call
    /// <see cref="Reset"/> before calling <see cref="Stop"/> (not after, as <see cref="Reset"/> has no effect once the
    /// <see cref="CurrentAnimation"/> is cleared).
    /// </para>
    /// </remarks>
    public void Stop()
    {
        CurrentAnimation = null;
        IsReversing = false;
        Paused = false;
    }

    /// <summary>
    /// Continues animating in the current direction.
    /// </summary>
    /// <param name="elapsed">Time elapsed since last tick.</param>
    public void Tick(TimeSpan elapsed)
    {
        if (
            Paused
            || !targetRef.TryGetTarget(out var target)
            || CurrentAnimation is null
            || (!Loop && IsReversing && this.elapsed == TimeSpan.Zero)
            || (!Loop && !IsReversing && this.elapsed >= CurrentAnimation.Duration)
        )
        {
            return;
        }
        if (IsReversing)
        {
            this.elapsed -= elapsed;
            if (this.elapsed < TimeSpan.Zero)
            {
                this.elapsed = Loop ? CurrentAnimation.Duration : TimeSpan.Zero;
            }
        }
        else
        {
            this.elapsed += elapsed;
            if (this.elapsed >= CurrentAnimation.Duration)
            {
                this.elapsed = Loop ? TimeSpan.Zero : CurrentAnimation.Duration;
            }
        }
        var progress = CurrentAnimation.Duration > TimeSpan.Zero ? this.elapsed / CurrentAnimation.Duration : 0;
        var value = lerpValue(CurrentAnimation.StartValue, CurrentAnimation.EndValue, (float)progress);
        setValue(target, value);
    }

    bool IAnimator.IsValid()
    {
        return targetRef.TryGetTarget(out _);
    }
}

/// <summary>
/// Defines a single animation.
/// </summary>
/// <typeparam name="T">The type of value being animated.</typeparam>
/// <param name="StartValue">The initial value for the animated property.</param>
/// <param name="EndValue">The final value for the animated property.</param>
/// <param name="Duration">Duration of the animation.</param>
public record Animation<T>(T StartValue, T EndValue, TimeSpan Duration);

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
    /// progress until set to <c>false</c> again. Does not affect the <see cref="CurrentAnimation"/>.
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

    public SpriteAnimator(Image image)
    {
        imageRef = new(image);
        AnimationRunner.Register(this);
    }

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

/// <summary>
/// Central registry for view animations, meant to be driven from the game loop.
/// </summary>
public static class AnimationRunner
{
    private static readonly LinkedList<IAnimator> animators = [];

    internal static void Register(IAnimator animator)
    {
        animators.AddLast(animator);
    }

    /// <summary>
    /// Handles a game tick.
    /// </summary>
    /// <param name="elapsed">Time elapsed since last tick.</param>
    public static void Tick(TimeSpan elapsed)
    {
        var node = animators.First;
        while (node is not null)
        {
            var nextNode = node.Next;
            if (node.Value.IsValid())
            {
                node.Value.Tick(elapsed);
            }
            else
            {
                animators.Remove(node);
            }
            node = nextNode;
        }
    }
}
