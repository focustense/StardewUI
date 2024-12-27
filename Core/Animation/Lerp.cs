using Microsoft.Xna.Framework;
using StardewUI.Graphics;
using StardewUI.Layout;

namespace StardewUI.Animation;

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
/// Common registration and lookup for interpolation functions.
/// </summary>
public static class Lerps
{
    private static readonly Dictionary<Type, Delegate> lerpsByType = [];

    static Lerps()
    {
        Add((sbyte x, sbyte y, float a) => (sbyte)MathF.Round(x * (1 - a) + y * a));
        Add((byte x, byte y, float a) => (byte)MathF.Round(x * (1 - a) + y * a));
        Add((short x, short y, float a) => (short)MathF.Round(x * (1 - a) + y * a));
        Add((ushort x, ushort y, float a) => (ushort)MathF.Round(x * (1 - a) + y * a));
        Add<int>(LerpInt);
        Add((uint x, uint y, float a) => (uint)MathF.Round(x * (1 - a) + y * a));
        Add((long x, long y, float a) => (long)MathF.Round(x * (1 - a) + y * a));
        Add((ulong x, ulong y, float a) => (ulong)MathF.Round(x * (1 - a) + y * a));
        Add<float>(MathHelper.LerpPrecise);
        Add((double x, double y, float a) => x * (1 - a) + y * a);
        Add((decimal x, decimal y, float a) => x * (1 - (decimal)a) + y * (decimal)a);
        Add<Point>(LerpPoint);
        Add<Rectangle>(LerpRectangle);
        Add<Vector2>(LerpVector2);
        Add<Edges>(LerpEdges);
        Add<LayoutParameters>(LerpLayoutParameters);
        Add<LayoutParameters?>(LerpLayoutParametersNullable);
        Add<Transform?>(LerpTransform);
        Add<Color>(Color.Lerp);
    }

    /// <summary>
    /// Registers a new interpolation function, if there is not already a function for the same type.
    /// </summary>
    /// <remarks>
    /// If an interpolation function is already known for the type <typeparamref name="T"/>, the call is ignored.
    /// </remarks>
    /// <param name="lerp">Interpolation function for the specified type.</param>
    /// <typeparam name="T">The type of value to interpolate.</typeparam>
    public static void Add<T>(Lerp<T> lerp)
    {
        lerpsByType.TryAdd(typeof(T), lerp);
    }

    /// <summary>
    /// Retrieves the interpolation function for a given type, if one is defined.
    /// </summary>
    /// <typeparam name="T">The type of value to interpolate.</typeparam>
    /// <returns>The interpolation function for type <typeparamref name="T"/>, or <c>null</c> if there is no known
    /// function for the given type.</returns>
    public static Lerp<T>? Get<T>()
    {
        return lerpsByType.TryGetValue(typeof(T), out var lerp) ? (Lerp<T>)lerp : null;
    }

    private static Edges LerpEdges(Edges x, Edges y, float a)
    {
        return new(
            Left: LerpInt(x.Left, y.Left, a),
            Top: LerpInt(x.Top, y.Top, a),
            Right: LerpInt(x.Right, y.Right, a),
            Bottom: LerpInt(x.Bottom, y.Bottom, a)
        );
    }

    private static int LerpInt(int x, int y, float a)
    {
        return (int)MathF.Round(x * (1 - a) + y * a);
    }

    private static LayoutParameters LerpLayoutParameters(LayoutParameters x, LayoutParameters y, float amount)
    {
        if (x.Width.Type != y.Width.Type)
        {
            throw new ArgumentException(
                $"Interpolation of {nameof(LayoutParameters)} can only be performed on instances using the same "
                    + $"width types. The first instance has type {x.Width.Type} and the second has {y.Width.Type}.",
                nameof(y)
            );
        }
        if (x.Height.Type != y.Height.Type)
        {
            throw new ArgumentException(
                $"Interpolation of {nameof(LayoutParameters)} can only be performed on instances using the same "
                    + $"height types. The first instance has type {x.Height.Type} and the second has {y.Height.Type}.",
                nameof(y)
            );
        }
        var widthValue = MathHelper.Lerp(x.Width.Value, y.Width.Value, amount);
        var width = new Length(x.Width.Type, widthValue);
        var heightValue = MathHelper.Lerp(x.Height.Value, y.Height.Value, amount);
        var height = new Length(x.Height.Type, heightValue);
        float? minWidth =
            amount >= 1 ? y.MinWidth
            : x.MinWidth.HasValue || y.MinWidth.HasValue ? MathHelper.Lerp(x.MinWidth ?? 0, y.MinWidth ?? 0, amount)
            : null;
        float? maxWidth =
            amount >= 1 ? y.MaxWidth
            : x.MaxWidth.HasValue && y.MaxWidth.HasValue ? MathHelper.Lerp(x.MaxWidth.Value, y.MaxWidth.Value, amount)
            : (x.MaxWidth ?? y.MaxWidth);
        float? minHeight =
            amount >= 1 ? y.MinHeight
            : x.MinHeight.HasValue || y.MinHeight.HasValue ? MathHelper.Lerp(x.MinHeight ?? 0, y.MinHeight ?? 0, amount)
            : null;
        float? maxHeight =
            amount >= 1 ? y.MaxHeight
            : x.MaxHeight.HasValue && y.MaxHeight.HasValue
                ? MathHelper.Lerp(x.MaxHeight.Value, y.MaxHeight.Value, amount)
            : (x.MaxHeight ?? y.MaxHeight);
        return new()
        {
            Width = width,
            Height = height,
            MinWidth = minWidth,
            MaxWidth = maxWidth,
            MinHeight = minHeight,
            MaxHeight = maxHeight,
        };
    }

    private static LayoutParameters? LerpLayoutParametersNullable(
        LayoutParameters? a,
        LayoutParameters? b,
        float amount
    )
    {
        if (a is null && b is null)
        {
            return null;
        }
        a ??= new LayoutParameters();
        b ??= new LayoutParameters();
        return LerpLayoutParameters(a.Value, b.Value, amount);
    }

    private static Point LerpPoint(Point p1, Point p2, float a)
    {
        return new(LerpInt(p1.X, p2.X, a), LerpInt(p1.Y, p2.Y, a));
    }

    private static Rectangle LerpRectangle(Rectangle r1, Rectangle r2, float a)
    {
        return new(LerpPoint(r1.Location, r2.Location, a), LerpPoint(r1.Size, r2.Size, a));
    }

    private static Transform LerpTransform(Transform? x, Transform? y, float a)
    {
        x ??= Transform.Default;
        y ??= Transform.Default;
        return new(
            Scale: LerpVector2(x.Scale, y.Scale, a),
            Rotation: MathHelper.LerpPrecise(x.Rotation, y.Rotation, a),
            Translation: LerpVector2(x.Translation, y.Translation, a)
        );
    }

    // For some reason, C# compiler doesn't like us referencing Vector2.LerpPrecise directly as a Delegate.
    private static Vector2 LerpVector2(Vector2 x, Vector2 y, float a)
    {
        return Vector2.LerpPrecise(x, y, a);
    }
}
