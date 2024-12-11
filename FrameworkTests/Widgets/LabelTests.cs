using Microsoft.Xna.Framework;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewUI.Widgets;
using StardewValley;

namespace StardewUI.Framework.Tests.Widgets;

public class LabelTests
{
    [Fact]
    public void WhenTextFitsOnSingleLine_DoesNotAppendEllipsis()
    {
        var label = new Label()
        {
            Layout = LayoutParameters.FixedSize(200, 200),
            Font = Game1.smallFont,
            Color = Color.Black,
            MaxLines = 1,
            Text = "Bacon ipsum",
        };

        var spriteBatch = new FakeSpriteBatch();
        label.Measure(new(1000, 1000));
        label.Draw(spriteBatch);

        Assert.Equal(
            [
                .. ViewBoilerplate.ZeroMarginAndBorderStart,
                new DrawStringInfo(Game1.smallFont, "Bacon ipsum", new(0, 0), Color.Black),
                .. ViewBoilerplate.ZeroMarginAndBorderEnd,
            ],
            spriteBatch.History
        );
    }

    [Fact]
    public void WhenTextFitsOnMultipleLine_DoesNotAppendEllipsis()
    {
        var label = new Label()
        {
            Layout = LayoutParameters.FixedSize(450, 200),
            Font = Game1.smallFont,
            Color = Color.Black,
            MaxLines = 2,
            Text = "Bacon ipsum dolor amet incididunt shank rump hamburger elit",
        };

        var spriteBatch = new FakeSpriteBatch();
        label.Measure(new(1000, 1000));
        label.Draw(spriteBatch);

        Assert.Equal(
            [
                .. ViewBoilerplate.ZeroMarginAndBorderStart,
                new DrawStringInfo(Game1.smallFont, "Bacon ipsum dolor amet incididunt", new(0, 0), Color.Black),
                new DrawStringInfo(Game1.smallFont, "shank rump hamburger elit", new(0, 28), Color.Black),
                .. ViewBoilerplate.ZeroMarginAndBorderEnd,
            ],
            spriteBatch.History
        );
    }

    [Fact]
    public void WhenTextIsLongParagraphWithSpaces_AppliesWordBreaking()
    {
        var label = new Label()
        {
            Layout = LayoutParameters.FixedSize(440, 200),
            Font = Game1.smallFont,
            Color = Color.Black,
            MaxLines = 4,
            Text = """
                Bacon ipsum dolor amet incididunt shank rump hamburger elit, est exercitation consequat sint ea duis.
                Culpa adipisicing proident hamburger kielbasa, minim est eu. Pancetta ham beef ribs, sirloin cupidatat
                incididunt duis labore magna pastrami.
                """.Replace(Environment.NewLine, " "),
        };

        var spriteBatch = new FakeSpriteBatch();
        label.Measure(new(1000, 1000));
        label.Draw(spriteBatch);

        Assert.Equal(
            [
                .. ViewBoilerplate.ZeroMarginAndBorderStart,
                new DrawStringInfo(Game1.smallFont, "Bacon ipsum dolor amet incididunt", new(0, 0), Color.Black),
                new DrawStringInfo(Game1.smallFont, "shank rump hamburger elit, est", new(0, 28), Color.Black),
                new DrawStringInfo(Game1.smallFont, "exercitation consequat sint ea duis.", new(0, 56), Color.Black),
                new DrawStringInfo(Game1.smallFont, "Culpa adipisicing proident ...", new(0, 84), Color.Black),
                .. ViewBoilerplate.ZeroMarginAndBorderEnd,
            ],
            spriteBatch.History
        );
    }

    [Fact]
    public void WhenLatinTextHasWordLongerThanSingleLine_AppliesCharacterBreakingWithHyphenation()
    {
        var label = new Label()
        {
            Layout = LayoutParameters.FixedSize(440, 200),
            Font = Game1.smallFont,
            Color = Color.Black,
            MaxLines = 4,
            Text = """
                Bacon ipsum dolor amet incididunt shank rump hamburger pneumonoultramicroscopicsilicovolcanoconiosis.
                Culpa adipisicing proident hamburger kielbasa, minim est eu. Pancetta ham beef ribs, sirloin cupidatat
                incididunt duis labore magna pastrami.
                """.Replace(Environment.NewLine, " "),
        };

        var spriteBatch = new FakeSpriteBatch();
        label.Measure(new(1000, 1000));
        label.Draw(spriteBatch);

        Assert.Equal(
            [
                .. ViewBoilerplate.ZeroMarginAndBorderStart,
                new DrawStringInfo(Game1.smallFont, "Bacon ipsum dolor amet incididunt", new(0, 0), Color.Black),
                new DrawStringInfo(Game1.smallFont, "shank rump hamburger", new(0, 28), Color.Black),
                new DrawStringInfo(Game1.smallFont, "pneumonoultramicroscopicsilicovolc-", new(0, 56), Color.Black),
                new DrawStringInfo(Game1.smallFont, "anoconiosis. Culpa adipisicing ...", new(0, 84), Color.Black),
                .. ViewBoilerplate.ZeroMarginAndBorderEnd,
            ],
            spriteBatch.History
        );
    }

    [Fact]
    public void WhenLatinTextHasWordLongerThanMultipleLines_AppliesCharacterBreakingWithHyphenation()
    {
        var label = new Label()
        {
            Layout = LayoutParameters.FixedSize(140, 200),
            Font = Game1.smallFont,
            Color = Color.Black,
            MaxLines = 4,
            Text = "Pneumonoultramicroscopicsilicovolcanoconiosis",
        };

        var spriteBatch = new FakeSpriteBatch();
        label.Measure(new(1000, 1000));
        label.Draw(spriteBatch);

        Assert.Equal(
            [
                .. ViewBoilerplate.ZeroMarginAndBorderStart,
                new DrawStringInfo(Game1.smallFont, "Pneumono-", new(0, 0), Color.Black),
                new DrawStringInfo(Game1.smallFont, "ultramicro-", new(0, 28), Color.Black),
                new DrawStringInfo(Game1.smallFont, "scopicsilico-", new(0, 56), Color.Black),
                new DrawStringInfo(Game1.smallFont, "volcanocon ...", new(0, 84), Color.Black),
                .. ViewBoilerplate.ZeroMarginAndBorderEnd,
            ],
            spriteBatch.History
        );
    }

    [Fact]
    public void WhenCjkTextHasWordLongerThanMultipleLines_AppliesCharacterBreakingWithoutHyphenation()
    {
        var label = new Label()
        {
            Layout = LayoutParameters.FixedSize(360, 200),
            Font = Game1.smallFont,
            Color = Color.Black,
            MaxLines = 4,
            Text = """
                哦，公爵，热那亚和卢卡，如今成了波拿巴家的领地了。我可要把话说在前面，您要是不承认我们在打仗，您要是再敢替这个基督的敌人
                （是的，我认为他是基督的敌人）的种种罪孽和暴行辩护，我就同您绝交，您就不再是我的朋友，也不再像您自称的那样，
                是我忠实的奴仆。哦，您好，您好！我知道我把您吓坏了，请坐，坐下来谈吧。
                """.Replace(Environment.NewLine, ""),
        };

        var spriteBatch = new FakeSpriteBatch();
        label.Measure(new(1000, 1000));
        label.Draw(spriteBatch);

        Assert.Equal(
            [
                .. ViewBoilerplate.ZeroMarginAndBorderStart,

                // csharpier-ignore-start
                new DrawStringInfo(Game1.smallFont, "哦，公爵，热那亚和卢卡，如今成了波拿巴家的领地", new(0, 0), Color.Black),
                new DrawStringInfo(Game1.smallFont, "了。我可要把话说在前面，您要是不承认我们在打", new(0, 28), Color.Black),
                new DrawStringInfo(Game1.smallFont, "仗，您要是再敢替这个基督的敌人（是的，我认为他", new(0, 56), Color.Black),
                new DrawStringInfo(Game1.smallFont, "是基督的敌人）的种种罪孽和暴行辩护，我就同您绝 ...", new(0, 84), Color.Black),
                // csharpier-ignore-end
                .. ViewBoilerplate.ZeroMarginAndBorderEnd,
            ],
            spriteBatch.History
        );
    }

    static class ViewBoilerplate
    {
        public static readonly IReadOnlyList<ISpriteBatchLogEntry> ZeroMarginAndBorderEnd =
        [
            new RevertInfo(new SaveTransformInfo()),
        ];

        public static readonly IReadOnlyList<ISpriteBatchLogEntry> ZeroMarginAndBorderStart =
        [
            new TransformInfo(Transform.FromTranslation(Vector2.Zero)),
            new SaveTransformInfo(),
            new TransformInfo(Transform.FromTranslation(Vector2.Zero)),
        ];
    }
}
