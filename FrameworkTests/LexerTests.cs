using StardewUI.Framework.Grammar;

namespace StarML.Tests;

public class LexerTests
{
    // Actual tokens are ref struct, so we can't use them in TheoryData.
    public record TestableToken(TokenType Type, string Text)
    {
        public static implicit operator TestableToken(Token token)
        {
            return new(token.Type, token.Text.ToString());
        }
    }

    public static TheoryData<string, TestableToken[]> Data =>
        new()
        {
            {
                @"<lane orientation=""vertical"" align-content=""middle end"">
                    <image width={{<ImageWidth}} sprite={{@Mods/focustense.StardewUITest/Sprites/Header}} />
                    <label font=""dialogue"" text={{HeaderText}} />
                    <checkbox is-checked={{<>Checked}}/>
                </lane>",

                [
                    new(TokenType.OpeningTagStart, "<"),
                    new(TokenType.Name, "lane"),
                    new(TokenType.Name, "orientation"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.Quote, "\""),
                    new(TokenType.Literal, "vertical"),
                    new(TokenType.Quote, "\""),
                    new(TokenType.Name, "align-content"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.Quote, "\""),
                    new(TokenType.Literal, "middle end"),
                    new(TokenType.Quote, "\""),
                    new(TokenType.TagEnd, ">"),
                    new(TokenType.OpeningTagStart, "<"),
                    new(TokenType.Name, "image"),
                    new(TokenType.Name, "width"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.BindingStart, "{{"),
                    new(TokenType.BindingModifier, "<"),
                    new(TokenType.Literal, "ImageWidth"),
                    new(TokenType.BindingEnd, "}}"),
                    new(TokenType.Name, "sprite"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.BindingStart, "{{"),
                    new(TokenType.BindingModifier, "@"),
                    new(TokenType.Literal, "Mods/focustense.StardewUITest/Sprites/Header"),
                    new(TokenType.BindingEnd, "}}"),
                    new(TokenType.SelfClosingTagEnd, "/>"),
                    new(TokenType.OpeningTagStart, "<"),
                    new(TokenType.Name, "label"),
                    new(TokenType.Name, "font"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.Quote, "\""),
                    new(TokenType.Literal, "dialogue"),
                    new(TokenType.Quote, "\""),
                    new(TokenType.Name, "text"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.BindingStart, "{{"),
                    new(TokenType.Literal, "HeaderText"),
                    new(TokenType.BindingEnd, "}}"),
                    new(TokenType.SelfClosingTagEnd, "/>"),
                    new(TokenType.OpeningTagStart, "<"),
                    new(TokenType.Name, "checkbox"),
                    new(TokenType.Name, "is-checked"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.BindingStart, "{{"),
                    new(TokenType.BindingModifier, "<>"),
                    new(TokenType.Literal, "Checked"),
                    new(TokenType.BindingEnd, "}}"),
                    new(TokenType.SelfClosingTagEnd, "/>"),
                    new(TokenType.ClosingTagStart, "</"),
                    new(TokenType.Name, "lane"),
                    new(TokenType.TagEnd, ">"),
                ]
            },
            {
                @"<label font=""small"" *repeat={{<Items}} text={{DisplayName}} />",

                [
                    new(TokenType.OpeningTagStart, "<"),
                    new(TokenType.Name, "label"),
                    new(TokenType.Name, "font"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.Quote, "\""),
                    new(TokenType.Literal, "small"),
                    new(TokenType.Quote, "\""),
                    new(TokenType.AttributeModifier, "*"),
                    new(TokenType.Name, "repeat"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.BindingStart, "{{"),
                    new(TokenType.BindingModifier, "<"),
                    new(TokenType.Literal, "Items"),
                    new(TokenType.BindingEnd, "}}"),
                    new(TokenType.Name, "text"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.BindingStart, "{{"),
                    new(TokenType.Literal, "DisplayName"),
                    new(TokenType.BindingEnd, "}}"),
                    new(TokenType.SelfClosingTagEnd, "/>"),
                ]
            },
            {
                @"<textinput text={{<>^Name}} />",

                [
                    new(TokenType.OpeningTagStart, "<"),
                    new(TokenType.Name, "textinput"),
                    new(TokenType.Name, "text"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.BindingStart, "{{"),
                    new(TokenType.BindingModifier, "<>"),
                    new(TokenType.BindingParent, "^"),
                    new(TokenType.Literal, "Name"),
                    new(TokenType.BindingEnd, "}}"),
                    new(TokenType.SelfClosingTagEnd, "/>"),
                ]
            },
            {
                @"<label text={{<^^^Name}} />",

                [
                    new(TokenType.OpeningTagStart, "<"),
                    new(TokenType.Name, "label"),
                    new(TokenType.Name, "text"),
                    new(TokenType.Assignment, "="),
                    new(TokenType.BindingStart, "{{"),
                    new(TokenType.BindingModifier, "<"),
                    new(TokenType.BindingParent, "^"),
                    new(TokenType.BindingParent, "^"),
                    new(TokenType.BindingParent, "^"),
                    new(TokenType.Literal, "Name"),
                    new(TokenType.BindingEnd, "}}"),
                    new(TokenType.SelfClosingTagEnd, "/>"),
                ]
            },
        };

    [Theory]
    [MemberData(nameof(Data))]
    public void LexerTokens(string markup, TestableToken[] expectedTokens)
    {
        var lexer = new Lexer(markup);
        foreach (var token in expectedTokens)
        {
            Assert.True(lexer.MoveNext());
            Assert.Equal(token.Type, lexer.Current.Type);
            Assert.Equal(token.Text, lexer.Current.Text.ToString());
        }
        Assert.True(lexer.Eof);
    }
}