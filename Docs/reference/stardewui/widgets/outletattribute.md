---
title: OutletAttribute
description: Marks a child/children property as a named outlet.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class OutletAttribute

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Marks a child/children property as a named outlet.

```cs
[System.AttributeUsage]
public class OutletAttribute : System.Attribute
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute) ⇦ OutletAttribute

## Remarks

Outlets are used by the UI Framework, in StarML views, to differentiate between multiple child properties of the same layout view. For example, the [Expander](expander.md) defines both a [Content](expander.md#content) view (the "main" view) and a separate [Header](expander.md#header) view, but normally only one children/content property is allowed per layout view. 

 When a property is decorated with an `OutletAttribute`, it is ignored by the framework unless the markup element includes an `*outlet` attribute with a value equal to the outlet [Name](outletattribute.md#name), in which case the element (or elements) will be added or assigned to that specific outlet. 

 The attribute should be omitted for whichever outlet is considered the default, i.e. to be targeted whenever the markup element does not include an `*outlet` attribute. 

 Has no effect when used outside a data binding context, or when applied to any property that does not have either [IView](../iview.md) or a collection of [IView](../iview.md) elements such as [IEnumerable&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1).

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [OutletAttribute(string)](#outletattributestring) | Marks a child/children property as a named outlet. | 

### Properties

 | Name | Description |
| --- | --- |
| [Name](#name) | The outlet name, to be matched in an `*outlet` attribute. | 
| [TypeId](https://learn.microsoft.com/en-us/dotnet/api/system.attribute.typeid) | <span class="muted" markdown>(Inherited from [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [Equals(Object)](https://learn.microsoft.com/en-us/dotnet/api/system.attribute.equals) | <span class="muted" markdown>(Inherited from [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute))</span> | 
| [GetHashCode()](https://learn.microsoft.com/en-us/dotnet/api/system.attribute.gethashcode) | <span class="muted" markdown>(Inherited from [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute))</span> | 
| [IsDefaultAttribute()](https://learn.microsoft.com/en-us/dotnet/api/system.attribute.isdefaultattribute) | <span class="muted" markdown>(Inherited from [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute))</span> | 
| [Match(Object)](https://learn.microsoft.com/en-us/dotnet/api/system.attribute.match) | <span class="muted" markdown>(Inherited from [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute))</span> | 

## Details

### Constructors

#### OutletAttribute(string)

Marks a child/children property as a named outlet.

```cs
public OutletAttribute(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The outlet name, to be matched in an `*outlet` attribute.

##### Remarks

Outlets are used by the UI Framework, in StarML views, to differentiate between multiple child properties of the same layout view. For example, the [Expander](expander.md) defines both a [Content](expander.md#content) view (the "main" view) and a separate [Header](expander.md#header) view, but normally only one children/content property is allowed per layout view. 

 When a property is decorated with an `OutletAttribute`, it is ignored by the framework unless the markup element includes an `*outlet` attribute with a value equal to the outlet [Name](outletattribute.md#name), in which case the element (or elements) will be added or assigned to that specific outlet. 

 The attribute should be omitted for whichever outlet is considered the default, i.e. to be targeted whenever the markup element does not include an `*outlet` attribute. 

 Has no effect when used outside a data binding context, or when applied to any property that does not have either [IView](../iview.md) or a collection of [IView](../iview.md) elements such as [IEnumerable&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1).

-----

### Properties

#### Name

The outlet name, to be matched in an `*outlet` attribute.

```cs
public string Name { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

