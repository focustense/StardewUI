---
title: ContextRedirect.Type
description: Redirects to the nearest ancestor matching a specified type.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ContextRedirect.Type

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Redirects to the nearest ancestor matching a specified type.

```cs
public record ContextRedirect.Type : StardewUI.Framework.Dom.ContextRedirect, 
    IEquatable<StardewUI.Framework.Dom.ContextRedirect.Type>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ContextRedirect](contextredirect.md) ⇦ Type

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Type](contextredirect.type.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Type(string)](#typestring) | Redirects to the nearest ancestor matching a specified type. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) | <span class="muted" markdown>(Overrides [ContextRedirect](contextredirect.md).`get_EqualityContract()`)</span> | 
| [TypeName](#typename) | The [Name](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.name) of the target ancestor's [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type). | 

## Details

### Constructors

#### Type(string)

Redirects to the nearest ancestor matching a specified type.

```cs
public Type(string TypeName);
```

##### Parameters

**`TypeName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The [Name](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.name) of the target ancestor's [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type).

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### TypeName

The [Name](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.name) of the target ancestor's [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type).

```cs
public string TypeName { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

