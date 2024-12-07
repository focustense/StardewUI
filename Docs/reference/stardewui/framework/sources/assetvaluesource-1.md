---
title: AssetValueSource&lt;T&gt;
description: Value source that looks up an asset registered with SMAPI's content manager.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class AssetValueSource&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Sources](index.md)  
Assembly: StardewUI.dll  

</div>

Value source that looks up an asset registered with SMAPI's content manager.

```cs
public class AssetValueSource<T> : StardewUI.Framework.Sources.IValueSource<T>, 
    StardewUI.Framework.Sources.IValueSource, System.IDisposable
```

### Type Parameters

**`T`**  
The type of asset to retrieve.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ AssetValueSource&lt;T&gt;

**Implements**  
[IValueSource&lt;T&gt;](ivaluesource-1.md), [IValueSource](ivaluesource.md), [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [AssetValueSource&lt;T&gt;(IAssetCache, string)](#assetvaluesourcetiassetcache-string) | Value source that looks up an asset registered with SMAPI's content manager. | 

### Properties

 | Name | Description |
| --- | --- |
| [CanRead](#canread) | Whether or not the source can be read from, i.e. if an attempt to **get** the [Value](ivaluesource.md#value) should succeed. | 
| [CanWrite](#canwrite) | Whether or not the source can be written back to, i.e. if an attempt to **set** the [Value](ivaluesource.md#value) should succeed. | 
| [DisplayName](#displayname) | Descriptive name for the property, used primarily for debug views and log/exception messages. | 
| [Value](#value) |  | 
| [ValueType](#valuetype) | The compile-time type of the value tracked by this source; the type parameter for [IValueSource&lt;T&gt;](ivaluesource-1.md). | 

### Methods

 | Name | Description |
| --- | --- |
| [Dispose()](#dispose) |  | 
| [Update(Boolean)](#updatebool) | Checks if the value needs updating, and if so, updates [Value](ivaluesource.md#value) to the latest. | 

## Details

### Constructors

#### AssetValueSource&lt;T&gt;(IAssetCache, string)

Value source that looks up an asset registered with SMAPI's content manager.

```cs
public AssetValueSource<T>(StardewUI.Framework.Content.IAssetCache cache, string name);
```

##### Parameters

**`cache`** &nbsp; [IAssetCache](../content/iassetcache.md)  
Asset cache used to obtain current value/status.

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The asset name/path as it would be supplied to SMAPI in Load&lt;T&gt;(string).

-----

### Properties

#### CanRead

Whether or not the source can be read from, i.e. if an attempt to **get** the [Value](ivaluesource.md#value) should succeed.

```cs
public bool CanRead { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### CanWrite

Whether or not the source can be written back to, i.e. if an attempt to **set** the [Value](ivaluesource.md#value) should succeed.

```cs
public bool CanWrite { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### DisplayName

Descriptive name for the property, used primarily for debug views and log/exception messages.

```cs
public string DisplayName { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Value



```cs
public T Value { get; set; }
```

##### Property Value

`T`

-----

#### ValueType

The compile-time type of the value tracked by this source; the type parameter for [IValueSource&lt;T&gt;](ivaluesource-1.md).

```cs
public System.Type ValueType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

### Methods

#### Dispose()



```cs
public void Dispose();
```

-----

#### Update(bool)

Checks if the value needs updating, and if so, updates [Value](ivaluesource.md#value) to the latest.

```cs
public bool Update(bool force);
```

##### Parameters

**`force`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
If `true`, forces the source to update its value even if it isn't considered dirty. This should never be used in a regular binding, but can be useful in sources that are intended for occasional or one-shot use such as event handler arguments.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the underlying asset expired since the last update; `false` if the [Value](assetvaluesource-1.md#value) was still current.

##### Remarks

This method is called every frame, for every binding, and providing a correct return value is essential in order to avoid slowdowns due to unnecessary rebinds.

-----

