---
title: FlagEventArgs
description: Arguments for events relating to a flag included in an IViewState.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class FlagEventArgs

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Arguments for events relating to a flag included in an [IViewState](iviewstate.md).

```cs
public class FlagEventArgs : System.EventArgs
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs) ⇦ FlagEventArgs

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [FlagEventArgs(string)](#flageventargsstring) | Arguments for events relating to a flag included in an [IViewState](iviewstate.md). | 

### Properties

 | Name | Description |
| --- | --- |
| [Name](#name) | The name of the affected flag. | 

## Details

### Constructors

#### FlagEventArgs(string)

Arguments for events relating to a flag included in an [IViewState](iviewstate.md).

```cs
public FlagEventArgs(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the affected flag.

-----

### Properties

#### Name

The name of the affected flag.

```cs
public string Name { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

