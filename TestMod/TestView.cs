using Microsoft.Xna.Framework;
using StardewUI;
using StardewValley;

namespace StardewUITest;

internal class TestView : WrapperView
{
    protected override IView CreateView()
    {
        Sprite questionIcon = new(Game1.mouseCursors, new Rectangle(240, 192, 16, 16));
        return new ScrollableView()
        {
            Layout = LayoutParameters.FixedSize(400, 400),
            Content = new Grid()
            {
                Children = Enumerable.Range(0, 200).Select((i) =>
                {
                    return new Frame()
                    {
                        Content = new Image()
                        {
                            Name = i.ToString(),
                            Layout = LayoutParameters.FixedSize(64, 64),
                            Sprite = questionIcon,
                            Focusable = false
                        },
                        Focusable = true
                    };
                }).ToList<IView>(),
            }
        };
    }
}
