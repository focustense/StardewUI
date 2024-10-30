---
title: DirtyTrackingList&lt;T&gt;
description: List wrapper that tracks whether changes have been made.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class DirtyTrackingList&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

List wrapper that tracks whether changes have been made.

```cs
public class DirtyTrackingList<T> : System.Collections.Generic.IList<T>, 
    System.Collections.Generic.ICollection<T>, 
    System.Collections.Generic.IEnumerable<T>, System.Collections.IEnumerable, 
    System.Collections.Generic.IReadOnlyList<T>, 
    System.Collections.Generic.IReadOnlyCollection<T>
```

### Type Parameters

**`T`**  
The type of elements in the list.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ DirtyTrackingList&lt;T&gt;

**Implements**  
[IList&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1), [ICollection&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1), [IEnumerable&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1), [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.ienumerable), [IReadOnlyList&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1), [IReadOnlyCollection&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlycollection-1)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [DirtyTrackingList&lt;T&gt;()](#dirtytrackinglistt) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [Count](#count) |  | 
| [IsDirty](#isdirty) | Whether changes have been made since the last call to [ResetDirty()](dirtytrackinglist-1.md#resetdirty). | 
| [IsReadOnly](#isreadonly) |  | 
| [Item[int]](#itemint) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Add(T)](#addt) |  | 
| [Clear()](#clear) |  | 
| [Contains(T)](#containst) |  | 
| [CopyTo(T, Int32)](#copytot-int) |  | 
| [GetEnumerator()](#getenumerator) |  | 
| [IndexOf(T)](#indexoft) |  | 
| [Insert(Int32, T)](#insertint-t) |  | 
| [Remove(T)](#removet) |  | 
| [RemoveAt(Int32)](#removeatint) |  | 
| [ResetDirty()](#resetdirty) | Resets the dirty state; [IsDirty](dirtytrackinglist-1.md#isdirty) will return `false` until another mutation occurs. | 
| [SetItems(IEnumerable&lt;T&gt;)](#setitemsienumerablet) | Replaces the entire list with the specified sequence. | 

## Details

### Constructors

#### DirtyTrackingList&lt;T&gt;()



```cs
public DirtyTrackingList<T>();
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

#### IsDirty

Whether changes have been made since the last call to [ResetDirty()](dirtytrackinglist-1.md#resetdirty).

```cs
public bool IsDirty { get; private set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### IsReadOnly



```cs
public bool IsReadOnly { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### Item[int]



```cs
public T Item[int] { get; set; }
```

##### Property Value

`T`

-----

### Methods

#### Add(T)



```cs
public void Add(T item);
```

##### Parameters

**`item`** &nbsp; T

-----

#### Clear()



```cs
public void Clear();
```

-----

#### Contains(T)



```cs
public bool Contains(T item);
```

##### Parameters

**`item`** &nbsp; T

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### CopyTo(T, int)



```cs
public void CopyTo(T array, int arrayIndex);
```

##### Parameters

**`array`** &nbsp; `T`

**`arrayIndex`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### GetEnumerator()



```cs
public System.Collections.Generic.IEnumerator<T> GetEnumerator();
```

##### Returns

[IEnumerator&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerator-1)

-----

#### IndexOf(T)



```cs
public int IndexOf(T item);
```

##### Parameters

**`item`** &nbsp; T

##### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### Insert(int, T)



```cs
public void Insert(int index, T item);
```

##### Parameters

**`index`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

**`item`** &nbsp; T

-----

#### Remove(T)



```cs
public bool Remove(T item);
```

##### Parameters

**`item`** &nbsp; T

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### RemoveAt(int)



```cs
public void RemoveAt(int index);
```

##### Parameters

**`index`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### ResetDirty()

Resets the dirty state; [IsDirty](dirtytrackinglist-1.md#isdirty) will return `false` until another mutation occurs.

```cs
public void ResetDirty();
```

-----

#### SetItems(IEnumerable&lt;T&gt;)

Replaces the entire list with the specified sequence.

```cs
public bool SetItems(System.Collections.Generic.IEnumerable<T> items);
```

##### Parameters

**`items`** &nbsp; [IEnumerable&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)  
The new list items.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

