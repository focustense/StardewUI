namespace StardewUI.Graphics;

/// <summary>
/// Provides a single method to obtain a sprite for some key, such as <see cref="SButton"/>.
/// </summary>
/// <typeparam name="T">Type of key for which to obtain sprites.</typeparam>
public interface ISpriteMap<T>
{
    /// <summary>
    /// Gets the sprite corresponding to a particular key.
    /// </summary>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="isPlaceholder"><c>true</c> if the returned <see cref="Sprite"/> is not specific to the
    /// <paramref name="key"/>, but is instead a placeholder (border/background) in which some substitute, typically
    /// normal text, must be drawn. <c>false</c> if the <see cref="Sprite"/> is a complete self-contained representation
    /// of the <paramref name="key"/>.</param>
    /// <returns>The precise or generic sprite for the given <paramref name="key"/>.</returns>
    Sprite Get(T key, out bool isPlaceholder);
}
