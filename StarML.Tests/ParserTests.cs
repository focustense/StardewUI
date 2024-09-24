using StardewUITest.StarML;
using Xunit.Abstractions;

namespace StarML.Tests;

public class ParserTests(ITestOutputHelper output)
{
    [Fact]
    public void TestParserBasic()
    {
        string markup =
            @"<lane orientation=""vertical"" align-content=""middle end"">
                <image width=""400"" sprite={{@Mods/focustense.StardewUITest/Sprites/Header}} />
                <label font=""dialogue"" text={{HeaderText}} />
            </lane>";

        var reader = new DocumentReader(markup);
        while (reader.NextElement())
        {
            var label = reader.Tag.IsClosingTag ? "Closing Tag" : "Opening Tag";
            output.WriteLine($"{label}: {reader.Tag.Name}");
            while (reader.NextAttribute())
            {
                var attribute = reader.Attribute;
                output.WriteLine($"  {attribute.Name} = [{attribute.ValueType}] {attribute.Value}");
            }
        }
    }
}