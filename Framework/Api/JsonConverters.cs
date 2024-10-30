using Newtonsoft.Json;
using StardewUI.Framework.Converters;
using StardewUI.Layout;

namespace StardewUI.Framework.Api;

/// <summary>
/// JSON converter for the <see cref="Edges"/> type.
/// </summary>
internal class EdgesJsonConverter : JsonConverter<Edges>
{
    public override Edges? ReadJson(
        JsonReader reader,
        Type objectType,
        Edges? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer
    )
    {
        return reader.TokenType switch
        {
            JsonToken.Null => null,
            JsonToken.String => Edges.Parse((string)reader.Value!),
            _ => throw new FormatException($"Can't parse {typeof(Edges).Name} from a {reader.TokenType} JSON token."),
        };
    }

    public override void WriteJson(JsonWriter writer, Edges? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString());
    }
}

/// <summary>
/// JSON converter for the <see cref="Rectangle"/> type.
/// </summary>
internal class RectangleJsonConverter : JsonConverter<Rectangle>
{
    public override Rectangle ReadJson(
        JsonReader reader,
        Type objectType,
        Rectangle existingValue,
        bool hasExistingValue,
        JsonSerializer serializer
    )
    {
        return reader.TokenType switch
        {
            JsonToken.Null => default,
            JsonToken.String => RectangleConverter.Parse((string)reader.Value!),
            _ => throw new FormatException(
                $"Can't parse {typeof(Rectangle).Name} from a {reader.TokenType} JSON token."
            ),
        };
    }

    public override void WriteJson(JsonWriter writer, Rectangle value, JsonSerializer serializer)
    {
        writer.WriteValue($"{value.X}, {value.Y}, {value.Width}, {value.Height}");
    }
}
