---
title: Frame
description: Represents a single captured frame, or slice.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Frame

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Diagnostics](index.md)  
Assembly: StardewUI.dll  

</div>

Represents a single captured frame, or slice.

```cs
public record Frame : IEquatable<StardewUI.Framework.Diagnostics.Frame>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Frame

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Frame](frame.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Frame(string)](#framestring) | Represents a single captured frame, or slice. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [Name](#name) | Name of the method or operation that was measured. | 

## Details

### Constructors

#### Frame(string)

Represents a single captured frame, or slice.

```cs
public Frame(string Name);
```

##### Parameters

**`Name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Name of the method or operation that was measured.

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Name

Name of the method or operation that was measured.

```cs
public string Name { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

