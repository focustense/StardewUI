---
title: IViewDefaults
description: Provides access to the default values of a view's properties.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IViewDefaults

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Provides access to the default values of a view's properties.

```cs
public interface IViewDefaults
```

## Remarks

These defaults are not part of the [IViewDescriptor](iviewdescriptor.md) or [IPropertyDescriptor&lt;T&gt;](ipropertydescriptor-1.md) interfaces because they cannot be reliably detected through reflection alone; instead they require support from a source generator, if known at compile time, or a dummy/"blank" instance of the view created at runtime otherwise.

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetDefaultValue&lt;T&gt;(string)](#getdefaultvaluetstring) | Gets the default value for the named property. | 

## Details

### Methods

#### GetDefaultValue&lt;T&gt;(string)

Gets the default value for the named property.

```cs
T GetDefaultValue<T>(string propertyName);
```

##### Parameters

**`propertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The property name.

##### Returns

`T`

  The default value of the specified property for a newly-created view.

-----

