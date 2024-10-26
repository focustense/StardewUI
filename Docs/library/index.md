# Core Library

StardewUI's Core Library is all of the mundane, engine-room components and functionality running underneath the magic of the [framework](../framework/index.md).

When you [display a view](../getting-started/displaying-ui.md) written with [StarML](../framework/starml.md), the framework does not translate that directly to pixels on the screen; instead, it is transformed into a _[view tree](../concepts.md#layout-views-and-view-trees)_—a hierarchy of layout views and content views, which handle everything that _isn't_ related to [data binding](../concepts.md#data-binding).

StardewUI [Views](../concepts.md#views) are identical in concept to [Qt widgets](https://doc.qt.io/qt-6/qtwidgets-index.html), [Android views](https://developer.android.com/reference/android/view/View) or [HTML elements](https://developer.mozilla.org/en-US/docs/Web/API/Element); they are responsible for performing [layout](../concepts.md#layout), handling [user interaction](../framework/focus-and-interaction.md) and ultimately drawing themselves to the screen, or [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) as is used in Stardew Valley.

## Features

Core library features are loosely divided into the same categories as code namespaces:

- [**Graphics**](../reference/stardewui/graphics/index.md): Abstractions for game sprites, the MonoGame [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html), and everything else that deals directly with "pixels on the screen".
- [**Input**](../reference/stardewui/input/index.md): A variety of useful tools for dealing with user input (text entry, button repeat/throttling, etc.) that Stardew Valley and SMAPI don't provide themselves, or hide behind implementation details.
- [**Layout**](../reference/stardewui/layout/index.md): Essential types and tools for describing layouts: positions, alignments and other dimensions.
- [**Widgets**](../reference/stardewui/widgets/index.md): All the [standard views](standard-views.md).
- [**Overlays**](../reference/stardewui/overlays/index.md): System for displaying views as either dismissable overlays, such as [dropdowns](standard-views.md#drop-down-list), or full-screen ("modal dialog") style, such as the [keybinding editor](standard-views.md#key-binding-editor).
- [**Animation**](../reference/stardewui/animation/index.md): Animates properties of views. Includes the common [HoverScale](../reference/stardewui/animation/hoverscale.md) that can be attached to any image to make it zoom on mouseover.
- [**Diagnostics**](../reference/stardewui/diagnostics/index.md): Primarily [performance tracing](../performance.md).

## Usage

When you use the UI Framework, the Core Library is designed to work behind the scenes. However, there are some cases when you might need to use it directly, e.g. if you are writing [extensions](../framework/extensions.md).

Unless you are specifically opting out of all framework features, the correct way to reference the Core Library is via SMAPI's [ModBuildConfig](https://www.nuget.org/packages/Pathoschild.Stardew.ModBuildConfig), which most mods should normally already be using; the reference can be added either directly, or indirectly via [ModManifestBuilder](https://www.nuget.org/packages/Leclair.Stardew.ModManifestBuilder).

### Using `ModManifestBuilder`

Simply add the reference to your `.csproj` file using [`SMAPIDependency`](https://github.com/KhloeLeclair/Stardew-ModManifestBuilder?tab=readme-ov-file#smapidependency-), and set the attribute `Reference="true"`.

!!! example

    ```xml
    <ItemGroup>
        <SMAPIDependency Include="focustense.StardewUI" Version="{{{ release.version }}}" Reference="true" />
    </ItemGroup>

    <ItemGroup>
        <PackageReference Include="Pathoschild.Stardew.ModBuildConfig" Version="4.1.1" />
        <PackageReference Include="Leclair.Stardew.ModManifestBuilder" Version="2.3.1" />
    </ItemGroup>
    ```

### Using ModBuildConfig

SMAPI defines a `GameModsPath` variable which you can use to reference the StardewUI assembly relative to the game path, instead of a physical directory on your machine:

!!! example

    ```xml
    <ItemGroup>
        <Reference Include="StardewUI">
            <HintPath>$(GameModsPath)\StardewUI\StardewUI.dll</HintPath>
            <Private>false</Private>
        </Reference>
    </ItemGroup>

    <ItemGroup>
        <PackageReference Include="Pathoschild.Stardew.ModBuildConfig" Version="4.1.1" />
    </ItemGroup>
    ```

Note that when you add a direct assembly reference—as opposed to the more common [API integration](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Integrations#Using_an_API) method—it is **extremely important** to also add the mod as a [required dependency](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Manifest#Dependencies) in your `manifest.json`, otherwise SMAPI may not load the mods in the correct order, resulting in your mod failing to load, and this failure may be intermittent or platform-specific, e.g. loads correctly on Windows but not Linux or vice versa.

!!! example

    ```json
    {
        "Name": "ModName",
        // Other fields omitted from example: Author, Version, Description, UniqueID, etc.
        "Dependencies": [
           {
              "UniqueID": "focustense.StardewUI",
              "MinimumVersion": "{{{ release.version }}}"
           }
        ]
    }
    ```

These instructions only apply to the `ModBuildConfig` method, because `ModManifestBuilder` does it automatically for you.

Although `MinimumVersion` is optional for manifest dependencies, you should always include it for any StardewUI versions prior to 1.0. StardewUI follows [semantic versioning](https://semver.org/) and any versions prior to 1.0 are considered development versions that may introduce breaking changes.

### Using Submodule/Shared Project

!!! danger

    Submodule/shared project imports are **only** for users who are not interested in _any_ of the [framework](../framework/index.md) features, and need a completely standalone solution for their project.
    
    Visual Studio Shared Projects are a 10-year-old technology that has never been clearly documented; the best resources available are a few [obsolete documentation pages](https://learn.microsoft.com/en-us/previous-versions/xamarin/cross-platform/app-fundamentals/shared-projects?tabs=windows#what-is-a-shared-project) and [blog posts](https://dailydotnettips.com/using-shared-project-across-multiple-applications-in-visual-studio-2015/). They do work, and are fairly common to see in Stardew Modding [Monorepos](https://en.wikipedia.org/wiki/Monorepo), but have many quirks, tend to behave oddly and in frustrating ways within the IDE, and most importantly, are compiled as _your own_ source code, meaning they generate **new types** inside your mod and are therefore **not compatible** with the types inside `StardewUI.dll`. If you try to use them together, you will run into cryptic errors like `No value converter registered for Edges -> Edges`.
    
    Git submodules (AKA "sob modules") have many problems of their own and are [widely hated](https://diziet.dreamwidth.org/14666.html) for many good reasons. An alternative is [git subtree](https://git-memo.readthedocs.io/en/latest/subtree.html), which solves some but not all of the problems of submodules, and solves _none_ of the problems of Shared Projects.
    
    Consider carefully before choosing this option, because it may complicate your project maintenance and is effectively exclusive with the Framework; you can only use one or the other, and switching to the Framework later on may be difficult if there have been breaking changes, or if you depend on any internal types.

If you have elected to use the Core Library Shared Project (_please read the warnings above_) then use the following process for including it:

1. `cd` into your mod's _solution_ directory.  
    (This is the directory containing the `.sln` file, _not_ the `.csproj`)
2. Add the submodule: `git submodule add https://github.com/focustense/StardewUI.git`
3. Add the shared project to your solution. In Visual Studio, right-click on the Solution in the Solution Explorer, then **Add** -> **Existing Project...**, find and select the `StardewUI.shproj` inside the `StardewUI\Core` folder created in step 1.
4. Add a project reference to your mod. In Visual Studio, right-click on on your Project (_not_ the solution, and _not_ the StardewUI project; the project for _your mod_), then **Add** -> **Shared Project Reference**, and tick `StardewUI`. If you don't see `StardewUI` in the list, then you made a mistake in one of the previous steps.
5. Double-check; if you open your `.csproj` file in VS or a text editor, it should have the following line:
   `<Import Project="..\StardewUI\Core\StardewUI.projitems" Label="Shared" />`

Once imported, you can reference StardewUI's types directly, including [ComponentView](../reference/stardewui/widgets/componentview-1.md) and [ViewMenu](../reference/stardewui/viewmenu-1.md) which are the entry points for most user-defined views and menus, and what the Framework uses internally.

A few other considerations and tips for shared project users:

- Since you are depending on the source code, expect things to break if you update (pull).

    While there is still some risk of this even when using the other, recommended methods, breaking changes to the Framework/API should normally be reflected in the project's [semantic versioning](https://semver.org/), whereas potentially breaking changes to the source will not. If you fork or modify the shared project in any way or use any of its `internal` types or members, there is a good chance that your solution will no longer compile after an update.

- If you update/pull the shared project, and Visual Studio starts behaving strangely (not showing the correct files, producing compilation errors that make no sense, etc.), try closing Visual Studio, deleting the `.vs` folder inside your solution, and reopening it.

    The exact nature of this bug isn't clear, but sometimes Visual Studio, MSBuild, and even the `dotnet` tool can keep holding onto old state, even if you clean and rebuild. Deleting and re-importing the entire submodule will _not_ fix the problem, but deleting `.vs` usually will.

!!! info "Why use a submodule/shared project?"

    With all the ominous warnings above, you might be wondering _"why would anyone use the submodule/shared project method?"_ There are two reasons why this usage is still available and (somewhat) supported, despite being strongly recommended against:

    1. You want to avoid _any_ dependencies in your mod for personal or philosophical reasons. If you are offended by the idea of depending on GMCM, SpaceCore, etc., then you might not want to depend on StardewUI either.

    2. You don't plan on maintaining your mod long-term, and want to be immune from any future changes to StardewUI; i.e. you're willing to trade off the benefits of any new features, bug fixes, performance improvements, etc. for the guarantee of not being broken by outside changes.

    While your users would undoubtedly prefer that you maintain your mods, and most users really do not mind adding one more shared dependency to their mod list, we recognize that this may not suit all authors, which is why the shared project is only "soft-deprecated" and not officially obsolete or removed.