---
title: IPropertyStates&lt;T&gt;
description: Provides methods for tracking and modifying state-based overrides for a view's property.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IPropertyStates&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Provides methods for tracking and modifying state-based overrides for a view's property.

```cs
public interface IPropertyStates<T> : 
    System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, T>>, 
    System.Collections.IEnumerable
```

### Type Parameters

**`T`**  
The property value type.


**Implements**  
[IEnumerable&lt;KeyValuePair&lt;string, T&gt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1), [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.ienumerable)

## Remarks

State overrides provide a clean priority scheme and reversion path for semantic states such as "hover" or "pressed". Instead of behaviors modifying an [IView](../../iview.md) directly, they can instead push their override to the view's propert states, and as long as that override remains the topmost state, it is authoritative for that specific view and property. If it is later removed, then whichever other state is subsequently on top takes precedence. 

 Using this abstraction avoids the need for individual behaviors to save the previous value, and more importantly, prevents unintended conflicts between multiple behaviors each trying to act on the same property of the same view.

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Push(string, T)](#pushstring-t) | Pushes a new state to the top of the stack, making it the active override. | 
| [Replace(string, T)](#replacestring-t) | Replaces the value associated with a specified state. | 
| [TryPeek(T&&lt;&gt;)](#trypeekt) | Gets the value with highest priority, i.e. on top of the stack. | 
| [TryRemove(string, T&&lt;&gt;)](#tryremovestring-t) | Removes a specified state override, if one exists. | 

## Details

### Methods

#### Push(string, T)

Pushes a new state to the top of the stack, making it the active override.

```cs
void Push(string stateName, T value);
```

##### Parameters

**`stateName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the new state.

**`value`** &nbsp; T  
The property value to use when while the state is active.

##### Remarks

If a state with the specified `stateName` already exists on the stack, then this will remove the previous instance and add the new instance on top.

-----

#### Replace(string, T)

Replaces the value associated with a specified state.

```cs
void Replace(string stateName, T value);
```

##### Parameters

**`stateName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the state on the stack.

**`value`** &nbsp; T  
The new value to associate with the specified `stateName`.

##### Remarks

If no state with the specified `stateName` is on the stack, then this does nothing. It will not push a new state.

-----

#### TryPeek(T&&lt;&gt;)

Gets the value with highest priority, i.e. on top of the stack.

```cs
bool TryPeek(out T&<> value);
```

##### Parameters

**`value`** &nbsp; `T`  
The value of the active override, or the default for `T` if the function returned `false`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if there was at least one active override for this property; `false` if the stack is currently empty.

-----

#### TryRemove(string, T&&lt;&gt;)

Removes a specified state override, if one exists.

```cs
bool TryRemove(string stateName, out T&<> value);
```

##### Parameters

**`stateName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the state on the stack.

**`value`** &nbsp; `T`  
The value associated with the specified `stateName`, if there was an existing override, or `null` if there was no instance of that state.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if an override for the specified `stateName` was removed from the stack; `false` if no such state was on the stack.

-----

