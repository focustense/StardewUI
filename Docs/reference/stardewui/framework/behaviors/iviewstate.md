---
title: IViewState
description: Provides access to all state-based overrides associated with a view.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IViewState

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Provides access to all state-based overrides associated with a view.

```cs
public interface IViewState
```

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetDefaultValue&lt;T&gt;(string)](#getdefaultvaluetstring) | Retrieves the default value for a given property. | 
| [GetOrAddProperty&lt;T&gt;(string)](#getoraddpropertytstring) | Gets the override states for the specified property, creating a new one if it does not already exist. | 
| [GetProperty&lt;T&gt;(string)](#getpropertytstring) | Gets the override states for the specified property, if any exist. | 
| [Write(IView)](#writeiview) | Writes the active overrides to the target view. | 

## Details

### Methods

#### GetDefaultValue&lt;T&gt;(string)

Retrieves the default value for a given property.

```cs
T GetDefaultValue<T>(string propertyName);
```

##### Parameters

**`propertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The property name.

##### Returns

`T`

  The default value for the specified `propertyName`.

##### Remarks

The default value is the value that will be used when there are no states for that property, i.e. when [GetProperty&lt;T&gt;(string)](iviewstate.md#getpropertytstring) returns `null` for the specified `propertyName` or when the property's states are empty. 

 Defaults are real-time; if the property is linked via data binding, then the default value is the value that is currently bound.

-----

#### GetOrAddProperty&lt;T&gt;(string)

Gets the override states for the specified property, creating a new one if it does not already exist.

```cs
StardewUI.Framework.Behaviors.IPropertyStates<T> GetOrAddProperty<T>(string propertyName);
```

##### Parameters

**`propertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The property name.

##### Returns

[IPropertyStates&lt;T&gt;](ipropertystates-1.md)

  The state overrides for the specified property on the current view.

-----

#### GetProperty&lt;T&gt;(string)

Gets the override states for the specified property, if any exist.

```cs
StardewUI.Framework.Behaviors.IPropertyStates<T> GetProperty<T>(string propertyName);
```

##### Parameters

**`propertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The property name.

##### Returns

[IPropertyStates&lt;T&gt;](ipropertystates-1.md)

  The state overrides for the specified property, or `null` if none have been added.

-----

#### Write(IView)

Writes the active overrides to the target view.

```cs
void Write(StardewUI.IView view);
```

##### Parameters

**`view`** &nbsp; [IView](../../iview.md)  
The view that should receive the state/overrides.

-----

