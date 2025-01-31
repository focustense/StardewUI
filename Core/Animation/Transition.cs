using System.Diagnostics.CodeAnalysis;

namespace StardewUI.Animation;

/// <summary>
/// Describes the transition behavior of a single property.
/// </summary>
/// <param name="Duration">Duration of the transition.</param>
/// <param name="Delay">Delay during which to hold the current value before transitioning to the new value.</param>
/// <param name="Easing">Type of easing or acceleration curve for the transition.</param>
[DuckType]
public record Transition(TimeSpan Duration, TimeSpan Delay, Easing Easing)
{
    /// <summary>
    /// A <see cref="Transition"/> instance with all values set to their defaults.
    /// </summary>
    public static readonly Transition Default = new(DefaultDuration, DefaultDelay, DefaultEasing!);

    /// <summary>
    /// Default delay (zero) for transitions not specifying an explicit delay.
    /// </summary>
    public static readonly TimeSpan DefaultDelay = TimeSpan.Zero;

    /// <summary>
    /// Default duration (1 second) for transitions not specifying an explicit duration.
    /// </summary>
    public static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Default easing (linear) for transitions not specifying an explicit easing function.
    /// </summary>
    public static readonly Easing DefaultEasing = x => x;

    /// <summary>
    /// The total duration of the transition, including both the animation itself and any pre-delay.
    /// </summary>
    public TimeSpan TotalDuration => Duration + Delay;

    /// <summary>
    /// Initializes a new <see cref="Transition"/> instance.
    /// </summary>
    /// <param name="duration">Duration of the transition. If not specified, the transition will use the
    /// <see cref="DefaultDuration"/>.</param>
    /// <param name="delay">Delay during which to hold the current value before transitioning to the new value. If not
    /// specified, the transition will use the <see cref="DefaultDelay"/>.</param>
    /// <param name="easing">Type of easing or acceleration curve for the transition. If not specified, the transition
    /// will use the <see cref="DefaultEasing"/>.</param>
    public Transition(TimeSpan? duration = null, TimeSpan? delay = null, Easing? easing = null)
        : this(duration ?? DefaultDuration, delay ?? DefaultDelay, easing ?? DefaultEasing) { }

    /// <summary>
    /// Computes the interpolation position for this transition, given the time elapsed since the transition was first
    /// triggered.
    /// </summary>
    /// <remarks>
    /// The value is independent of the type of property being transitioned, or how it is interpolated; the result is
    /// intended to be used as the <c>amount</c> argument to a <see cref="Lerp{T}"/> delegate.
    /// </remarks>
    /// <param name="elapsed">Time elapsed since the transition was initiated.</param>
    /// <returns>The interpolation amount or "y position" at the specified time.</returns>
    public float GetPosition(TimeSpan elapsed)
    {
        if (elapsed < Delay)
        {
            // Being within the delay period should be taken to mean "transition literally hasn't started yet" as
            // opposed to "use the value at t=0" which could be different with some easing functions.
            return 0;
        }
        if (Duration == TimeSpan.Zero)
        {
            return Easing(1);
        }
        elapsed -= Delay;
        var x = elapsed > Duration ? 1f : (float)(elapsed / Duration);
        return Easing(x);
    }

    /// <inheritdoc cref="Parse(ReadOnlySpan{char})" />
    public static Transition Parse(string value)
    {
        return Parse(value.AsSpan());
    }

    /// <summary>
    /// Parses a <see cref="Transition"/> value from a string value.
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <returns>The parsed transition.</returns>
    /// <exception cref="FormatException">Thrown when the <paramref name="value"/> is not a valid transition
    /// string.</exception>
    public static Transition Parse(ReadOnlySpan<char> value)
    {
        return TryParse(value, out var result)
            ? result
            : throw new FormatException($"Invalid transition string '{value}'.");
    }

    /// <summary>
    /// Attempts to parse a <see cref="Transition"/> value from a string value.
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <param name="result">The parsed transition, if successful; otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> was parsed successfully, otherwise <c>false</c>.</returns>
    public static bool TryParse(ReadOnlySpan<char> value, [MaybeNullWhen(false)] out Transition result)
    {
        const char DELIMITER = ' ';

        value = value.Trim();
        if (value.IsEmpty)
        {
            result = Default;
            return true;
        }

        var duration = DefaultDuration;
        var delay = DefaultDelay;
        var easing = DefaultEasing;

        var token = ReadNextToken(value, DELIMITER, out var remaining);
        if (TryParseTime(token, out var parsedDuration))
        {
            duration = parsedDuration;
            value = remaining;

            // Duration and delay use the same syntax, so delay can only appear after a duration was parsed.
            token = ReadNextToken(value, DELIMITER, out remaining);
            if (TryParseTime(token, out var parsedDelay))
            {
                delay = parsedDelay;
                value = remaining;
            }
        }

        // We already guarded against totally empty transition strings at the beginning of the method, so if we get here
        // and the value is *now* empty, it means that it's OK not to have an easing function because we already parsed
        // a duration and possibly delay.
        //
        // If value is non-empty, then whatever is left *must* be a valid easing string, because no other arguments are
        // allowed; otherwise, the entire transition string is invalid.
        if (!value.IsEmpty)
        {
            if (!Easings.TryParse(value, out easing))
            {
                result = null;
                return false;
            }
        }

        result = new(duration, delay, easing);
        return true;
    }

    private static ReadOnlySpan<char> ReadNextToken(
        ReadOnlySpan<char> value,
        char delimiter,
        out ReadOnlySpan<char> remaining
    )
    {
        var delimiterIndex = value.IndexOf(delimiter);
        var result = delimiterIndex >= 0 ? value[..delimiterIndex] : value;
        remaining = delimiterIndex >= 0 ? value[(delimiterIndex + 1)..].Trim() : [];
        return result.Trim();
    }

    private static bool TryParseTime(ReadOnlySpan<char> value, out TimeSpan result)
    {
        // Only seconds and milliseconds supported, both end with "s".
        if (value.Length < 2 || !char.IsDigit(value[0]) || value[^1] is not 's' or 'S')
        {
            result = default;
            return false;
        }
        if (value[^2] is 'm' or 'M' && float.TryParse(value[..^2], out var ms))
        {
            result = TimeSpan.FromMilliseconds(ms);
            return true;
        }
        else if (float.TryParse(value[..^1], out var sec))
        {
            result = TimeSpan.FromSeconds(sec);
            return true;
        }
        result = default;
        return false;
    }
}
