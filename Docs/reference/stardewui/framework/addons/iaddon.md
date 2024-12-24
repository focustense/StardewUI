---
title: IAddon
description: Entry point for a UI add-on.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IAddon

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Addons](index.md)  
Assembly: StardewUI.dll  

</div>

Entry point for a UI add-on.

```cs
public interface IAddon
```

## Remarks

Add-ons are a plugin-like system that allow mods to extend the UI capabilities through new views, tags, converters, and other features. All add-ons must be registered via [RegisterAddon(IAddon)](../../ui.md#registeraddoniaddon).

## Members

### Properties

 | Name | Description |
| --- | --- |
| [BehaviorFactory](#behaviorfactory) | Provides user-defined behavior extensions that run on existing view types. | 
| [Dependencies](#dependencies) | List of dependencies, each corresponding to another [Id](iaddon.md#id), required by this addon. | 
| [Id](#id) | Unique ID for this addon. | 
| [ValueConverterFactory](#valueconverterfactory) | Provides user-defined type conversions in addition to the standard conversions. | 
| [ViewFactory](#viewfactory) | Provides user-defined view types and enables them to be used with custom markup tags. | 

## Details

### Properties

#### BehaviorFactory

Provides user-defined behavior extensions that run on existing view types.

```cs
StardewUI.Framework.Behaviors.IBehaviorFactory BehaviorFactory { get; }
```

##### Property Value

[IBehaviorFactory](../behaviors/ibehaviorfactory.md)

##### Remarks

All user-defined behaviors have lower priority than the built-in behaviors; a UI add-on is not allowed to remap an existing behavior name to its own implementation. Within the set of user-defined behaviors, the priority is based on inverted load order; the _last_ add-on to associate a particular name with some behavior type will be the one to always handle that name, as long as it is not a standard behavior name.

-----

#### Dependencies

List of dependencies, each corresponding to another [Id](iaddon.md#id), required by this addon.

```cs
System.Collections.Generic.IReadOnlyList<string> Dependencies { get; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)>

##### Remarks

Dependencies will always be loaded first. If any dependencies are missing, or if a cycle is detected (e.g. addon A depends on B which depends on A again) then the addon will be prevented from loading.

-----

#### Id

Unique ID for this addon.

```cs
string Id { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

##### Remarks

Prevents two copies of the same addon from trying to run at the same time, and allows other addons to depend on the features of this one by adding it to their [Dependencies](iaddon.md#dependencies).

-----

#### ValueConverterFactory

Provides user-defined type conversions in addition to the standard conversions.

```cs
StardewUI.Framework.Converters.IValueConverterFactory ValueConverterFactory { get; }
```

##### Property Value

[IValueConverterFactory](../converters/ivalueconverterfactory.md)

##### Remarks

All user-defined converters have lower priority than the built-in converters, except for the duck-type converters and [StringConverterFactory](../converters/stringconverterfactory.md) which are always considered last. Within the set of user-defined converters, the priority is based on inverted load order; the _last_ add-on that registered a converter able to handle a particular type conversion will be the one chosen, as long as none of the standard conversions can apply.

-----

#### ViewFactory

Provides user-defined view types and enables them to be used with custom markup tags.

```cs
StardewUI.Framework.Binding.IViewFactory ViewFactory { get; }
```

##### Property Value

[IViewFactory](../binding/iviewfactory.md)

##### Remarks

All user-defined views have lower priority than the built-in views; a UI add-on is not allowed to replace the behavior of a standard tag such as `<label>` or `<lane>`. Within the set of user-defined views, the priority is based on inverted load order; the _last_ add-on to associate a particular tag with some view type will be the one to always handle that tag, as long as it is not a standard tag.

-----

