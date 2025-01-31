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
| [GetFlag&lt;T&gt;(string)](#getflagtstring) | Gets the current value of a flag, if one is set. | 
| [GetOrAddProperty&lt;T&gt;(string)](#getoraddpropertytstring) | Gets the override states for the specified property, creating a new one if it does not already exist. | 
| [GetProperty&lt;T&gt;(string)](#getpropertytstring) | Gets the override states for the specified property, if any exist. | 
| [SetFlag(string, Object)](#setflagstring-object) | Sets an arbitrary flag that other behaviors can read and/or be notified about. | 
| [Write(IView)](#writeiview) | Writes the active overrides to the target view. | 

### Events

 | Name | Description |
| --- | --- |
| [FlagChanged](#flagchanged) | Event raised when a flag changes, i.e. as the outcome of [SetFlag(string, Object)](iviewstate.md#setflagstring-object). | 

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

#### GetFlag&lt;T&gt;(string)

Gets the current value of a flag, if one is set.

```cs
T GetFlag<T>(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The flag name.

##### Returns

`T`

  The flag value, or the default of `T` if not set.

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

#### SetFlag(string, Object)

Sets an arbitrary flag that other behaviors can read and/or be notified about.

```cs
void SetFlag(string name, System.Object value);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The flag name.

**`value`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The flag value, or `null` to unset.

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

### Events

#### FlagChanged

Event raised when a flag changes, i.e. as the outcome of [SetFlag(string, Object)](iviewstate.md#setflagstring-object).

```cs
event EventHandler<StardewUI.Framework.Behaviors.FlagEventArgs>? FlagChanged;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[FlagEventArgs](flageventargs.md)>

-----

