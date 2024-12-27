---
title: PropertyStateList&lt;T&gt;
description: Simple list-based implementation of IPropertyStates&lt;T&gt; optimized for low override counts, typically fewer than 5 and never more than 10-20.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class PropertyStateList&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Simple list-based implementation of [IPropertyStates&lt;T&gt;](ipropertystates-1.md) optimized for low override counts, typically fewer than 5 and never more than 10-20.

```cs
public class PropertyStateList<T> : 
    StardewUI.Framework.Behaviors.IPropertyStates<T>, 
    System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, T>>, 
    System.Collections.IEnumerable, 
    System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<string, T>>, 
    System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.KeyValuePair<string, T>>
```

### Type Parameters

**`T`**  
The property value type.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ PropertyStateList&lt;T&gt;

**Implements**  
[IPropertyStates&lt;T&gt;](ipropertystates-1.md), [IEnumerable&lt;KeyValuePair&lt;string, T&gt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1), [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.ienumerable), [IReadOnlyList&lt;KeyValuePair&lt;string, T&gt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1), [IReadOnlyCollection&lt;KeyValuePair&lt;string, T&gt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlycollection-1)

## Remarks

Internally uses a [List&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1), which is memory-efficient and has fast appends, but requires shifting items when a state is removed from the middle of the list. This is suitable for small stacks (e.g. a pressed state on top of a hover state, where the latter might be removed before the former), but if the stacks become very large, i.e. having hundreds of items, then a different implementation such as linked list or linked hash set might be required. Pushing a new state also requires checking for the existing state first, which is faster than hashing for very small lists but, similar to removals, may be inefficient for very large ones.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [PropertyStateList&lt;T&gt;()](#propertystatelistt) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [Count](#count) |  | 
| [Item[int]](#itemint) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [GetEnumerator()](#getenumerator) |  | 
| [Push(string, T)](#pushstring-t) | Pushes a new state to the top of the stack, making it the active override. | 
| [Replace(string, T)](#replacestring-t) | Replaces the value associated with a specified state. | 
| [TryPeek(ValueTuple&lt;string, T&gt;)](#trypeekvaluetuplestring-t) | Gets the state name and value with highest priority, i.e. on top of the stack. | 
| [TryPeekValue(T&&lt;&gt;)](#trypeekvaluet) | Gets the value with highest priority, i.e. on top of the stack. | 
| [TryRemove(string, T&&lt;&gt;)](#tryremovestring-t) | Removes a specified state override, if one exists. | 

## Details

### Constructors

#### PropertyStateList&lt;T&gt;()



```cs
public PropertyStateList<T>();
```

-----

### Properties

#### Count



```cs
public int Count { get; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### Item[int]



```cs
public System.Collections.Generic.KeyValuePair<string, T> Item[int] { get; }
```

##### Property Value

[KeyValuePair&lt;string, T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.keyvaluepair-2)

-----

### Methods

#### GetEnumerator()



```cs
public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<string, T>> GetEnumerator();
```

##### Returns

[IEnumerator&lt;KeyValuePair&lt;string, T&gt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerator-1)

-----

#### Push(string, T)

Pushes a new state to the top of the stack, making it the active override.

```cs
public void Push(string stateName, T value);
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
public bool Replace(string stateName, T value);
```

##### Parameters

**`stateName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the state on the stack.

**`value`** &nbsp; T  
The new value to associate with the specified `stateName`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

If no state with the specified `stateName` is on the stack, then this does nothing. It will not push a new state.

-----

#### TryPeek(ValueTuple&lt;string, T&gt;)

Gets the state name and value with highest priority, i.e. on top of the stack.

```cs
public bool TryPeek(out ValueTuple<string, T> result);
```

##### Parameters

**`result`** &nbsp; [ValueTuple&lt;string, T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.valuetuple-2)  
The state name and value of the active override, or the default for `T` if the function returned `false`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### TryPeekValue(T&&lt;&gt;)

Gets the value with highest priority, i.e. on top of the stack.

```cs
public bool TryPeekValue(out T&<> value);
```

##### Parameters

**`value`** &nbsp; `T`  
The value of the active override, or the default for `T` if the function returned `false`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### TryRemove(string, T&&lt;&gt;)

Removes a specified state override, if one exists.

```cs
public bool TryRemove(string stateName, out T&<> value);
```

##### Parameters

**`stateName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the state on the stack.

**`value`** &nbsp; `T`  
The value associated with the specified `stateName`, if there was an existing override, or `null` if there was no instance of that state.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

