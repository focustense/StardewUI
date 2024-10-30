using Microsoft.Xna.Framework;

namespace StardewUI.Layout;

/// <summary>
/// Provides a method to clone the current instance with an offset applied.
/// </summary>
/// <typeparam name="T">The output type; should be the same as the implementing class.</typeparam>
public interface IOffsettable<T>
{
    /// <summary>
    /// Creates a clone of this instance with an offset applied to its position.
    /// </summary>
    /// <param name="distance">The offset distance.</param>
    T Offset(Vector2 distance);
}

/// <summary>
/// Extensions for the <see cref="IOffsettable{T}"/> interface.
/// </summary>
public static class OffsettableExtensions
{
    /// <summary>
    /// Clones an <see cref="IOffsettable{T}"/>.
    /// </summary>
    /// <remarks>
    /// Since every <see cref="IOffsettable{T}.Offset"/> is implicitly a clone, we can perform an "explicit" clone by
    /// providing a zero offset.
    /// </remarks>
    /// <typeparam name="T">The source/output type.</typeparam>
    /// <param name="instance">The instance to clone.</param>
    /// <returns>A copy of the <paramref name="instance"/>.</returns>
    public static T Clone<T>(this T instance)
        where T : IOffsettable<T>
    {
        return instance.Offset(Vector2.Zero);
    }
}
