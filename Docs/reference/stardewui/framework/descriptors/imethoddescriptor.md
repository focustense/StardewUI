---
title: IMethodDescriptor
description: Describes a single method on some type.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IMethodDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Describes a single method on some type.

```cs
public interface IMethodDescriptor : 
    StardewUI.Framework.Descriptors.IMemberDescriptor
```

**Implements**  
[IMemberDescriptor](imemberdescriptor.md)

## Members

### Properties

 | Name | Description |
| --- | --- |
| [ArgumentTypes](#argumenttypes) | The exact types expected for the method's arguments. | 
| [OptionalArgumentCount](#optionalargumentcount) | The number of optional arguments at the end of the argument list. | 
| [ReturnType](#returntype) | The method's return type. | 

## Details

### Properties

#### ArgumentTypes

The exact types expected for the method's arguments.

```cs
ReadOnlySpan<System.Type> ArgumentTypes { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)>

-----

#### OptionalArgumentCount

The number of optional arguments at the end of the argument list.

```cs
int OptionalArgumentCount { get; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

##### Remarks

Optional arguments can be provided with [Missing](https://learn.microsoft.com/en-us/dotnet/api/system.type.missing) in order to ignore them in the invocation.

-----

#### ReturnType

The method's return type.

```cs
System.Type ReturnType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

