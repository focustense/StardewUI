// Game data for the Tempering example, i.e. what's in the tempering.json file.
//
// Even though it's not a real mod, the data is still too complex and messy to hard-code and still keep the example
// reasonably readable, so we use these classes to hold the configuration data as a regular mod would.

namespace StardewUITest.Examples.Tempering;

using TemperingData = Dictionary<string, Dictionary<string, EffectData>>;

internal class EffectData
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public Dictionary<string, float> Materials { get; set; } = [];
}

internal class EffectRepository(TemperingData data)
{
    public IEnumerable<string> MaterialIds => effectsByToolAndMaterialId.Keys.Select(key => key.Item2).Distinct();

    private readonly Dictionary<(string, string), MaterialEffectViewModel> effectsByToolAndMaterialId = (
        from toolPair in data
        from effectPair in toolPair.Value
        from materialPair in effectPair.Value.Materials
        select (
            key: (toolPair.Key, materialPair.Key),
            effect: new MaterialEffectViewModel(
                effectPair.Key,
                effectPair.Value.Name,
                effectPair.Value.Description,
                materialPair.Value
            )
        )
    ).ToDictionary(x => x.key, x => x.effect);

    public MaterialEffectViewModel? Get(string toolName, string itemId, int quantity = 1)
    {
        return effectsByToolAndMaterialId.GetValueOrDefault((toolName, itemId))?.WithAdditionalQuantity(quantity);
    }
}
