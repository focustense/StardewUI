using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace StardewUI.Animation;

/// <summary>
/// An easing function which computes the Y value between transitions or keyframes.
/// </summary>
/// <param name="x">A value between <c>0.0</c> and <c>1.0</c> representing the position on a timeline.</param>
/// <returns>The <c>y</c> value for the given <paramref name="x"/> value; the interpolation amount to use in the
/// <see cref="Lerp{T}"/> between states. This value does not have to be between <c>0.0</c> and <c>0.1</c>; it can be
/// outside that range, e.g. to create an elastic or bounce effect, but generally should be <c>0</c>> when <c>x</c> is
/// exactly <c>0</c>> and <c>1</c> when <c>x</c> is exactly <c>1</c>.</returns>
public delegate float Easing(float x);

/// <summary>
/// Common registration and lookup for easing functions.
/// </summary>
public static class Easings
{
    private static readonly Dictionary<string, Easing> namedEasings = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Linear", x => x },
        { "EaseInSine", KeySpline.EaseInSine.Get },
        { "EaseInOutSine", KeySpline.EaseInOutSine.Get },
        { "EaseOutSine", KeySpline.EaseOutSine.Get },
        { "EaseInQuad", KeySpline.EaseInQuad.Get },
        { "EaseInOutQuad", KeySpline.EaseInOutQuad.Get },
        { "EaseOutQuad", KeySpline.EaseOutQuad.Get },
        { "EaseInCubic", KeySpline.EaseInCubic.Get },
        { "EaseInOutCubic", KeySpline.EaseInOutCubic.Get },
        { "EaseOutCubic", KeySpline.EaseOutCubic.Get },
        { "EaseInQuart", KeySpline.EaseInQuart.Get },
        { "EaseInOutQuart", KeySpline.EaseInOutQuart.Get },
        { "EaseOutQuart", KeySpline.EaseOutQuart.Get },
        { "EaseInQuint", KeySpline.EaseInQuint.Get },
        { "EaseInOutQuint", KeySpline.EaseInOutQuint.Get },
        { "EaseOutQuint", KeySpline.EaseOutQuint.Get },
        { "EaseInExpo", KeySpline.EaseInExpo.Get },
        { "EaseInOutExpo", KeySpline.EaseInOutExpo.Get },
        { "EaseOutExpo", KeySpline.EaseOutExpo.Get },
        { "EaseInCirc", KeySpline.EaseInCirc.Get },
        { "EaseInOutCirc", KeySpline.EaseInOutCirc.Get },
        { "EaseOutCirc", KeySpline.EaseOutCirc.Get },
        { "EaseInBack", KeySpline.EaseInBack.Get },
        { "EaseInOutBack", KeySpline.EaseInOutBack.Get },
        { "EaseOutBack", KeySpline.EaseOutBack.Get },
        { "EaseInElastic", EaseInElastic },
        { "EaseInOutElastic", EaseInOutElastic },
        { "EaseOutElastic", EaseOutElastic },
        { "EaseInBounce", EaseInBounce },
        { "EaseInOutBounce", EaseInOutBounce },
        { "EaseOutBounce", EaseOutBounce },
    };

    /// <summary>
    /// Registers a new easing, if one with the same name is not already registered.
    /// </summary>
    /// <remarks>
    /// If an easing with the specified <paramref name="name"/> already exists, the call is ignored.
    /// </remarks>
    /// <param name="name">The name of the easing function.</param>
    /// <param name="easing">The easing function.</param>
    public static void Add(string name, Easing easing)
    {
        namedEasings.TryAdd(name, easing);
    }

    /// <summary>
    /// Creates a Cubic Bézier (AKA key spline) easing from two control points.
    /// </summary>
    /// <param name="x1">The X value of the first control point.</param>
    /// <param name="y1">The Y value of the first control point.</param>
    /// <param name="x2">The X value of the second control point.</param>
    /// <param name="y2">The Y value of the second control point.</param>
    /// <returns>The easing function described by the control points.</returns>
    /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/CSS/easing-function/cubic-bezier"/>
    public static Easing CubicBezier(float x1, float y1, float x2, float y2)
    {
        return new KeySpline(x1, y1, x2, y2).Get;
    }

    /// <summary>
    /// Retrieves an easing function, given its registered name.
    /// </summary>
    /// <param name="name">An easing function name, such as <c>EaseOutCubic</c>.</param>
    /// <returns>The easing function registered with the specified <paramref name="name"/>, or <c>null</c> if no such
    /// function was registered.</returns>
    public static Easing? Named(string name)
    {
        return namedEasings.GetValueOrDefault(name);
    }

    /// <inheritdoc cref="Parse(ReadOnlySpan{char})" />
    public static Easing Parse(string value)
    {
        return Parse(value.AsSpan());
    }

    /// <summary>
    /// Parses an <see cref="Easing"/> value from a string value.
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <returns>The parsed easing function.</returns>
    /// <exception cref="FormatException">Thrown when the <paramref name="value"/> is not a known easing function or a
    /// valid user-defined easing definition.</exception>
    public static Easing Parse(ReadOnlySpan<char> value)
    {
        return TryParse(value, out var result)
            ? result
            : throw new FormatException($"Unknown easing function or invalid format: '{value}'.");
    }

    /// <summary>
    /// Attempts to parse an <see cref="Easing"/> value from a string value.
    /// </summary>
    /// <remarks>
    /// Works with both named easings, and known easing functions like <c>CubicBezier</c>.
    /// </remarks>
    /// <param name="value">The string value to parse.</param>
    /// <param name="result">The parsed easing function, if successful; otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> was parsed successfully, otherwise <c>false</c>.</returns>
    public static bool TryParse(ReadOnlySpan<char> value, [MaybeNullWhen(false)] out Easing result)
    {
        value = value.Trim();
        if (namedEasings.TryGetValue(value.ToString(), out result))
        {
            return true;
        }
        if (!value.StartsWith("CubicBezier(", StringComparison.OrdinalIgnoreCase) || value[^1] != ')')
        {
            result = null;
            return false;
        }
        var cubicBezierArgs = value[12..^1];
        if (
            TryParseNextArg(ref cubicBezierArgs, out var x1)
            && TryParseNextArg(ref cubicBezierArgs, out var y1)
            && TryParseNextArg(ref cubicBezierArgs, out var x2)
            && float.TryParse(cubicBezierArgs, out var y2)
        )
        {
            result = CubicBezier(x1, y1, x2, y2);
            return true;
        }
        return false;

        static bool TryParseNextArg(ref ReadOnlySpan<char> args, out float value)
        {
            var separatorIndex = args.IndexOf(',');
            var nextArg = separatorIndex >= 0 ? args[..separatorIndex] : args;
            args = separatorIndex >= 0 ? args[(separatorIndex + 1)..] : [];
            return float.TryParse(nextArg, out value);
        }
    }

    // These functions are common enough to be on easings.net but can't be represented with key splines (cubic bezier).
    private static float EaseInElastic(float x)
    {
        const float c4 = MathHelper.TwoPi / 3;

        if (x <= 0)
        {
            return 0;
        }
        if (x >= 1)
        {
            return 1;
        }
        return -MathF.Pow(2, 10 * x - 10) * MathF.Sin((x * 10 - 10.75f) * c4);
    }

    private static float EaseInOutElastic(float x)
    {
        const float c5 = MathHelper.TwoPi / 4.5f;

        if (x <= 0)
        {
            return 0;
        }
        if (x >= 1)
        {
            return 1;
        }
        return x < 0.5f
            ? -(MathF.Pow(2, 20 * x - 10) * MathF.Sin((20 * x - 11.125f) * c5)) / 2
            : (MathF.Pow(2, -20 * x + 10) * MathF.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
    }

    private static float EaseOutElastic(float x)
    {
        const float c4 = MathHelper.TwoPi / 3;

        if (x <= 0)
        {
            return 0;
        }
        if (x >= 1)
        {
            return 1;
        }
        return MathF.Pow(2, -10 * x) * MathF.Sin((x * 10 - 0.75f) * c4) + 1;
    }

    private static float EaseInBounce(float x)
    {
        return 1 - EaseOutBounce(1 - x);
    }

    private static float EaseInOutBounce(float x)
    {
        return x < 0.5f ? (1 - EaseOutBounce(1 - 2 * x)) / 2 : (1 + EaseOutBounce(2 * x - 1)) / 2;
    }

    private static float EaseOutBounce(float x)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        const float t1 = 1f / d1;
        const float t2 = 2f / d1;
        const float t3 = 2.5f / d1;

        if (x < t1)
        {
            return n1 * x * x;
        }
        if (x < t2)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        if (x < t3)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        return n1 * (x -= 2.625f / d1) * x + 0.984375f;
    }
}
