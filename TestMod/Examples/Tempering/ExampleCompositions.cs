namespace StardewUITest.Examples.Tempering;

// These would normally be in the savegame, mod data, etc. For the example, we hardcode them.
internal static class ExampleCompositions
{
    public static readonly Dictionary<string, int> Axe = new()
    {
        { "(O)86", 10 },
        { "(O)546", 4 },
        { "(O)548", 3 },
    };

    public static readonly Dictionary<string, int> Pickaxe = new()
    {
        { "(O)547", 3 },
        { "(O)561", 4 },
        { "(O)558", 3 },
        { "(O)546", 6 },
        { "(O)578", 2 },
    };

    public static readonly Dictionary<string, int> Hoe = new()
    {
        { "(O)543", 12 },
        { "(O)539", 8 },
        { "(O)574", 7 },
    };
}
