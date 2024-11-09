---
title: SelectionGroup
description: Provides a single selection key with change notifications.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class SelectionGroup

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Provides a single selection key with change notifications.

```cs
public class SelectionGroup
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ SelectionGroup

## Remarks

Can be used to group together UI widgets so that only one at a time can be active, e.g. in a tab or radio group.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [SelectionGroup()](#selectiongroup) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [Key](#key) | The currently-selected key. | 

### Events

 | Name | Description |
| --- | --- |
| [Change](#change) | Raised when the [Key](selectiongroup.md#key) changes. | 

## Details

### Constructors

#### SelectionGroup()



```cs
public SelectionGroup();
```

-----

### Properties

#### Key

The currently-selected key.

```cs
public string Key { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

### Events

#### Change

Raised when the [Key](selectiongroup.md#key) changes.

```cs
public event System.EventHandler? Change;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler)

-----

