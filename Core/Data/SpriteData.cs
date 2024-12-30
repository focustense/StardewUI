using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewUI.Graphics;
using StardewUI.Layout;

namespace StardewUI.Data;

/// <summary>
/// JSON data for a sprite sheet configuration.
/// </summary>
/// <remarks>
/// Only used externally; internally, all UI code uses <see cref="Sprite"/>.
/// </remarks>
internal class SpriteSheetData
{
    /// <summary>
    /// The name of the texture asset (not path) to load for all <see cref="Sprites"/>.
    /// </summary>
    [JsonProperty("Texture")]
    public string TextureAssetName { get; set; } = "";

    /// <summary>
    /// The individual sprites - source rectangles and other attributes.
    /// </summary>
    public Dictionary<string, SpriteData> Sprites { get; set; } = [];
}

/// <summary>
/// JSON data for an individual sprite configuration.
/// </summary>
/// <remarks>
/// Requires the parent <see cref="SpriteData"/> for identifying the texture. Only used externally; internally, all UI
/// code uses <see cref="Sprite"/>.
/// </remarks>
internal class SpriteData
{
    /// <summary>
    /// The rectangular region of the source texture where the sprite is located.
    /// </summary>
    [JsonConverter(typeof(RectangleJsonConverter))]
    public Rectangle SourceRect { get; set; }

    /// <summary>
    /// The sprite's fixed edges, containing the length on each side (left, top, right, bottom) that should not be
    /// scaled with the rest of the image. Used for nine-slice sprites.
    /// </summary>
    [JsonConverter(typeof(EdgesJsonConverter))]
    public Edges? FixedEdges { get; set; }

    /// <summary>
    /// Additional settings specific to nine-slicing.
    /// </summary>
    public SliceSettings? SliceSettings { get; set; }

    /// <summary>
    /// The <em>native</em> scale at which to draw the sprite.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Sprites drawn as part of an <see cref="Widgets.Image"/> are always scaled according to the layout dimensions and
    /// fit settings; specifying a non-unity scale here will also cause any <see cref="FixedEdges"/> to be scaled. This
    /// is meant for nine-slice sprites that need to scale borders up by a fixed amount (most often 4, sometimes 2) and
    /// the center slice by an arbitrary amount.
    /// </para>
    /// <para>
    /// This setting is shorthand for <see cref="SliceSettings.Scale"/> since it is the most commonly used slice
    /// setting. Providing a non-default value will create the <see cref="SliceSettings"/> instance.
    /// </para>
    /// </remarks>
    public float Scale
    {
        get => SliceSettings?.Scale ?? 1f;
        set => SliceSettings = SliceSettings?.WithScale(value) ?? new(Scale: value);
    }

    /// <summary>
    /// Creates a <see cref="Sprite"/> based on this data.
    /// </summary>
    /// <param name="texture">The texture containing the sprite.</param>
    public Sprite CreateSprite(Texture2D texture)
    {
        return new(texture, SourceRect, FixedEdges, SliceSettings);
    }
}
