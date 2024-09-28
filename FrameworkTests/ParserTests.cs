using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;
using Xunit.Abstractions;

namespace StarML.Tests;

public class ParserTests(ITestOutputHelper output)
{
    public record TagExpectation(string Name, SAttribute[]? Attributes = null, bool IsClosingTag = false);

    public static TheoryData<string, TagExpectation[]> Data => new()
    {
        {
            @"<lane orientation=""vertical"" align-content=""middle end"">
                <image width={{<ImageWidth}} sprite={{@Mods/focustense.StardewUITest/Sprites/Header}} />
                <label font=""dialogue"" text={{HeaderText}} />
                <checkbox is-checked={{<>Checked}}/>
            </lane>",
            [
                new("lane", [
                    new("orientation", AttributeValueType.Literal, "vertical"),
                    new("align-content", AttributeValueType.Literal, "middle end"),
                ]),
                new("image", [
                    new("width", AttributeValueType.InputBinding, "ImageWidth"),
                    new("sprite", AttributeValueType.AssetBinding, "Mods/focustense.StardewUITest/Sprites/Header"),
                ]),
                new("image", IsClosingTag: true),
                new("label", [
                    new("font", AttributeValueType.Literal, "dialogue"),
                    new("text", AttributeValueType.InputBinding, "HeaderText"),
                ]),
                new("label", IsClosingTag: true),
                new("checkbox", [
                    new("is-checked", AttributeValueType.TwoWayBinding, "Checked"),
                ]),
                new("checkbox", IsClosingTag: true),
                new("lane", IsClosingTag: true),
            ]
        },
    };

    [Theory]
    [MemberData(nameof(Data))]
    public void ParsedSyntax(string markup, TagExpectation[] tags)
    {
        var reader = new DocumentReader(markup);
        foreach (var tag in tags)
        {
            Assert.True(reader.NextTag());
            Assert.Equal(tag.Name, reader.Tag.Name.ToString());
            Assert.Equal(tag.IsClosingTag, reader.Tag.IsClosingTag);
            foreach (var attribute in tag.Attributes ?? [])
            {
                Assert.True(reader.NextAttribute());
                Assert.Equal(attribute.Name, reader.Attribute.Name.ToString());
                Assert.Equal(attribute.Value, reader.Attribute.Value.ToString());
                Assert.Equal(attribute.ValueType, reader.Attribute.ValueType);
            }
            Assert.False(reader.NextAttribute());
        }
        Assert.True(reader.Eof);
    }

    [Fact]
    public void TestDocumentParsing()
    {
        string markup =
            @"<lane orientation=""vertical"" align-content=""middle end"">
                <image width={{<ImageWidth}} sprite={{@Mods/focustense.StardewUITest/Sprites/Header}} />
                <label font=""dialogue"" text={{HeaderText}} />
                <checkbox is-checked={{<>Checked}}/>
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
                    attr => Assert.Equal(new("width", AttributeValueType.InputBinding, "ImageWidth"), attr),
                    attr => Assert.Equal(new("sprite", AttributeValueType.AssetBinding, "Mods/focustense.StardewUITest/Sprites/Header"), attr));
            },
            node =>
            {
                Assert.Equal("label", node.Tag);
                Assert.Collection(
                    node.Attributes,
                    attr => Assert.Equal(new("font", AttributeValueType.Literal, "dialogue"), attr),
                    attr => Assert.Equal(new("text", AttributeValueType.InputBinding, "HeaderText"), attr));
            },
            node =>
            {
                Assert.Equal("checkbox", node.Tag);
                Assert.Collection(
                    node.Attributes,
                    attr => Assert.Equal(new("is-checked", AttributeValueType.TwoWayBinding, "Checked"), attr));
            });
    }
}