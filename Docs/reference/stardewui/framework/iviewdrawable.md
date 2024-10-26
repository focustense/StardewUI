---
title: IViewDrawable
description: Provides methods to update and draw a simple, non-interactive UI component, such as a HUD widget.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IViewDrawable

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework](index.md)  
Assembly: StardewUI.dll  

</div>

Provides methods to update and draw a simple, non-interactive UI component, such as a HUD widget.

```cs
public interface IViewDrawable : System.IDisposable
```

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Properties

 | Name | Description |
| --- | --- |
| [ActualSize](#actualsize) | The current size required for the content. | 
| [Context](#context) | The context, or "model", for the menu's view, which holds any data-dependent values. | 
| [MaxSize](#maxsize) | The maximum size, in pixels, allowed for this content. | 

### Methods

 | Name | Description |
| --- | --- |
| [Draw(SpriteBatch, Vector2)](#drawspritebatch-vector2) | Draws the current contents. | 

## Details

### Properties

#### ActualSize

The current size required for the content.

```cs
Microsoft.Xna.Framework.Vector2 ActualSize { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

##### Remarks

Use for calculating the correct position for a [Draw(SpriteBatch, Vector2)](iviewdrawable.md#drawspritebatch-vector2), especially for elements that should be aligned to the center or right edge of the viewport.

-----

#### Context

The context, or "model", for the menu's view, which holds any data-dependent values.

```cs
System.Object Context { get; set; }
```

##### Property Value

[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)

##### Remarks

The type must implement [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) in order for any changes to this data to be automatically reflected on the next [Draw(SpriteBatch, Vector2)](iviewdrawable.md#drawspritebatch-vector2).

-----

#### MaxSize

The maximum size, in pixels, allowed for this content.

```cs
Microsoft.Xna.Framework.Vector2? MaxSize { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)>

##### Remarks

If no value is specified, then the content is allowed to use the entire uiViewport.

-----

### Methods

#### Draw(SpriteBatch, Vector2)

Draws the current contents.

```cs
void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch b, Microsoft.Xna.Framework.Vector2 position);
```

##### Parameters

**`b`** &nbsp; [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html)  
Target sprite batch.

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
Position on the screen or viewport to use as the top-left corner.

-----

