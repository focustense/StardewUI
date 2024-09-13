using StardewModdingAPI.Utilities;

namespace StardewUI.Widgets.Keybinding;

/// <summary>
/// Overlay control for editing a keybinding, or list of bindings.
/// </summary>
/// <param name="spriteMap">Map of bindable buttons to sprite representations.</param>
/// <param name="emptyText">Text to display when the keybind is empty (has no buttons).</param>
public class KeybindOverlay(ISpriteMap<SButton> spriteMap, string emptyText) : FullScreenOverlay
{
    /// <summary>
    /// The current keybinds to display in the list.
    /// </summary>
    public KeybindList KeybindList
    {
        get => keybindList;
        set
        {
            if (value == keybindList)
            {
                return;
            }
            keybindList = value;
            UpdateKeybinds();
        }
    }

    private readonly Image horizontalDivider =
        new()
        {
            Layout = LayoutParameters.AutoRow(),
            Margin = new(0, 16),
            Fit = ImageFit.Stretch,
            Sprite = UiSprites.GenericHorizontalDivider,
        };

    private KeybindList keybindList = new();

    // Initialized in CreateView
    private KeybindView currentKeybindView = null!;
    private Lane keybindsLane = null!;

    protected override IView CreateView()
    {
        currentKeybindView = new(spriteMap, "");
        var currentKeybindFrame = new Frame()
        {
            Layout = LayoutParameters.FitContent(),
            Content = currentKeybindView,
            IsFocusable = true,
        };
        keybindsLane = new() { Layout = LayoutParameters.AutoRow(), Orientation = Orientation.Vertical };
        UpdateKeybinds();
        return new Frame()
        {
            Layout = new() { Width = Length.Px(640), Height = Length.Content() },
            Border = UiSprites.ControlBorder,
            Padding = UiSprites.ControlBorder.FixedEdges! + new Edges(8),
            Content = new Lane()
            {
                Layout = LayoutParameters.AutoRow(),
                Orientation = Orientation.Vertical,
                Children = [currentKeybindFrame, horizontalDivider, keybindsLane],
            },
        };
    }

    private void AddKeybindRow(Keybind keybind)
    {
        var keybindView = new KeybindView(spriteMap, emptyText)
        {
            Layout = LayoutParameters.AutoRow(),
            Margin = keybindsLane.Children.Count > 0 ? new(Top: 16) : Edges.NONE,
            Keybind = keybind,
        };
        var deleteButton = new Image()
        {
            Layout = new() { Width = Length.Content(), Height = Length.Px(40) },
            Sprite = UiSprites.SmallTrashCan,
            ShadowAlpha = 0.35f,
            ShadowOffset = new(-3, 4),
            IsFocusable = true,
        };
        var row = new Lane()
        {
            Layout = LayoutParameters.AutoRow(),
            VerticalContentAlignment = Alignment.Middle,
            Children = [keybindView, new Spacer() { Layout = LayoutParameters.AutoRow() }, deleteButton],
        };
        keybindsLane.Children.Add(row);
    }

    private void UpdateKeybinds()
    {
        if (keybindsLane is null)
        {
            return;
        }
        keybindsLane.Children.Clear();
        foreach (var keybind in KeybindList.Keybinds)
        {
            if (keybind.IsBound)
            {
                AddKeybindRow(keybind);
            }
        }
    }
}
