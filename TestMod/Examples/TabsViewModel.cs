using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PropertyChanged.SourceGenerator;

namespace StardewUITest.Examples;

internal partial class TabData(string name, Texture2D texture, Rectangle sourceRect) : INotifyPropertyChanged
{
    public string Name { get; } = name;
    public Tuple<Texture2D, Rectangle> Sprite { get; } = Tuple.Create(texture, sourceRect);

    [Notify]
    private bool active;
}

internal partial class TabsViewModel
{
    public IReadOnlyList<TabData> Tabs { get; set; } = [];

    public void OnTabActivated(string name)
    {
        foreach (var tab in Tabs)
        {
            if (tab.Name != name)
            {
                tab.Active = false;
            }
        }
    }
}
