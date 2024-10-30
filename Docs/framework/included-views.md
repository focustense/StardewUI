# Included Views

StardewUI enables you to use any view within another view. This can be useful in a number of situations, for example:

- Creating reusable "building blocks" to use in multiple views;
- Simplifying a large or complex view that is becoming difficult to maintain;
- Implementing a dynamic template system, in similar fashion to the WPF [DataTemplateSelector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.datatemplateselector?view=windowsdesktop-8.0&redirectedfrom=MSDN);
- Enabling other mods to more easily or safely customize your own UI.

All of these are supported by the `<include>` tag, which is a special kind of tag that does not have its own view; instead, it behaves similarly to a [structural attribute](starml.md#structural-attributes).

## Usage

```html
<include name="..." />
```

Include tags are are written in [StarML](starml.md) like any other tags, but don't use the [common attributes](starml.md#common-attributes). Instead, the attributes supported are:

- `name`, which holds the [asset name](../getting-started/adding-ui-assets.md#adding-views) of the view to include.
    - This attribute is **required** and is the same name you'd provide [to the API](../getting-started/displaying-ui.md#menus).
    - Supports either literal names, like `"Mods/MyMod/Views/MyView"`, or data bindings, such as `{ViewName}`.
    - Although views are assets, data bindings on the name of an `<include>` element should _not_ include a `@` prefix, since the attribute is already understood to be the _view name_ and not the view itself.
- All [structural attributes](starml.md#structural-attributes) that could apply to any other tag.
    - `*context` is especially useful for included views, since a reusable view will generally have assumptions about the type of data it receives.

!!! example "Minimal Example"

    === "Data.cs"
    
        ```cs
        public class OuterViewModel
        {
            public InnerViewModel Inner { get; set; }
        }

        public class InnerViewModel
        {
            public string Text { get; set; }
        }
        ```

    === "OuterView.sml"

        ```html
        <include name="Mods/MyMod/Views/InnerView" *context={Inner} />
        ```

    === "InnerView.sml"

        ```html
        <label text={Text} />
        ```

## Detailed Example

Consider a UI similar to Stardew's main menu, with many different tabs that all perform entirely different functions. While this won't be a guide to patching the actual, vanilla main menu (StardewUI is indifferent to vanilla UI, and seamless integration is an explicit non-goal of the project), we will show that not only is it easy to build a similar UI, but it is easy to build it in such a way that it is _extensible_, with straightforward hooks for other mods to add their own pages.

This example is aimed at demonstrating how to use includes, so it will intentionally omit many of the visual styles, borders, backgrounds, etc. that are associated with tabbed UI, but there are other [examples](../examples/index.md) of how to create the visual look and feel. For conciseness, default values and other validation concerns are also omitted.

Note: This uses [PropertyChanged.SourceGenerator](https://github.com/canton7/PropertyChanged.SourceGenerator) to provide change notifications, as described in the [Binding Context](binding-context.md) guide.

=== "ModEntry.cs"

    ```cs
    public class ModEntry : Mod
    {
        // Preconditions and setup omitted; trigger the menu however you like.
        
        private IApi api;
        private ModConfig config; // E.g. loaded via Helper.ReadConfig
        private ModData data; // Can be any data/type, this is only an example.
        private IViewEngine viewEngine;
        
        public override object? GetApi()
        {
            return api;
        }
        
        public void ShowMenu()
        {
            var menuViewModel = new MenuViewModel([
                new()
                {
                    Title = I18n.HomeTabTitle(),
                    Icon = Tuple.Create(...),
                    ContentViewName = "Mods/MyMod/Views/HomeMenuPage",
                    ContentData = data
                },
                new()
                {
                    Title = I18n.SettingsTabTitle(),
                    Icon = Tuple.Create(...),
                    ContentViewName = "Mods/MyMod/Views/SettingsMenuPage",
                    ContentData = config
                },
                .. api.GetCustomPages()
            ]);
            Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset(
                "Mods/MyMod/Views/MainMenu",
                menuViewModel
            );
        }
    }
    ```

=== "Api.cs"

    ```cs
    public interface IApi
    {
        void AddCustomPage(
            Func<string> title,
            Func<Texture2D, Rectangle> iconSelector,
            Func<object> contentDataSelector,
            string contentViewName);
    }

    public class Api : IApi
    {
        private readonly List<Func<MenuPageViewModel>> customPages = [];
        
        public void AddCustomPage(
            Func<string> title,
            Func<Texture2D, Rectangle> iconSelector,
            Func<object> contentDataSelector,
            string contentViewName)
        {
            CustomPages.Add(() => new MenuPageViewModel()
            {
                Title = title(),
                Icon = iconSelector(),
                ContentData = contentDataSelector(),
                ContentViewName = contentViewName,
            });
        }
        
        internal IEnumerable<MenuPageViewModel> GetCustomPages()
        {
            return customPages.Select(page => page());
        }
    }
    ```

=== "MenuViewModel.cs"

    ```cs
    public partial class MenuViewModel
    {
        public IReadOnlyList<MenuPageViewModel> Pages;

        [Notify] private MenuPageViewModel currentPage;
        
        public MenuViewModel(IReadOnlyList<MenuPageViewModel> pages)
        {
            Pages = pages;
            currentPage = pages[0];
            currentPage.IsSelected = true;
        }
        
        public void SelectPage(MenuPageViewModel page)
        {
            if (page == currentPage)
            {
                return;
            }
            currentPage.IsSelected = false;
            CurrentPage = page;
            page.IsSelected = true;
        }
    }
    
    public partial class MenuPageViewModel
    {
        public string Title { get; set; }
        public Tuple<Texture2D, Rectangle> Icon { get; set; }
        public object ContentData { get; set; }
        public string ContentViewName { get; set; }

        // Not used in this example, but in a real tab implementation, this property
        // would be used to style the selected tab differently.
        [Notify] private bool isSelected;
    }
    ```

=== "ModMenu.sml"

    ```html
    <lane orientation="vertical">
        <lane>
            <frame *repeat={Pages}
                   padding="8px"
                   background={@Mods/MyMod/Sprites/UI:TabBackground}
                   tooltip={Title}
                   focusable="true"
                   click=|SelectPage(this)|>
                <image layout="32px 32px" sprite={Icon} />
            </frame>
        <lane>
        <frame *context={CurrentPage}
               background={@Mods/StardewUI/Sprites/ControlBorder}>
            <include *context={ContentData} name={ContentViewName} />
        </frame>
    </lane>
    ```

This mod creates a menu with two pages by default, Home and Settings. Other mods can register any pages they want, using any view and any data they want, using `IApi.AddCustomPage`, and those pages will automatically be integrated into the menu. They will have a corresponding tab, and show their corresponding view when the tab is clicked; they will have intrinsically correct focus (no need for Harmony patching to set up neighbors, etc.) and, with minor changes to the API, could even be sorted or added in arbitrary positions.

While a lot of specifics, such as texture/sprite references, are excluded from the example in order to highlight the important parts—it doesn't matter what specifically is in the `HomeMenuPage` or `SettingsMenuPage`, only that they are controlled by the tabs—this really is the _entire_ code for both the main-menu UI and the API.

You might not want your mod's UI to be this "open", but the external API is optional, and could be either removed entirely or replaced with more constrained types such as `Func<IMenuPage>` instead of `Func<object>` where `IMenuPage` is something you define—for example, the data for a category of items, or a single game location.

`<include>` is here to offer an extra level of customizability. Using it, you can change from a single view using [data binding](../concepts.md#data-binding) to a completely dynamic view whose layout is undecided until it is time to display it.