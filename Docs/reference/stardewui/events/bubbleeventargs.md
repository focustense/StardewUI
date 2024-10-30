---
title: BubbleEventArgs
description: Base class for events that can bubble up to parents from descendant views.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BubbleEventArgs

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Events](index.md)  
Assembly: StardewUI.dll  

</div>

Base class for events that can bubble up to parents from descendant views.

```cs
public class BubbleEventArgs : System.EventArgs
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs) ⇦ BubbleEventArgs

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [BubbleEventArgs()](#bubbleeventargs) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [Handled](#handled) | Whether or not the view receiving the event handled the event. Set to `true` to prevent bubbling. | 

## Details

### Constructors

#### BubbleEventArgs()



```cs
public BubbleEventArgs();
```

-----

### Properties

#### Handled

Whether or not the view receiving the event handled the event. Set to `true` to prevent bubbling.

```cs
public bool Handled { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

