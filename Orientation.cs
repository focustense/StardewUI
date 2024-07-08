using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Available orientation directions for views such as <see cref="Lane"/>.
/// </summary>
public enum Orientation
{
    /// <summary>
    /// Content flows in the horizontal direction (generally, left to right).
    /// </summary>
    Horizontal,

    /// <summary>
    /// Content flows in the vertical direction (generally, top to bottom).
    /// </summary>
    Vertical,
}

/// <summary>
/// Helpers for working with <see cref="Orientation"/>.
/// </summary>
public static class OrientationExtensions
{
    /// <summary>
    /// Gets the component of a vector along the orientation's axis.
    /// </summary>
    /// <param name="orientation">The orientation.</param>
    /// <param name="vec">Any vector value.</param>
    /// <returns>The vector's <see cref="Vector2.X"/> component if <see cref="Orientation.Horizontal"/>, or
    /// <see cref="Vector2.Y"/> if <see cref="Orientation.Vertical"/>.</returns>
    public static float Get(this Orientation orientation, Vector2 vec)
    {
        return orientation == Orientation.Horizontal ? vec.X : vec.Y;
    }

    /// <summary>
    /// Sets the component of a vector corresponding to the orientation's axis.
    /// </summary>
    /// <param name="orientation">The orientation.</param>
    /// <param name="vec">Any vector value.</param>
    /// <param name="value">The new value for the specified axis.</param>
    public static void Set(this Orientation orientation, ref Vector2 vec, float value)
    {
        // We could write this in terms of Update, but it would run slower.
        if (orientation == Orientation.Horizontal)
        {
            vec.X = value;
        }
        else
        {
            vec.Y = value;
        }
    }

    /// <summary>
    /// Gets the opposite/perpendicular orientation to a given orientation.
    /// </summary>
    /// <param name="orientation">The orientation.</param>
    public static Orientation Swap(this Orientation orientation)
    {
        return orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="orientation">The orientation.</param>
    /// <param name="vec">Any vector value.</param>
    /// <param name="select">A function that takes the previous value and returns the updated value.</param>
    /// <returns>The value after update.</returns>
    public static float Update(this Orientation orientation, ref Vector2 vec, Func<float, float> select)
    {
        if (orientation == Orientation.Horizontal)
        {
            vec.X = select(vec.X);
            return vec.X;
        }
        else
        {
            vec.Y = select(vec.Y);
            return vec.Y;
        }
    }
}