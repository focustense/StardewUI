---
title: BoundViewDefaults
description: View defaults that provide the current data-bound values for any bound attributes/properties and fall back to the original ("blank") view defaults for unbound properties.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BoundViewDefaults

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

View defaults that provide the current data-bound values for any bound attributes/properties and fall back to the original ("blank") view defaults for unbound properties.

```cs
public class BoundViewDefaults : StardewUI.Framework.Descriptors.IViewDefaults
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ BoundViewDefaults

**Implements**  
[IViewDefaults](../descriptors/iviewdefaults.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [BoundViewDefaults(IViewDefaults, IEnumerable&lt;IAttributeBinding&gt;)](#boundviewdefaultsiviewdefaults-ienumerableiattributebinding) | View defaults that provide the current data-bound values for any bound attributes/properties and fall back to the original ("blank") view defaults for unbound properties. | 

### Methods

 | Name | Description |
| --- | --- |
| [GetDefaultValue&lt;T&gt;(string)](#getdefaultvaluetstring) | Gets the default value for the named property. | 

## Details

### Constructors

#### BoundViewDefaults(IViewDefaults, IEnumerable&lt;IAttributeBinding&gt;)

View defaults that provide the current data-bound values for any bound attributes/properties and fall back to the original ("blank") view defaults for unbound properties.

```cs
public BoundViewDefaults(StardewUI.Framework.Descriptors.IViewDefaults original, System.Collections.Generic.IEnumerable<StardewUI.Framework.Binding.IAttributeBinding> attributes);
```

##### Parameters

**`original`** &nbsp; [IViewDefaults](../descriptors/iviewdefaults.md)  
The original or reference defaults for the managed view type.

**`attributes`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[IAttributeBinding](iattributebinding.md)>  
The bound property attributes for the view's node.

-----

### Methods

#### GetDefaultValue&lt;T&gt;(string)

Gets the default value for the named property.

```cs
public T GetDefaultValue<T>(string propertyName);
```

##### Parameters

**`propertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The property name.

##### Returns

`T`

  The default value of the specified property for a newly-created view.

-----

