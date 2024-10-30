---
title: Event
description: An event handler parsed from StarML.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Struct Event

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

An event handler parsed from StarML.

```cs
[System.Obsolete]
public readonly ref struct Event
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ Event

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Event(ReadOnlySpan&lt;Char&gt;, ReadOnlySpan&lt;Char&gt;, UInt32, ReadOnlySpan&lt;Char&gt;)](#eventreadonlyspanchar-readonlyspanchar-uint-readonlyspanchar) | An event handler parsed from StarML. | 

### Properties

 | Name | Description |
| --- | --- |
| [EventName](#eventname) | The name of the event to which a handler should be attached. | 
| [HandlerName](#handlername) | The name of the handler method to invoke. | 
| [ParentDepth](#parentdepth) | The depth to walk - i.e. number of parents to traverse - to find the object on which to invoke the handler method. | 
| [ParentType](#parenttype) | The type name of the parent to walk up to for a context redirect. Exclusive with [ParentDepth](event.md#parentdepth). | 

### Methods

 | Name | Description |
| --- | --- |
| [Equals(Object)](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.equals) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [GetHashCode()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.gethashcode) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [ToString()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.tostring) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 

## Details

### Constructors

#### Event(ReadOnlySpan&lt;Char&gt;, ReadOnlySpan&lt;Char&gt;, uint, ReadOnlySpan&lt;Char&gt;)

An event handler parsed from StarML.

```cs
public Event(ReadOnlySpan<System.Char> eventName, ReadOnlySpan<System.Char> handlerName, uint parentDepth, ReadOnlySpan<System.Char> parentType);
```

##### Parameters

**`eventName`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The name of the event to which a handler should be attached.

**`handlerName`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The name of the handler method to invoke.

**`parentDepth`** &nbsp; [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)  
The depth to walk - i.e. number of parents to traverse - to find the object on which to invoke the handler method.

**`parentType`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The type name of the parent to walk up to for a context redirect. Exclusive with `parentDepth`.

-----

### Properties

#### EventName

The name of the event to which a handler should be attached.

```cs
public ReadOnlySpan<System.Char> EventName { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>

-----

#### HandlerName

The name of the handler method to invoke.

```cs
public ReadOnlySpan<System.Char> HandlerName { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>

-----

#### ParentDepth

The depth to walk - i.e. number of parents to traverse - to find the object on which to invoke the handler method.

```cs
public uint ParentDepth { get; }
```

##### Property Value

[UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)

-----

#### ParentType

The type name of the parent to walk up to for a context redirect. Exclusive with [ParentDepth](event.md#parentdepth).

```cs
public ReadOnlySpan<System.Char> ParentType { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>

-----

