using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;
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
        while (reader.NextTag())
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

    [Fact]
    public void TestDocumentParsing()
    {
        string markup =
            @"<lane orientation=""vertical"" align-content=""middle end"">
                <image width=""400"" sprite={{@Mods/focustense.StardewUITest/Sprites/Header}} />
                <label font=""dialogue"" text={{HeaderText}} />
            </lane>";

        var document = Document.Parse(markup);

        Assert.Equal("lane", document.Root.Tag);
        Assert.Collection(
            document.Root.Attributes,
            attr => Assert.Equal(new("orientation", AttributeValueType.Literal, "vertical"), attr),
            attr => Assert.Equal(new("align-content", AttributeValueType.Literal, "middle end"), attr));
        Assert.Collection(
            document.Root.ChildNodes,
            node =>
            {
                Assert.Equal("image", node.Tag);
                Assert.Collection(
                    node.Attributes,
                    attr => Assert.Equal(new("width", AttributeValueType.Literal, "400"), attr),
                    attr => Assert.Equal(new("sprite", AttributeValueType.Binding, "@Mods/focustense.StardewUITest/Sprites/Header"), attr));
            },
            node =>
            {
                Assert.Equal("label", node.Tag);
                Assert.Collection(
                    node.Attributes,
                    attr => Assert.Equal(new("font", AttributeValueType.Literal, "dialogue"), attr),
                    attr => Assert.Equal(new("text", AttributeValueType.Binding, "HeaderText"), attr));
            });
    }
}