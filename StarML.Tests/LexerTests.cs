using StardewUITest.StarML;
using Xunit.Abstractions;

namespace StarML.Tests;

public class LexerTests(ITestOutputHelper output)
{
    [Fact]
    public void TestLexerBasic()
    {
        string markup =
            @"<lane orientation=""vertical"" align-content=""middle end"">
                <image width=""400"" sprite={{@Mods/focustense.StardewUITest/Sprites/Header}} />
                <label font=""dialogue"" text={{HeaderText}} />
            </lane>";

        var lexer = new Lexer(markup);
        foreach (var token in lexer)
        {
            output.WriteLine($"[{token.Type}] {token.Text}");
        }
    }
}