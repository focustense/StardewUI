using System.Runtime.CompilerServices;

namespace StardewUI.Animation;

internal readonly struct KeySpline(float x1, float y1, float x2, float y2)
{
    public static readonly KeySpline Linear = new(0f, 0f, 1f, 1f);
    public static readonly KeySpline EaseInSine = new(0.12f, 0.0f, 0.39f, 0.0f);
    public static readonly KeySpline EaseOutSine = new(0.61f, 1.0f, 0.88f, 1.0f);
    public static readonly KeySpline EaseInOutSine = new(0.37f, 0.0f, 0.63f, 1.0f);
    public static readonly KeySpline EaseInQuad = new(0.11f, 0.0f, 0.5f, 0.0f);
    public static readonly KeySpline EaseOutQuad = new(0.5f, 1.0f, 0.89f, 1.0f);
    public static readonly KeySpline EaseInOutQuad = new(0.45f, 0.0f, 0.55f, 1.0f);
    public static readonly KeySpline EaseInCubic = new(0.32f, 0.0f, 0.67f, 0.0f);
    public static readonly KeySpline EaseOutCubic = new(0.33f, 1.0f, 0.68f, 1.0f);
    public static readonly KeySpline EaseInOutCubic = new(0.65f, 0.0f, 0.35f, 1.0f);
    public static readonly KeySpline EaseInQuart = new(0.5f, 0.0f, 0.75f, 0.0f);
    public static readonly KeySpline EaseOutQuart = new(0.25f, 1.0f, 0.5f, 1.0f);
    public static readonly KeySpline EaseInOutQuart = new(0.76f, 0.0f, 0.24f, 1.0f);
    public static readonly KeySpline EaseInQuint = new(0.64f, 0.0f, 0.78f, 0.0f);
    public static readonly KeySpline EaseOutQuint = new(0.22f, 1.0f, 0.36f, 1.0f);
    public static readonly KeySpline EaseInOutQuint = new(0.83f, 0.0f, 0.17f, 1.0f);
    public static readonly KeySpline EaseInExpo = new(0.7f, 0.0f, 0.84f, 0.0f);
    public static readonly KeySpline EaseOutExpo = new(0.16f, 1.0f, 0.3f, 1.0f);
    public static readonly KeySpline EaseInOutExpo = new(0.87f, 0.0f, 0.13f, 1.0f);
    public static readonly KeySpline EaseInCirc = new(0.55f, 0.0f, 1.0f, 0.45f);
    public static readonly KeySpline EaseOutCirc = new(0.0f, 0.55f, 0.45f, 1.0f);
    public static readonly KeySpline EaseInOutCirc = new(0.85f, 0.0f, 0.15f, 1.0f);
    public static readonly KeySpline EaseInBack = new(0.36f, 0.0f, 0.66f, -0.56f);
    public static readonly KeySpline EaseOutBack = new(0.34f, 1.56f, 0.64f, 1.0f);
    public static readonly KeySpline EaseInOutBack = new(0.68f, -0.6f, 0.32f, 1.6f);

    public float Get(float x)
    {
        return x1 == y1 && x2 == y2 ? x : GetBezier(GetT(x), y1, y2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float A(float a1, float a2) => 1f - 3f * a2 + 3f * a1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float B(float a1, float a2) => 3f * a2 - 6f * a1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float C(float a1) => 3f * a1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetBezier(float t, float a1, float a2) => ((A(a1, a2) * t + B(a1, a2)) * t + C(a1)) * t;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetSlope(float t, float a1, float a2) => 3f * A(a1, a2) * t * t + 2f * B(a1, a2) * t + C(a1);

    private float GetT(float x)
    {
        const int MAX_ITERATIONS = 4;

        float t = x; // Current guess
        for (int i = 0; i < MAX_ITERATIONS; i++)
        {
            float slope = GetSlope(t, x1, x2);
            if (slope == 0.0)
            {
                return t;
            }
            float bx = GetBezier(t, x1, x2) - x;
            t -= bx / slope;
        }
        return t;
    }
}
