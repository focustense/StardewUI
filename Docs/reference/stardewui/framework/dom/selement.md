---
title: SElement
description: Record implementation of a StarML IElement.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class SElement

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Record implementation of a StarML [IElement](ielement.md).

```cs
public record SElement : StardewUI.Framework.Dom.IElement, 
    IEquatable<StardewUI.Framework.Dom.SElement>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ SElement

**Implements**  
[IElement](ielement.md), [IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[SElement](selement.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [SElement(string, IReadOnlyList&lt;SAttribute&gt;, IReadOnlyList&lt;SEvent&gt;)](#selementstring-ireadonlylistsattribute-ireadonlylistsevent) | Record implementation of a StarML [IElement](ielement.md). | 

### Properties

 | Name | Description |
| --- | --- |
| [Attributes](#attributes) | The attributes applied to this tag. | 
| [EqualityContract](#equalitycontract) |  | 
| [Events](#events) | The events applied to this tag. | 
| [Tag](#tag) | The tag name. | 

## Details

### Constructors

#### SElement(string, IReadOnlyList&lt;SAttribute&gt;, IReadOnlyList&lt;SEvent&gt;)

Record implementation of a StarML [IElement](ielement.md).

```cs
public SElement(string Tag, System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SAttribute> Attributes, System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SEvent> Events);
```

##### Parameters

**`Tag`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The tag name.

**`Attributes`** &nbsp; [IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SAttribute](sattribute.md)>  
The attributes applied to this tag.

**`Events`** &nbsp; [IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SEvent](sevent.md)>  
The events applied to this tag.

-----

### Properties

#### Attributes

The attributes applied to this tag.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SAttribute> Attributes { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SAttribute](sattribute.md)>

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Events

The events applied to this tag.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SEvent> Events { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SEvent](sevent.md)>

-----

#### Tag

The tag name.

```cs
public string Tag { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

