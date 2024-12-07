---
title: Tags
description: Typesafe heterogeneous container for associating arbitrary data with a view or other UI object.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Tags

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Typesafe heterogeneous container for associating arbitrary data with a view or other UI object.

```cs
public class Tags
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Tags

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Tags()](#tags) |  | 

### Fields

 | Name | Description |
| --- | --- |
| [Empty](#empty) | Empty tags that can be used as a placeholder. | 

### Methods

 | Name | Description |
| --- | --- |
| [Create&lt;T&gt;(T)](#creatett) | Creates a new [Tags](tags.md) holding a single initial value. | 
| [Create&lt;T1, T2&gt;(T1, T2)](#createt1-t2t1-t2) | Creates a new [Tags](tags.md) holding two initial values. | 
| [Create&lt;T1, T2, T3&gt;(T1, T2, T3)](#createt1-t2-t3t1-t2-t3) | Creates a new [Tags](tags.md) holding three initial values. | 
| [Equals(Object)](#equalsobject) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`Equals(Object)`)</span> | 
| [Get&lt;T&gt;()](#gett) | Gets the tag value of the specified type, if one exists. | 
| [GetHashCode()](#gethashcode) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`GetHashCode()`)</span> | 
| [Set&lt;T&gt;(T)](#settt) | Replaces the tag value of the specified type. | 

## Details

### Constructors

#### Tags()



```cs
public Tags();
```

-----

### Fields

#### Empty

Empty tags that can be used as a placeholder.

```cs
public static readonly StardewUI.Tags Empty;
```

##### Field Value

[Tags](tags.md)

-----

### Methods

#### Create&lt;T&gt;(T)

Creates a new [Tags](tags.md) holding a single initial value.

```cs
public static StardewUI.Tags Create<T>(T value);
```

##### Parameters

**`value`** &nbsp; T  
The tag value.

##### Returns

[Tags](tags.md)

-----

#### Create&lt;T1, T2&gt;(T1, T2)

Creates a new [Tags](tags.md) holding two initial values.

```cs
public static StardewUI.Tags Create<T1, T2>(T1 value1, T2 value2);
```

##### Parameters

**`value1`** &nbsp; T1  
The first value.

**`value2`** &nbsp; T2  
The second value.

##### Returns

[Tags](tags.md)

-----

#### Create&lt;T1, T2, T3&gt;(T1, T2, T3)

Creates a new [Tags](tags.md) holding three initial values.

```cs
public static StardewUI.Tags Create<T1, T2, T3>(T1 value1, T2 value2, T3 value3);
```

##### Parameters

**`value1`** &nbsp; T1  
The first value.

**`value2`** &nbsp; T2  
The second value.

**`value3`** &nbsp; T3  
The third value.

##### Returns

[Tags](tags.md)

-----

#### Equals(Object)



```cs
public override bool Equals(System.Object obj);
```

##### Parameters

**`obj`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### Get&lt;T&gt;()

Gets the tag value of the specified type, if one exists.

```cs
public T Get<T>();
```

##### Returns

`T`

  The stored value of type `T`, if any; otherwise `null`.

-----

#### GetHashCode()



```cs
public override int GetHashCode();
```

##### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### Set&lt;T&gt;(T)

Replaces the tag value of the specified type.

```cs
public void Set<T>(T value);
```

##### Parameters

**`value`** &nbsp; T  
The new tag value.

-----

