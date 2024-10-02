using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;

namespace StarML.Tests;

public class ParserTests
{
    public record TagExpectation(string Name, SAttribute[]? Attributes = null, bool IsClosingTag = false);

    public static TheoryData<string, TagExpectation[]> Data =>
        new()
        {
            {
                @"<lane orientation=""vertical"" align-content=""middle end"">
                <image width={{<ImageWidth}} sprite={{@Mods/focustense.StardewUITest/Sprites/Header}} />
                <label font=""dialogue"" text={{HeaderText}} />
                <checkbox is-checked={{<>Checked}}/>
            </lane>",

                [
                    new("lane", [new("orientation", "vertical"), new("align-content", "middle end")]),
                    new(
                        "image",
                        [
                            new("width", "ImageWidth", ValueType: AttributeValueType.InputBinding),
                            new(
                                "sprite",
                                "Mods/focustense.StardewUITest/Sprites/Header",
                                ValueType: AttributeValueType.AssetBinding
                            ),
                        ]
                    ),
                    new("image", IsClosingTag: true),
                    new(
                        "label",
                        [new("font", "dialogue"), new("text", "HeaderText", ValueType: AttributeValueType.InputBinding)]
                    ),
                    new("label", IsClosingTag: true),
                    new("checkbox", [new("is-checked", "Checked", ValueType: AttributeValueType.TwoWayBinding)]),
                    new("checkbox", IsClosingTag: true),
                    new("lane", IsClosingTag: true),
                ]
            },
            {
                @"<label font=""small"" *repeat={{<>Items}} text={{DisplayName}} />",

                [
                    new(
                        "label",
                        [
                            new("font", "small"),
                            new("repeat", "Items", AttributeType.Structural, AttributeValueType.TwoWayBinding),
                            new("text", "DisplayName", ValueType: AttributeValueType.InputBinding),
                        ]
                    ),
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
            attr => Assert.Equal(new("orientation", "vertical"), attr),
            attr => Assert.Equal(new("align-content", "middle end"), attr)
        );
        Assert.Collection(
            document.Root.ChildNodes,
            node =>
            {
                Assert.Equal("image", node.Tag);
                Assert.Collection(
                    node.Attributes,
                    attr => Assert.Equal(new("width", "ImageWidth", ValueType: AttributeValueType.InputBinding), attr),
                    attr =>
                        Assert.Equal(
                            new(
                                "sprite",
                                "Mods/focustense.StardewUITest/Sprites/Header",
                                ValueType: AttributeValueType.AssetBinding
                            ),
                            attr
                        )
                );
            },
            node =>
            {
                Assert.Equal("label", node.Tag);
                Assert.Collection(
                    node.Attributes,
                    attr => Assert.Equal(new("font", "dialogue"), attr),
                    attr => Assert.Equal(new("text", "HeaderText", ValueType: AttributeValueType.InputBinding), attr)
                );
            },
            node =>
            {
                Assert.Equal("checkbox", node.Tag);
                Assert.Collection(
                    node.Attributes,
                    attr =>
                        Assert.Equal(new("is-checked", "Checked", ValueType: AttributeValueType.TwoWayBinding), attr)
                );
            }
        );
    }
}
