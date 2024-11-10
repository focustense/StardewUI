# Binding Context

StardewUI's framework API is based on [data binding](../concepts.md#data-binding). While the UI state or "widget tree" is retained, you don't interact it with it directly; instead, in order to provide interactivity and/or dynamic content to views, you define a [data model](https://en.wikipedia.org/wiki/Data_model), which becomes the _context_.

Context is simply a [tree node](https://en.wikipedia.org/wiki/Tree_(abstract_data_type)) which contains the particular model/data for a specific view or position in the hierarchy. It provides access to the model itself, as well as [redirects](#redirects), which are important for building more complex UI. Other frameworks use similar names, such as [DataContext](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement.datacontext?view=windowsdesktop-8.0) in WPF.

## Context vs Data

As an example, consider a simple list of selectable items:

=== "Model Diagram"

    ```mermaid
    classDiagram
    direction LR
        class MenuViewModel {
            string Title
            List~MenuItem~ Items
            SelectItem(id)
        }
        class MenuItem {
            string Id
            string Text
            int Price
        }
        MenuViewModel *-- MenuItem
    ```

=== "UI Mockup"

    ```mermaid
    block-beta
    columns 2
        A["Joey Jo-Jo Junior Shabadoo's Shop"]:2
        space:2
        B["Bread"] C["500"]
        D["Peanut Butter"] E["1200"]
        F["Strawberry Jam"] G["800"]
    ```

=== "StarML"

    ```html
    <lane layout="500px content" orientation="vertical">
        <label font="dialogue" text={Title} />
        <lane *repeat={Items} click=|^SelectItem(Id)|>
            <label margin="0, 0, 0, 32" text={Text} />
            <label text={Price} />
        </lane>
    </lane>
    ```

The specific layout or exact appearance in game isn't important to this example; what we're focused on here are the [binding attributes](starml.md#attribute-flavors) and [events](binding-events.md). Specifically, we have:

- `text={Title}`, which is clearly referring to `MenuViewModel.Title`
- `click=|^SelectItem(Id)|`, which refers to the same-named method on `MenuViewModel`
- `text={Text}` and `text={Price}` which are referring, _not_ to the `MenuViewModel` anymore, but to properties on the `MenuItem`.

What happened? Although we only actually provided a single "model" (the `MenuViewModel`), the _context_ changed as soon as `*repeat` was encountered. Repeaters replace the original binding context (`MenuViewModel`) with a context referring to the specific item in the collection (`MenuItem`). As far as those inner `<label>` elements are concerned, the "model" or "data" is actually `MenuItem`.

This behavior isn't necessarily limited to repeaters; for example, another way to narrow or drill down the context would be the [`*context` attribute](starml.md#structural-attributes).

But note the `click` event in particular: `^SelectItem(Id)`. The `Id` is a property on the `MenuItem`, as we would expect since it is attached to the repeating element; however, `SelectItem` is referring back to the `MenuViewModel`. It does this using the `^` [redirect](#redirects), which instructs StardewUI to look at the _parent_ or _previous_ context to find the `SelectItem` method.

The _model_ or _data_ refers to the specific object attached, or "bound", to any given node; the outer `<lane>` is bound to a `MenuViewModel` and each inner `<lane>` is bound to a `MenuItem`. The _context_ has an awareness of the overall structure, and is able to backtrack to a previous level at any time.

## Source Updates

It is rare for any UI to be completely static, with content that never changes. Most UI needs to respond not only to user input, but to changes in the underlying data or state. More generally, it should always show the _most current_ data, however one chooses to define it.

!!! failure "Broken"

    === "C#"
    
        ```cs
        public class CounterViewModel
        {
            public int Count { get; set; }
            
            public void Increment()
            {
                Count++;
            }
        }
        ```
    
    === "StarML"
    
        ```html
        <lane>
            <button click=|Increment()| text="Add One" />
            <label text={Count} />
        </lane>
        ```

If you run this example, and click the button, you'll see that **nothing happens**. Our `Count` does get incremented, but the UI never updates. That is because StardewUI's data binding depends on having an implementation of [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-8.0) (hereafter: "INPC") in order to detect changes. Without INPC, every binding is a "one-time" binding, meaning it is only read when the view is first created.

### Implementing `INotifyPropertyChanged`

INPC can be extremely tedious to implement for many models and properties, as it requires intercepting every property setter, comparing the values, raising an event, etc. Fortunately, the .NET ecosystem has many tools to automate the process, and using one is **strongly** recommended over a "manual" implementation.

The recommended options for INPC include:

| Library | Package | Source |
| ---- | ---- | --- |
| PropertyChanged.SourceGenerator | [:simple-nuget: NuGet](https://www.nuget.org/packages/PropertyChanged.SourceGenerator) | [:simple-github: GitHub](https://github.com/canton7/PropertyChanged.SourceGenerator) |
| PropertyChanged.Fody | [:simple-nuget: NuGet](https://www.nuget.org/packages/PropertyChanged.Fody/) | [:simple-github: GitHub](https://github.com/Fody/PropertyChanged) |

The author(s) of this guide and of StardewUI have no affiliation with any of the above projects; they are recommended on the basis of:

- :white_check_mark: Leaving **no footprint**.

    Although they are installed as packages, they do not add any new assemblies to your build output, meaning your INPC-enhanced mod can continue to be a single DLL;

- :white_check_mark: Handling **dependent properties** automatically.

    For example, `public string FullName => $"{FirstName} {LastName}"` will raise notifications for changes to either `FirstName` or `LastName`.

- :white_check_mark: Being **free and open source**, with a permissive license (MIT, Apache, etc.).[^1]

    In other words, their license imposes no requirements on _your_ license, your ability to opt into Nexus Donation Points, etc.

[^1]: Fody is ["legally free, morally paid"](https://github.com/Fody/Home/blob/master/pages/licensing-patron-faq.md#but-it-is-mit-cant-i-use-it-for-free), but the advantages of this clearly show in its maintenance statistics; `PropertyChanged` is over 12 years old, still being updated, and has the most frictionless syntax, in addition to the Fody master project being a much larger collection of also useful tools.

Using the first library (`PropertyChanged.SourceGenerator`), we can quickly convert the non-working example above to one that does work:

!!! success

    === "C#"
    
        ```cs
        using PropertyChanged.SourceGenerator;
    
        public partial class CounterViewModel
        {
            [Notify] private int count;
            
            public void Increment()
            {
                Count++;
            }
        }
        ```
    
    === "StarML"
    
        ```html
        <lane>
            <button click=|Increment()| text="Add One" />
            <label text={Count} />
        </lane>
        ```

Note that the markup has not changed at all. All we had to do for the C# code was:

- Add the `partial` keyword to the `CounterViewModel` class (required for code generation)
- Change the `Count` auto-property to be a field
- Make it lowerCamelCase, so it doesn't conflict with the auto-generated property
- Add a `[Notify]` attribute.

You don't need to `[Notify]` every single property, only the ones that might change. The mixed approach is demonstrated in several [examples](../examples/bestiary.md/#monsterviewmodelcs).

### Collection Updates

A special case of updates is collections. Consider the case of a UI that adds items to a list:

!!! failure "Broken"

    === "C#"
    
        ```cs
        public partial class TodoViewModel
        {
            public List<string> Items { get; } = [];
    
            [Notify] private string currentItem = "";
    
            public void AddCurrentItem()
            {
                if (!string.IsNullOrWhitespace(CurrentItem))
                {
                    Items.Add(CurrentItem);
                    CurrentItem = "";
                }
            }
        }
        ```
    
    === "StarML"
    
        ```html
        <lane layout="500px content" orientation="vertical">
            <lane>
                <textinput text={CurrentItem} />
                <button text="Add" click=|AddCurrentItem()| />
            </lane>
            <label *repeat={Items} text={this} />
        </lane>
        ```

If you run this, you'll observe that the text box is cleared when clicking "Add", but the item does not appear to be added to the list. Moreover, converting `Items` to a `[Notify]` will not help in this case, because the _field itself_ has not changed. `Items` always points to the same _list_.

Before explaining the solution, it is worth noting the workarounds that should **not** be used, even if they appear to be effective at first:

- :x: Converting the `Items` list to `[Notify]` (or equivalent INPC) and replacing the entire list with a new list, e.g. `Items = Items.Append(CurrentItem).ToList()`.

    This has quadratic or [Schlemiel the Painter](https://www.joelonsoftware.com/2001/12/11/back-to-basics/) performance, in both time and memory.

- :x: Leaving the list alone, but calling an `OnPropertyChanged` or `PropertyChanged?.Invoke` after adding an item, to force an INPC change notification.

    While not as serious an offense as the previous version, it still forces StardewUI to rebuild the entire view tree for the `*repeat`. Even if only one item changed, it must recreate the views for all of them.

All performance is relative, and these workarounds might be perfectly acceptable for our toy example above. However, if there are hundreds of items in the list, and each item has many views within—for example, an image, quality icon, quantity text, etc.—then making this type of change very often is still likely to cause jank.

StardewUI has a better solution: [`INotifyCollectionChanged`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=net-8.0), which is the collection-based counterpart to INPC. While "INCC" is also difficult to implement, you don't have to, because there is already an [`ObservableCollection`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=net-8.0) type to do it for you.

Using `ObservableCollection`, the revised and working code then becomes:

!!! success

    === "C#"
    
        ```cs
        public partial class TodoViewModel
        {
            public ObservableCollection<string> Items { get; } = [];
    
            [Notify] private string currentItem = "";
    
            public void AddCurrentItem()
            {
                if (!string.IsNullOrWhitespace(CurrentItem))
                {
                    Items.Add(CurrentItem);
                    CurrentItem = "";
                }
            }
        }
        ```
    
    === "StarML"
    
        ```html
        <lane layout="500px content" orientation="vertical">
            <lane>
                <textinput text={CurrentItem} />
                <button text="Add" click=|AddCurrentItem()| />
            </lane>
            <label *repeat={Items} text={this} />
        </lane>
        ```

We changed only one line here: `List<string>` became `ObservableCollection<string>`.

## Redirects

Any binding that references context data, including properties or [event handlers](binding-events.md) or arguments, can use a redirect operator.

The types of redirects are:

| Name     | Syntax  | Example     | Behavior |
| -------- | ------- | ----------- | -------- |
| Parent   | `^`     | `^Prop`     | Goes back to the previous, or "parent" context.<br/>**Can be repeated.** |
| Ancestor | `~Type` | `~Foo.Prop` | Backtracks to the nearest ancestor of the specified type.<br/>**Cannot be repeated.** |

The following example will walk through the different redirects in more detail.

=== "C#"

    ```cs
    class InventoryViewModel
    {
        public string OwnerName { get; set; }
        public PageViewModel ActivePage { get; set; }
    }
    
    class PageViewModel
    {
        public string Category { get; set; }
        public List<ItemViewModel> Items { get; set; }
    }
    
    record ItemViewModel(string Name);
    
    void ShowMenu()
    {
        var context = new InventoryViewModel()
        {
            OwnerName = "Timmy",
            ActivePage = new()
            {
                Category = "Tools",
                Items = [new("Axe"), new("Hoe")]
            },
        };
        Game1.activeClickableMenu =
            viewEngine.CreateMenuFromAsset("Mods/Xyz/Views/Inventory", context);
    }
    ```

=== "StarML"

    ```html
    <lane orientation="vertical" *context={ActivePage}>
        <label text={^OwnerName} />
        <label text={~InventoryViewModel.OwnerName} />
        <lane *repeat={Items}>
            <label text={Name} />
            <label text={^Category} />
            <label text={~PageViewModel.Category} />
            <label text={^^OwnerName} />
            <label text={~InventoryViewModel.OwnerName} />
        </lane>
    </lane>
    ```

=== "Output"

    ```mermaid
    block-beta
    columns 5
        A["Timmy"]:5
        B["Timmy"]:5
        C1["Axe"]
        C2["Tools"]
        C3["Tools"]
        C4["Timmy"]
        C5["Timmy"]
        D1["Hoe"]
        D2["Tools"]
        D3["Tools"]
        D4["Timmy"]
        D5["Timmy"]
    ```

The above is not intended to represent any kind of realistic UI scenario, only to demonstrate what the different redirects do. It is important to realize that redirects do **not** navigate the actual model data; there is nothing in StardewUI, or anywhere else, that knows how to get from a `PageViewModel` back to its "parent" `InventoryViewModel`, and such a relationship may not exist or be meaningful at all.

Instead, we assign a context to each node:

::spantable:: class="dense"

| Node/Element @span | |                                   | Has Context                     |
| | | ---------------------------------------------------- | ------------------------------- |
| `<lane orientation​=​"vertical"...>` @span | |           | `InventoryViewModel` ("Timmy")  |
| | ` <label text={^OwnerName} />` @span |                 | `PageViewModel` ("Tools") @span |
| | `<label text={~PageViewModel.OwnerName} />` @span |    |                                 |
| | `<lane *repeat={Items}>` @span                         |                                 |
| | | `[1] <label text={Name} />`                          | `ItemViewModel` ("Axe") @span   |
| | | `[1] <label text={^Category} />`                     |                                 |
| | | `[1] <label text={~PageViewModel.Category} />`       |                                 |
| | | `[1] <label text={^^OwnerName} />`                   |                                 |
| | | `[1] <label text={~InventoryViewModel.OwnerName} />` |                                 |
| | | `[2] <label text={Name} />`                          | `ItemViewModel` ("Hoe") @span   |
| | | `[2] <label text={^Category} />`                     |                                 |
| | | `[2] <label text={~PageViewModel.Category} />`       |                                 |
| | | `[2] <label text={^^OwnerName} />`                   |                                 |
| | | `[2] <label text={~InventoryViewModel.OwnerName} />` |                                 |

::end-spantable::

To resolve the `^^OwnerName` near the end of the above table:

1. Walk up the parent _elements_ until we find one that changed the context. Since `*repeat` has a context effect, the first parent is the `<lane *repeat={Items}>` element.
2. Repeat the process from that position, since there is a second `^`. Since the `<lane orientation="vertical">` element has a `*context` modifier, it is chosen next.
3. Take the context data _linked to_ the element we ended up at; the root `<lane>` is associated with the `InventoryViewModel`.
4. Read the property from the data we just found, i.e. `InventoryViewModel.OwnerName`.

Resolving `~InventoryViewModel.OwnerName` follows a very similar process, but instead of going "up" a specific number of times (twice, for `^^`, or once, for a single `^`), it repeats the traversal step as many times as necessary until it reaches an element that has an `InventoryViewModel` as its context.

!!! info "Summary"

    The most important lesson to take away from this is that **context redirects follow the document structure, not the data (model/view-model) structure.**
    
    In an MV* design, document structure is itself usually based on the model, so you can _often_ treat redirects as going to the "parent object", but may eventually run into scenarios where this doesn't work as expected.
    
    Remember that context lives _outside_ your data.
    
## Update Ticks

As a convenience, StardewUI can dispatch update ticks to any objects bound as context so that you do not need to "drive" them from `ModEntry`, i.e. using SMAPI's [UpdateTicked](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Events#Game_loop) event.

Importantly, these updates can be at any arbitrary nesting level, as long as they are reachable by some view/node; they do not have to be at the top level. This can be useful for running animations, synchronizing with external game state or netfields, or for any other purpose that requires frame-precision updates.

To opt in, simply add a `void Update` method to any context type with either no parameters or a single `TimeSpan` parameter.

!!! example

    === "Code (C#)"
    
        ```cs
        public class OuterModel
        {
            public StopwatchModel Stopwatch { get; set; }
        }
        
        public class InnerModel
        {
            public string FormattedTime => Elapsed.ToString(@"mm\:ss\.fff");

            [Notify] private TimeSpan elapsed;

            public void Update(TimeSpan elapsed)
            {
                Elapsed += elapsed;
            }
        }
        ```

    === "View (StarML)"
    
        ```html
        <frame *context={Stopwatch}>
            <label text={FormattedTime} />
        </frame>
        ```

The `Update` method **must** match the signature above (with or without the `TimeSpan` argument); any other signature will cause a warning to be logged and the method to be ignored.