---
title: IEvent
description: Event wire-up in a StarML element.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IEvent

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Event wire-up in a StarML element.

```cs
public interface IEvent
```

## Members

### Properties

 | Name | Description |
| --- | --- |
| [Arguments](#arguments) | The arguments to the event handler. | 
| [ContextRedirect](#contextredirect) | Specifies the redirect to use for the context on which the method named [HandlerName](ievent.md#handlername) should exist. | 
| [HandlerName](#handlername) | The name of the event handler (method) to run on the bound or redirected context. | 
| [Name](#name) | The event name, i.e. name of the `event` field on the target [IView](../../iview.md). | 

### Methods

 | Name | Description |
| --- | --- |
| [Print(StringBuilder)](#printstringbuilder) | Prints the textual representation of this event attribute. | 

## Details

### Properties

#### Arguments

The arguments to the event handler.

```cs
System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.IArgument> Arguments { get; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[IArgument](iargument.md)>

-----

#### ContextRedirect

Specifies the redirect to use for the context on which the method named [HandlerName](ievent.md#handlername) should exist.

```cs
StardewUI.Framework.Dom.ContextRedirect ContextRedirect { get; }
```

##### Property Value

[ContextRedirect](contextredirect.md)

##### Remarks

Applies to the handler method itself but **not** any of the [Arguments](ievent.md#arguments), which specify their own redirects when applicable.

-----

#### HandlerName

The name of the event handler (method) to run on the bound or redirected context.

```cs
string HandlerName { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Name

The event name, i.e. name of the `event` field on the target [IView](../../iview.md).

```cs
string Name { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

### Methods

#### Print(StringBuilder)

Prints the textual representation of this event attribute.

```cs
void Print(System.Text.StringBuilder sb);
```

##### Parameters

**`sb`** &nbsp; [StringBuilder](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder)  
Builder to receive the attribute's text output.

-----

