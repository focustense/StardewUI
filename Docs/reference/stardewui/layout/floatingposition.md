---
title: FloatingPosition
description: Describes the position of a FloatingElement.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class FloatingPosition

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Describes the position of a [FloatingElement](floatingelement.md).

```cs
public class FloatingPosition
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ FloatingPosition

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [FloatingPosition(Func&lt;Vector2, Vector2, Vector2&gt;)](#floatingpositionfuncvector2-vector2-vector2) | Describes the position of a [FloatingElement](floatingelement.md). | 

### Fields

 | Name | Description |
| --- | --- |
| [AboveParent](#aboveparent) | Positions the floating element immediately above the parent view, so that its bottom edge is flush with the parent's top edge. | 
| [AfterParent](#afterparent) | Positions the floating element immediately to the right of (after) the parent view, so that its left edge is flush with the parent's right edge. | 
| [BeforeParent](#beforeparent) | Positions the floating element immediately to the left of (before) the parent view, so that its right edge is flush with the parent's left edge. | 
| [BelowParent](#belowparent) | Positions the floating element immediately below the parent view, so that its top edge is flush with the parent's bottom edge. | 

### Methods

 | Name | Description |
| --- | --- |
| [GetOffset(IView, View)](#getoffsetiview-view) | Calculates the final position of the floating view. | 

## Details

### Constructors

#### FloatingPosition(Func&lt;Vector2, Vector2, Vector2&gt;)

Describes the position of a [FloatingElement](floatingelement.md).

```cs
public FloatingPosition(Func<Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2> offsetSelector);
```

##### Parameters

**`offsetSelector`** &nbsp; [Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-3)<[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html), [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html), [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)>  
Calculates the position offset (relative to the parent) of the floating view. Takes the measured floating view size, and then the parent size, as arguments.

-----

### Fields

#### AboveParent

Positions the floating element immediately above the parent view, so that its bottom edge is flush with the parent's top edge.

```cs
public static readonly StardewUI.Layout.FloatingPosition AboveParent;
```

##### Field Value

[FloatingPosition](floatingposition.md)

-----

#### AfterParent

Positions the floating element immediately to the right of (after) the parent view, so that its left edge is flush with the parent's right edge.

```cs
public static readonly StardewUI.Layout.FloatingPosition AfterParent;
```

##### Field Value

[FloatingPosition](floatingposition.md)

-----

#### BeforeParent

Positions the floating element immediately to the left of (before) the parent view, so that its right edge is flush with the parent's left edge.

```cs
public static readonly StardewUI.Layout.FloatingPosition BeforeParent;
```

##### Field Value

[FloatingPosition](floatingposition.md)

-----

#### BelowParent

Positions the floating element immediately below the parent view, so that its top edge is flush with the parent's bottom edge.

```cs
public static readonly StardewUI.Layout.FloatingPosition BelowParent;
```

##### Field Value

[FloatingPosition](floatingposition.md)

-----

### Methods

#### GetOffset(IView, View)

Calculates the final position of the floating view.

```cs
public Microsoft.Xna.Framework.Vector2 GetOffset(StardewUI.IView view, StardewUI.View parentView);
```

##### Parameters

**`view`** &nbsp; [IView](../iview.md)  
The floating view to position.

**`parentView`** &nbsp; [View](../view.md)  
The parent relative to which the floating view is being positioned.

##### Returns

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

  The final position where the `view` should be drawn.

-----

