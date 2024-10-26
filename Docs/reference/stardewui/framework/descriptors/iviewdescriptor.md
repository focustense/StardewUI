---
title: IViewDescriptor
description: Describes a type of view that can be used in a view binding.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IViewDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Describes a type of view that can be used in a view binding.

```cs
public interface IViewDescriptor : 
    StardewUI.Framework.Descriptors.IObjectDescriptor
```

**Implements**  
[IObjectDescriptor](iobjectdescriptor.md)

## Remarks

The binding target is independent of the actual [IView](../../iview.md) instance; it provides methods and data to support interacting with any view of the given [TargetType](iobjectdescriptor.md#targettype).

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetChildrenProperty()](#getchildrenproperty) | Retrieves the property of the [TargetType](iobjectdescriptor.md#targettype) that holds the view's children/content. | 
| [TryGetChildrenProperty(IPropertyDescriptor)](#trygetchildrenpropertyipropertydescriptor) | Attempts to retrieve the property of the [TargetType](iobjectdescriptor.md#targettype) that holds the view's children/content. | 

## Details

### Methods

#### GetChildrenProperty()

Retrieves the property of the [TargetType](iobjectdescriptor.md#targettype) that holds the view's children/content.

```cs
StardewUI.Framework.Descriptors.IPropertyDescriptor GetChildrenProperty();
```

##### Returns

[IPropertyDescriptor](ipropertydescriptor.md)

  The view children property.

-----

#### TryGetChildrenProperty(IPropertyDescriptor)

Attempts to retrieve the property of the [TargetType](iobjectdescriptor.md#targettype) that holds the view's children/content.

```cs
bool TryGetChildrenProperty(out StardewUI.Framework.Descriptors.IPropertyDescriptor property);
```

##### Parameters

**`property`** &nbsp; [IPropertyDescriptor](ipropertydescriptor.md)  
When this method returns, holds a reference to the [IPropertyDescriptor](ipropertydescriptor.md) that holds the view's children/content, or `null` if no such property is available.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if a children/content property was found, otherwise `false`.

-----

