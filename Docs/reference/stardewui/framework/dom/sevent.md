---
title: SEvent
description: An event attribute in a StarML document.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class SEvent

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

An event attribute in a StarML document.

```cs
public record SEvent : StardewUI.Framework.Dom.IEvent, 
    IEquatable<StardewUI.Framework.Dom.SEvent>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ SEvent

**Implements**  
[IEvent](ievent.md), [IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[SEvent](sevent.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [SEvent(string, string, IReadOnlyList&lt;SArgument&gt;, ContextRedirect)](#seventstring-string-ireadonlylistsargument-contextredirect) | An event attribute in a StarML document. | 
| [SEvent(Event, IReadOnlyList&lt;SArgument&gt;)](#seventevent-ireadonlylistsargument) | Initializes a new [SArgument](sargument.md) from the data of a parsed argument. | 

### Properties

 | Name | Description |
| --- | --- |
| [Arguments](#arguments) | The arguments to the event handler. | 
| [ContextRedirect](#contextredirect) | The redirect to use for the context on which the method named `HandlerName` should exist. | 
| [EqualityContract](#equalitycontract) |  | 
| [HandlerName](#handlername) | The name of the event handler (method) to run on the bound or redirected context. | 
| [Name](#name) | The event name, i.e. name of the `event` field on the target [IView](../../iview.md). | 

## Details

### Constructors

#### SEvent(string, string, IReadOnlyList&lt;SArgument&gt;, ContextRedirect)

An event attribute in a StarML document.

```cs
public SEvent(string Name, string HandlerName, System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SArgument> Arguments, StardewUI.Framework.Dom.ContextRedirect ContextRedirect);
```

##### Parameters

**`Name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The event name, i.e. name of the `event` field on the target [IView](../../iview.md).

**`HandlerName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the event handler (method) to run on the bound or redirected context.

**`Arguments`** &nbsp; [IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SArgument](sargument.md)>  
The arguments to the event handler.

**`ContextRedirect`** &nbsp; [ContextRedirect](contextredirect.md)  
The redirect to use for the context on which the method named `HandlerName` should exist.

-----

#### SEvent(Event, IReadOnlyList&lt;SArgument&gt;)

Initializes a new [SArgument](sargument.md) from the data of a parsed argument.

```cs
public SEvent(StardewUI.Framework.Grammar.Event e, System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SArgument> arguments);
```

##### Parameters

**`e`** &nbsp; [Event](../grammar/event.md)  
The parsed event.

**`arguments`** &nbsp; [IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SArgument](sargument.md)>  
The event arguments.

-----

### Properties

#### Arguments

The arguments to the event handler.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SArgument> Arguments { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SArgument](sargument.md)>

-----

#### ContextRedirect

The redirect to use for the context on which the method named `HandlerName` should exist.

```cs
public StardewUI.Framework.Dom.ContextRedirect ContextRedirect { get; set; }
```

##### Property Value

[ContextRedirect](contextredirect.md)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### HandlerName

The name of the event handler (method) to run on the bound or redirected context.

```cs
public string HandlerName { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Name

The event name, i.e. name of the `event` field on the target [IView](../../iview.md).

```cs
public string Name { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

