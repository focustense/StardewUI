using System.ComponentModel;
using Microsoft.Xna.Framework;
using PropertyChanged.SourceGenerator;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace StardewUITest.Examples;

internal partial class FormViewModel : INotifyPropertyChanged
{
    public Color BackgroundTint => SelectedTheme.Color;

    public Func<float, string> FormatOpacity { get; } = v => v.ToString("f2");

    public PetType[] PetTypes { get; } = Enum.GetValues<PetType>();

    public ThemeViewModel[] Themes { get; } =
        [
            new(Color.SandyBrown) { Selected = true },
            new(Color.SkyBlue),
            new(Color.SeaGreen),
            new(Color.Violet),
            new(Color.LightGoldenrodYellow),
            new(Color.LightSlateGray),
        ];

    public TomatoType[] TomatoTypes { get; } = Enum.GetValues<TomatoType>();

    [Notify]
    private bool allowClose = true;

    [Notify]
    private KeybindList keybinds = new(new Keybind(SButton.LeftControl, SButton.F7));

    [Notify]
    private string name = "Cletus";

    [Notify]
    private float opacity = 1f;

    [Notify]
    private PetType selectedPetType;

    [Notify]
    private ThemeViewModel selectedTheme;

    [Notify]
    private TomatoType selectedTomatoType;

    public FormViewModel()
    {
        selectedTheme = Themes[0];
    }

    public void Close(bool save)
    {
        // Save is unused in the example, but in a real mod we'd use it to save mod settings/data.
        var menu = Game1.activeClickableMenu;
        while (menu.GetChildMenu() is IClickableMenu childMenu)
        {
            menu = childMenu;
        }
        if (!menu.readyToClose())
        {
            return;
        }
        Game1.playSound("bigDeSelect");
        if (menu.GetParentMenu() is IClickableMenu parentMenu)
        {
            parentMenu.SetChildMenu(null);
        }
        else
        {
            Game1.exitActiveMenu();
        }
    }

    public void SetTheme(ThemeViewModel theme)
    {
        if (SelectedTheme == theme)
        {
            return;
        }
        Game1.playSound("smallSelect");
        SelectedTheme.Selected = false;
        SelectedTheme = theme;
        theme.Selected = true;
    }
}

internal enum PetType
{
    Dog,
    Cat,
    Turtle,
    Parrot,
    Snake,
}

internal enum TomatoType
{
    Fruit,
    Vegetable,
    Condiment,
}

internal partial class ThemeViewModel(Color color)
{
    public Color Color { get; } = color;

    [Notify]
    private bool selected;
}
