---
title: Animation&lt;T&gt;
description: Defines a single animation.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Animation&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Animation](index.md)  
Assembly: StardewUI.dll  

</div>

Defines a single animation.

```cs
public record Animation<T> : IEquatable<StardewUI.Animation.Animation<T>>
```

### Type Parameters

**`T`**  
The type of value being animated.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Animation&lt;T&gt;

**Implements**  
[IEquatable&lt;Animation&lt;T&gt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Animation&lt;T&gt;(T, T, TimeSpan)](#animationtt-t-timespan) | Defines a single animation. | 

### Properties

 | Name | Description |
| --- | --- |
| [Duration](#duration) | Duration of the animation. | 
| [EndValue](#endvalue) | The final value for the animated property. | 
| [EqualityContract](#equalitycontract) |  | 
| [StartValue](#startvalue) | The initial value for the animated property. | 

## Details

### Constructors

#### Animation&lt;T&gt;(T, T, TimeSpan)

Defines a single animation.

```cs
public Animation<T>(T StartValue, T EndValue, System.TimeSpan Duration);
```

##### Parameters

**`StartValue`** &nbsp; T  
The initial value for the animated property.

**`EndValue`** &nbsp; T  
The final value for the animated property.

**`Duration`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Duration of the animation.

-----

### Properties

#### Duration

Duration of the animation.

```cs
public System.TimeSpan Duration { get; set; }
```

##### Property Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

#### EndValue

The final value for the animated property.

```cs
public T EndValue { get; set; }
```

##### Property Value

`T`

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### StartValue

The initial value for the animated property.

```cs
public T StartValue { get; set; }
```

##### Property Value

`T`

-----

