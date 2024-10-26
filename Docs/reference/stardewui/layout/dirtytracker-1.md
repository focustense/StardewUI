---
title: DirtyTracker&lt;T&gt;
description: Convenience class for tracking properties that have changed, i.e. for layout dirty checking.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class DirtyTracker&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Convenience class for tracking properties that have changed, i.e. for layout dirty checking.

```cs
public class DirtyTracker<T>
```

### Type Parameters

**`T`**  
Type of value held by the tracker.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ DirtyTracker&lt;T&gt;

## Remarks

Will not flag changes as dirty unless the changed value is different from previous. Requires a correct `Equals(Object)` implementation for this to work, typically meaning strings, primitives and records.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [DirtyTracker&lt;T&gt;(T)](#dirtytrackertt) | Convenience class for tracking properties that have changed, i.e. for layout dirty checking. | 

### Properties

 | Name | Description |
| --- | --- |
| [IsDirty](#isdirty) | Whether or not the value is dirty, i.e. has changed since the last call to [ResetDirty()](dirtytracker-1.md#resetdirty). | 
| [Value](#value) | The currently-held value. | 

### Methods

 | Name | Description |
| --- | --- |
| [ResetDirty()](#resetdirty) | Resets the dirty flag, so that [IsDirty](dirtytracker-1.md#isdirty) returns `false` until the [Value](dirtytracker-1.md#value) is changed again. | 
| [SetIfChanged(T)](#setifchangedt) | Updates the tracker with a new value, if it has changed since the last seen value. | 

## Details

### Constructors

#### DirtyTracker&lt;T&gt;(T)

Convenience class for tracking properties that have changed, i.e. for layout dirty checking.

```cs
public DirtyTracker<T>(T initialValue);
```

##### Parameters

**`initialValue`** &nbsp; T  
Value to initialize with.

##### Remarks

Will not flag changes as dirty unless the changed value is different from previous. Requires a correct `Equals(Object)` implementation for this to work, typically meaning strings, primitives and records.

-----

### Properties

#### IsDirty

Whether or not the value is dirty, i.e. has changed since the last call to [ResetDirty()](dirtytracker-1.md#resetdirty).

```cs
public bool IsDirty { get; private set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### Value

The currently-held value.

```cs
public T Value { get; set; }
```

##### Property Value

`T`

-----

### Methods

#### ResetDirty()

Resets the dirty flag, so that [IsDirty](dirtytracker-1.md#isdirty) returns `false` until the [Value](dirtytracker-1.md#value) is changed again.

```cs
public void ResetDirty();
```

-----

#### SetIfChanged(T)

Updates the tracker with a new value, if it has changed since the last seen value.

```cs
public bool SetIfChanged(T value);
```

##### Parameters

**`value`** &nbsp; T  
The new value.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the `value` was different from the previous [Value](dirtytracker-1.md#value), otherwise `false`.

##### Remarks

If this method returns `true`, then [IsDirty](dirtytracker-1.md#isdirty) will always also be `true` afterward. However, if it returns `false` then the dirty state simply remains unchanged, and will only be `false` if the value was not already dirty from a previous change.

-----

