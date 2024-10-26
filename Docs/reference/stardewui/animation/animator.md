---
title: Animator
description: Helpers for creating typed Animator&lt;T, V&gt; instances.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Animator

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Animation](index.md)  
Assembly: StardewUI.dll  

</div>

Helpers for creating typed [Animator&lt;T, V&gt;](animator-2.md) instances.

```cs
public static class Animator
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Animator

## Members

### Methods

 | Name | Description |
| --- | --- |
| [On&lt;T, V&gt;(T, Func&lt;T, V&gt;, Lerp&lt;V&gt;, Action&lt;T, V&gt;)](#ont-vt-funct-v-lerpv-actiont-v) | Creates a new [Animator&lt;T, V&gt;](animator-2.md). | 
| [On&lt;T&gt;(T, Func&lt;T, Single&gt;, Action&lt;T, Single&gt;)](#ontt-funct-single-actiont-single) | Creates a new [Animator&lt;T, V&gt;](animator-2.md) that animates a standard [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) property. | 

## Details

### Methods

#### On&lt;T, V&gt;(T, Func&lt;T, V&gt;, Lerp&lt;V&gt;, Action&lt;T, V&gt;)

Creates a new [Animator&lt;T, V&gt;](animator-2.md).

```cs
public static StardewUI.Animation.Animator<T, V> On<T, V>(T target, Func<T, V> getValue, StardewUI.Animation.Lerp<V> lerpValue, Action<T, V> setValue);
```

##### Parameters

**`target`** &nbsp; T  
The object whose property will be animated.

**`getValue`** &nbsp; [Func&lt;T, V&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2)  
Function to get the current value. Used for animations that don't explicit specify a start value, e.g. when using the [Start(Animation&lt;V&gt;)](animator-2.md#startanimationv) overload.

**`lerpValue`** &nbsp; [Lerp&lt;V&gt;](lerp-1.md)  
Function to linearly interpolate between the start and end values.

**`setValue`** &nbsp; [Action&lt;T, V&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2)  
Delegate to set the value on the `target`.

##### Returns

[Animator&lt;T, V&gt;](animator-2.md)

##### Remarks

Calling this is the same as calling the constructor, but typically does not require explicit type arguments.

-----

#### On&lt;T&gt;(T, Func&lt;T, Single&gt;, Action&lt;T, Single&gt;)

Creates a new [Animator&lt;T, V&gt;](animator-2.md) that animates a standard [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) property.

```cs
public static StardewUI.Animation.Animator<T, System.Single> On<T>(T target, Func<T, System.Single> getValue, Action<T, System.Single> setValue);
```

##### Parameters

**`target`** &nbsp; T  
The object whose property will be animated.

**`getValue`** &nbsp; [Func&lt;T, Single&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2)  
Function to get the current value. Used for animations that don't explicit specify a start value, e.g. when using the [Start(Animation&lt;V&gt;)](animator-2.md#startanimationv) overload.

**`setValue`** &nbsp; [Action&lt;T, Single&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2)  
Delegate to set the value on the `target`.

##### Returns

[Animator&lt;T, Single&gt;](animator-2.md)

##### Remarks

Calling this is the same as calling the constructor, but typically does not require explicit type arguments.

-----

