---
title: IAttributeBinding
description: Binding instance for a single attribute on a single view.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IAttributeBinding

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Binding instance for a single attribute on a single view.

```cs
public interface IAttributeBinding : System.IDisposable
```

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Remarks

Encapsulates the source of the value and provides a method to update the target view if the value has changed. 

 This is primarily for internal use, as a way of tracking fine-grained changes to views instead of having to rebind the entire view when anything changes.

## Members

### Properties

 | Name | Description |
| --- | --- |
| [DestinationPropertyName](#destinationpropertyname) | The name of the bound property on the destination view. | 
| [Direction](#direction) | The data flow direction for this binding. | 

### Methods

 | Name | Description |
| --- | --- |
| [UpdateSource(IView)](#updatesourceiview) | Updates the source to match the view's current value. | 
| [UpdateView(IView, Boolean)](#updateviewiview-bool) | Updates a target view with the most recent source value. | 

## Details

### Properties

#### DestinationPropertyName

The name of the bound property on the destination view.

```cs
string DestinationPropertyName { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Direction

The data flow direction for this binding.

```cs
StardewUI.Framework.Binding.BindingDirection Direction { get; }
```

##### Property Value

[BindingDirection](bindingdirection.md)

-----

### Methods

#### UpdateSource(IView)

Updates the source to match the view's current value.

```cs
void UpdateSource(StardewUI.IView target);
```

##### Parameters

**`target`** &nbsp; [IView](../../iview.md)  
The bound view.

##### Remarks

Allowed when the [Direction](iattributebinding.md#direction) is either [Out](bindingdirection.md#out) or [InOut](bindingdirection.md#inout).

-----

#### UpdateView(IView, bool)

Updates a target view with the most recent source value.

```cs
bool UpdateView(StardewUI.IView target, bool force);
```

##### Parameters

**`target`** &nbsp; [IView](../../iview.md)  
The view to receive the update.

**`force`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
If `true`, always re-publishes the latest value to the view even if the source value has not changed. Typically used for initial updates immediately after creation.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the view was updated; `false` if the update was skipped because the source value had not changed.

##### Remarks

Allowed when the [Direction](iattributebinding.md#direction) is either [In](bindingdirection.md#in) or [InOut](bindingdirection.md#inout).

-----

