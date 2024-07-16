using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

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
