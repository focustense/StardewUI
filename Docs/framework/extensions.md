# Framework Extensions

StardewUI's [framework](index.md) is built on its own [core library](../library/index.md). The built-in set of [tags](starml.md#tags) and [type conversions](starml.md#type-conversions) are the set of [standard views](../library/standard-views.md) and first-party converters.

In some of the more unusual scenarios, the built-in features might seem either too restrictive or simply awkward (e.g. copypasta-heavy) to use. To allow more flexibility for users, StardewUI can be extended using a feature called **add-ons**.

## Creating Add-Ons

To create an initially-empty add-on:

1. Add a [direct reference to StardewUI](../library/index.md#usage).

2. Implement the [`IAddon`](../reference/stardewui/framework/addons/iaddon.md) interface. The only required property is `Id`, which should normally be your mod's ID.

    !!! example
    
        Most add-ons can copy and paste the following template. Replace the name `MyAddon` with an appropriate name for your own mod/framework.
        
        ```cs
        internal class MyAddon(string id) : IAddon
        {
            public string Id { get; } = id;
        }
        ```

3. Register the addon using [`UI.RegisterAddon`](../reference/stardewui/ui.md#registeraddoniaddon). Do this **as early as possible**, ideally in your mod's `Entry` method:

    !!! example
    
        ```cs
        internal class ModEntry : Mod
        {
            public override void Entry(IModHelper helper)
            {
                UI.RegisterAddon(new MyAddon(ModManifest.UniqueID));
    
                // Other mod initialization code
            }
        }
        ```

4. (Optional) If your add-on depends on any other add-on to function, add a [`Dependencies`](../reference/stardewui/framework/addons/iaddon.md#dependencies) field. For example, many mod authors create their own "shared" or "core" framework and an individual mod might require the add-on from this shared mod.

    !!! example
    
        ```cs
        internal class MyAddon(string id) : IAddon
        {
            public string Id { get; } = id;
        
            public IReadOnlyList<string> Dependencies { get; } =
            [
                "authorname.SharedMod",
            ];
        }
        ```
        
        Note that `authorName.SharedMod` is the **addon ID**, which by the conventions described above _should_ be the same as the mod's unique ID, but that is up to the add-on's author.

## Custom Converters

User-defined conversions extend the [default type conversions](starml.md#type-conversions), so that other types—for example, types you've defined in your own mod—can be auto-converted to view properties/attributes without requiring any explicit conversion code in the model or context.

### Registering Converters

Converters are provided through the [`IValueConverterFactory`](../reference/stardewui/framework/converters/ivalueconverterfactory.md) type; every add-on must specify exactly one (optional) converter factory. Most add-ons should use the [`ValueConverterFactory`](../reference/stardewui/framework/converters/valueconverterfactory.md) base class, which simplifies the registration of multiple converters and is ideal for using as a "root" factory that handles many different types of conversions.

The [example addon](https://github.com/focustense/StardewUI/blob/feaffb9457ea0ad637e4dafd43bb9d7c019f4b97/TestAddon/ExampleAddon.cs#L15) uses the following logic:

```cs
internal class ExampleAddon(string id) : IAddon
{
    public string Id { get; } = id;
    
    // --- Other addon features ---
    
    public IValueConverterFactory ValueConverterFactory =>
        valueConverterFactory.Value;
    
    private readonly Lazy<IValueConverterFactory> valueConverterFactory =
        new(() =>
        {
            var factory = new ValueConverterFactory();
            factory.TryRegister(new ItemIdToSpriteConverter());
            factory.TryRegister(new StringToKeySplineConverter());
            return factory;
        });
}
```

Using `Lazy<T>` here can help slightly with performance, as it ensures that your factory/converters aren't created until they are actually necessary for a conversion; however, it is not necessary to do this and could easily replaced with a line such as:

```cs
public IValueConverterFactory ValueConverterFactory { get; } =
    CreateValueConverterFactory();
```

where `CreateValueConverterFactory` is some static method.

### Implementing Converters

Converters can be very simple or very complex. A simple example is the [ItemIdToSpriteConverter](https://github.com/focustense/StardewUI/blob/dev/TestAddon/ItemIdToSpriteConverter.cs) in the test addon:

```cs
internal class ItemIdToSpriteConverter : IValueConverter<string, Sprite>
{
    public Sprite Convert(string value)
    {
        var itemData = ItemRegistry.GetDataOrErrorItem(value);
        return new(itemData.GetTexture(), itemData.GetSourceRect());
    }
}
```

This is simply a generic implementation of `IValueConverter`, which converts a `string` to a `Sprite`, assuming that string is a Stardew Valley Item ID. (Note that StardewUI doesn't include this converter as built-in, because even though every valid item ID is expected to have a sprite, not every string is going to be a valid item ID!)

More complex converters that operate on runtime types will generally have to do so by adding a "sub-factory" or delegate factory implementing `IValueConverterFactory` and then registering that via [`ValueConverterFactory.Register`](../reference/stardewui/framework/converters/valueconverterfactory.md#registerivalueconverterfactory). The implementation of one of these delegates will generally use at least some amount of reflection. A good beginner's example of a dynamic converter is the [`EnumNameConverterFactory`](https://focustense.github.io/StardewUI/library/standard-views/), which allows any `string` to be converted to **any** enum type (not just a specific enum type) using its name.

!!! example

    ```cs
    public class EnumNameConverterFactory : IValueConverterFactory
    {
        public bool TryGetConverter<TSource, TDestination>(
            [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
        )
        {
            if (typeof(TSource) != typeof(string) || !typeof(TDestination).IsEnum)
            {
                converter = null;
                return false;
            }
            var converterType = typeof(Converter<>).MakeGenericType(typeof(TDestination));
            converter = (IValueConverter<TSource, TDestination>)
                Activator.CreateInstance(converterType)!;
            return true;
        }
    
        class Converter<T> : IValueConverter<string, T>
            where T : struct, Enum
        {
            public T Convert(string value)
            {
                return Enum.Parse<T>(value, true);
            }
        }
    }
    ```

Since many dynamic conversions are already built in (enums, `Nullable<T>`, assignment casts, etc.) the most likely reason you might want to define a new one is to handle conversion to or from a _generic_ type. [`NullableConverterFactory`](https://github.com/focustense/StardewUI/blob/dev/Framework/Converters/NullableConverterFactory.cs) illustrates the type of work generally involved with this approach.

Converters are cached by type, so it is not necessary to do your own caching in a custom `IValueConverterFactory`, nor to spend too much time optimize the `TryGetConverter` method. Instead, focus on making the actual `Convert` method as fast as possible, since that is the method that may be called on every view update.

!!! danger

    **Never include any state in a converter**, since the same converter will be reused for all conversions between the source and destination types. Stateful converters may break at arbitrary and unexpected times, and in ways that are difficult to isolate or debug.

## Custom Views

Every [tag](starml.md#tags) is really a [view](../library/standard-views.md), with the exception of a few special tags like `<include>`. If you can't find a standard view, or combination of standard views, that cleanly does what you are looking for, then there are several options for creating [custom views](../library/custom-views.md).

### Registering Custom Views

To register a view you've created as a custom tag, define and register a new [`IViewFactory`](../reference/stardewui/framework/binding/iviewfactory.md) implementation. The registration API is very similar to that for [converters](#registering-converters), also providing a base class, [`ViewFactory`](../reference/stardewui/framework/binding/viewfactory.md), to make registration of views more convenient.

The [Carousel example](../examples/carousel.md) defines a custom view, appropriately called [`Carousel`](https://github.com/focustense/StardewUI/blob/dev/TestAddon/Carousel.cs), and registers it via:

```cs
internal class ExampleAddon(string id) : IAddon
{
    public string Id { get; } = id;
    
    // --- Other addon features ---
    
    public IViewFactory ViewFactory => viewFactory.Value;
    
    private readonly Lazy<IViewFactory> viewFactory = new(() =>
    {
        var factory = new ViewFactory();
        factory.Register<Carousel>("carousel");
        return factory;
    });
}
```

As with converters, it is useful, but not required, to use `Lazy<T>` for this, as long as the `ViewFactory` property retrieves a single long-lived instance and does not recreate the factory each time.

### Using Custom Views

Once a custom view is registered, it can be used in a [StarML](starml.md) document like any other tag, using the registered tag name:

```html
<carousel layout="stretch"
          selection-layout="600px stretch"
          easing="OutCubic"
          gap="50"
          selected-index={SelectedPageIndex}>
    <!-- Carousel Contents -->
</carousel>
```