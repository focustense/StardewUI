using StardewUI;

namespace StardewUITest;

internal class TestMenu : ViewMenu<TestView>
{
    protected override TestView CreateView()
    {
        return new TestView();
    }
}
