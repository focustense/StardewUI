using Microsoft.Xna.Framework;

namespace StardewUI.Layout;

/// <summary>
/// Available orientation directions for views such as <see cref="Widgets.Lane"/>.
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
    /// Creates a new <see cref="Vector2"/> with the oriented dimension set to a specified length and the other
    /// dimension set to zero.
    /// </summary>
    /// <param name="orientation">The orientation.</param>
    /// <param name="length">The length along the orientation axis.</param>
    /// <returns>A new <see cref="Vector2"/> whose length along the <paramref name="orientation"/> axis is
    /// <paramref name="length"/>.</returns>
    public static Vector2 CreateVector(this Orientation orientation, float length)
    {
        var result = Vector2.Zero;
        orientation.Set(ref result, length);
        return result;
    }

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
    /// Gets the dimension setting of a layout along the orientation's axis.
    /// </summary>
    /// <param name="orientation">The orientation.</param>
    /// <param name="layout">Layout parameters to extract from.</param>
    /// <returns>The <see cref="LayoutParameters.Width"/> of the specified <paramref name="layout"/> if the orientation
    /// is <see cref="Orientation.Horizontal"/>; <see cref="LayoutParameters.Height"/> if
    /// <see cref="Orientation.Vertical"/>.</returns>
    public static Length Length(this Orientation orientation, LayoutParameters layout)
    {
        return orientation == Orientation.Horizontal ? layout.Width : layout.Height;
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
