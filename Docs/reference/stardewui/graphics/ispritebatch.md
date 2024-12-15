---
title: ISpriteBatch
description: Abstraction for the SpriteBatch providing sprite-drawing methods.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface ISpriteBatch

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Abstraction for the [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) providing sprite-drawing methods.

```cs
public interface ISpriteBatch : System.IDisposable
```

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Remarks

Importantly, this interface represents a "local" sprite batch with inherited transforms, so that views using it do not need to be given explicit information about global coordinates.

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Blend(BlendState)](#blendblendstate) | Sets up subsequent draw calls to use the designated blending settings. | 
| [Clip(Rectangle)](#cliprectangle) | Sets up subsequent draw calls to clip contents within the specified bounds. | 
| [DelegateDraw(Action&lt;SpriteBatch, Vector2&gt;)](#delegatedrawactionspritebatch-vector2) | Draws using a delegate action on a concrete [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html). | 
| [Draw(Texture2D, Vector2, Rectangle?, Color?, Single, Single, SpriteEffects, Single)](#drawtexture2d-vector2-rectangle-color-float-float-spriteeffects-float) |  | 
| [Draw(Texture2D, Vector2, Rectangle?, Color?, Single, Vector2?, SpriteEffects, Single)](#drawtexture2d-vector2-rectangle-color-float-vector2-spriteeffects-float) |  | 
| [Draw(Texture2D, Rectangle, Rectangle?, Color?, Single, SpriteEffects, Single)](#drawtexture2d-rectangle-rectangle-color-float-spriteeffects-float) |  | 
| [DrawString(SpriteFont, string, Vector2, Color, Single, Single, SpriteEffects, Single)](#drawstringspritefont-string-vector2-color-float-float-spriteeffects-float) |  | 
| [InitializeRenderTarget(RenderTarget2D, Int32, Int32)](#initializerendertargetrendertarget2d-int-int) | Initializes a [RenderTarget2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.RenderTarget2D.html) for use with [SetRenderTarget(RenderTarget2D, Color?)](ispritebatch.md#setrendertargetrendertarget2d-color). | 
| [Rotate(Single, TransformOrigin)](#rotatefloat-transformorigin) | Applies a rotation transformation to subsequent operations. | 
| [SaveTransform()](#savetransform) | Saves the current transform, so that it can later be restored to its current state. | 
| [Scale(Single, TransformOrigin)](#scalefloat-transformorigin) | Applies a uniform scale transformation to subsequent operations. | 
| [Scale(Single, Single, TransformOrigin)](#scalefloat-float-transformorigin) | Applies a scale transformation to subsequent operations. | 
| [Scale(Vector2, TransformOrigin)](#scalevector2-transformorigin) | Applies a scale transformation to subsequent operations. | 
| [SetRenderTarget(RenderTarget2D, Color?)](#setrendertargetrendertarget2d-color) | Sets up subsequent draw calls to use a custom render target. | 
| [Transform(Transform, TransformOrigin)](#transformtransform-transformorigin) | Applies an arbitrary transformation to subsequent operations. | 
| [Translate(Vector2)](#translatevector2) | Applies a translation transformation to subsequent operations. | 
| [Translate(Single, Single)](#translatefloat-float) | Applies a translation transformation to subsequent operations. | 

## Details

### Methods

#### Blend(BlendState)

Sets up subsequent draw calls to use the designated blending settings.

```cs
System.IDisposable Blend(Microsoft.Xna.Framework.Graphics.BlendState blendState);
```

##### Parameters

**`blendState`** &nbsp; [BlendState](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.BlendState.html)  
Blend state determining the color/alpha blend behavior.

##### Returns

[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

  A disposable instance which, when disposed, will revert to the previous blending state.

-----

#### Clip(Rectangle)

Sets up subsequent draw calls to clip contents within the specified bounds.

```cs
System.IDisposable Clip(Microsoft.Xna.Framework.Rectangle clipRect);
```

##### Parameters

**`clipRect`** &nbsp; [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)  
The clipping bounds in local coordinates.

##### Returns

[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

  A disposable instance which, when disposed, will revert to the previous clipping state.

-----

#### DelegateDraw(Action&lt;SpriteBatch, Vector2&gt;)

Draws using a delegate action on a concrete [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html).

```cs
void DelegateDraw(Action<Microsoft.Xna.Framework.Graphics.SpriteBatch, Microsoft.Xna.Framework.Vector2> draw);
```

##### Parameters

**`draw`** &nbsp; [Action](https://learn.microsoft.com/en-us/dotnet/api/system.action-2)<[SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html), [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)>  
A function that accepts an underlying [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) as well as the transformed (global/screen) position and draws using that position as the origin (top left).

##### Remarks

Delegation is provided as a fallback for game-specific "utilities" that require a [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) and are not trivial to reimplement; the method acts as a bridge between the abstract [ISpriteBatch](ispritebatch.md) and the concrete-dependent logic. 

 Most view types shouldn't use this; it is only needed for a few niche features like SpriteText.

-----

#### Draw(Texture2D, Vector2, Rectangle?, Color?, float, float, SpriteEffects, float)



```cs
void Draw(Microsoft.Xna.Framework.Graphics.Texture2D texture, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Rectangle? sourceRectangle, Microsoft.Xna.Framework.Color? color, float rotation, float scale, Microsoft.Xna.Framework.Graphics.SpriteEffects effects, float layerDepth);
```

##### Parameters

**`texture`** &nbsp; [Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html)

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

**`sourceRectangle`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)>

**`color`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)>

**`rotation`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

**`scale`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

**`effects`** &nbsp; [SpriteEffects](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteEffects.html)

**`layerDepth`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

#### Draw(Texture2D, Vector2, Rectangle?, Color?, float, Vector2?, SpriteEffects, float)



```cs
void Draw(Microsoft.Xna.Framework.Graphics.Texture2D texture, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Rectangle? sourceRectangle, Microsoft.Xna.Framework.Color? color, float rotation, Microsoft.Xna.Framework.Vector2? scale, Microsoft.Xna.Framework.Graphics.SpriteEffects effects, float layerDepth);
```

##### Parameters

**`texture`** &nbsp; [Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html)

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

**`sourceRectangle`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)>

**`color`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)>

**`rotation`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

**`scale`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)>

**`effects`** &nbsp; [SpriteEffects](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteEffects.html)

**`layerDepth`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

#### Draw(Texture2D, Rectangle, Rectangle?, Color?, float, SpriteEffects, float)



```cs
void Draw(Microsoft.Xna.Framework.Graphics.Texture2D texture, Microsoft.Xna.Framework.Rectangle destinationRectangle, Microsoft.Xna.Framework.Rectangle? sourceRectangle, Microsoft.Xna.Framework.Color? color, float rotation, Microsoft.Xna.Framework.Graphics.SpriteEffects effects, float layerDepth);
```

##### Parameters

**`texture`** &nbsp; [Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html)

**`destinationRectangle`** &nbsp; [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)

**`sourceRectangle`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)>

**`color`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)>

**`rotation`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

**`effects`** &nbsp; [SpriteEffects](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteEffects.html)

**`layerDepth`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

#### DrawString(SpriteFont, string, Vector2, Color, float, float, SpriteEffects, float)



```cs
void DrawString(Microsoft.Xna.Framework.Graphics.SpriteFont spriteFont, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, float rotation, float scale, Microsoft.Xna.Framework.Graphics.SpriteEffects effects, float layerDepth);
```

##### Parameters

**`spriteFont`** &nbsp; [SpriteFont](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteFont.html)

**`text`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

**`color`** &nbsp; [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)

**`rotation`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

**`scale`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

**`effects`** &nbsp; [SpriteEffects](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteEffects.html)

**`layerDepth`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

#### InitializeRenderTarget(RenderTarget2D, int, int)

Initializes a [RenderTarget2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.RenderTarget2D.html) for use with [SetRenderTarget(RenderTarget2D, Color?)](ispritebatch.md#setrendertargetrendertarget2d-color).

```cs
void InitializeRenderTarget(Microsoft.Xna.Framework.Graphics.RenderTarget2D target, int width, int height);
```

##### Parameters

**`target`** &nbsp; [RenderTarget2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.RenderTarget2D.html)  
The previous render target, if any, to reuse if possible.

**`width`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The target width.

**`height`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The target height.

##### Remarks

This will reuse an existing render target if available, i.e. if `target` is not `null` and matches the specified `width` and `height`; otherwise it will replace any previous `target` and replace it with a new instance.

-----

#### Rotate(float, TransformOrigin)

Applies a rotation transformation to subsequent operations.

```cs
void Rotate(float angle, StardewUI.Graphics.TransformOrigin origin);
```

##### Parameters

**`angle`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The rotation angle, in radians.

**`origin`** &nbsp; [TransformOrigin](transformorigin.md)  
The center of the rotation, or `null` to use the [Default](transformorigin.md#default) origin.

-----

#### SaveTransform()

Saves the current transform, so that it can later be restored to its current state.

```cs
System.IDisposable SaveTransform();
```

##### Returns

[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

  A disposable instance which, when disposed, restores the transform of this [ISpriteBatch](ispritebatch.md) to the same state it was in before `SaveTransform` was called.

##### Remarks

This is typically used in hierarchical layout; i.e. a view with children would apply a transform before handing the canvas or sprite batch down to any of those children, and then restore it after the child is done with it. This enables a single [ISpriteBatch](ispritebatch.md) instance to be used for the entire layout rather than having to create a tree.

-----

#### Scale(float, TransformOrigin)

Applies a uniform scale transformation to subsequent operations.

```cs
void Scale(float scale, StardewUI.Graphics.TransformOrigin origin);
```

##### Parameters

**`scale`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
Amount to scale, both horizontally and vertically. `1` is unity scale.

**`origin`** &nbsp; [TransformOrigin](transformorigin.md)  
The center of the scaling, or `null` to use the [Default](transformorigin.md#default) origin.

-----

#### Scale(float, float, TransformOrigin)

Applies a scale transformation to subsequent operations.

```cs
void Scale(float x, float y, StardewUI.Graphics.TransformOrigin origin);
```

##### Parameters

**`x`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
Amount of horizontal scaling. `1` is unity scale.

**`y`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
Amount of vertical scaling. `1` is unity scale.

**`origin`** &nbsp; [TransformOrigin](transformorigin.md)  
The center of the scaling, or `null` to use the [Default](transformorigin.md#default) origin.

-----

#### Scale(Vector2, TransformOrigin)

Applies a scale transformation to subsequent operations.

```cs
void Scale(Microsoft.Xna.Framework.Vector2 scale, StardewUI.Graphics.TransformOrigin origin);
```

##### Parameters

**`scale`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
Scaling vector containing the horizontal ([X](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2)) and vertical ([Y](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2)) scaling amounts.

**`origin`** &nbsp; [TransformOrigin](transformorigin.md)  
The center of the scaling, or `null` to use the [Default](transformorigin.md#default) origin.

-----

#### SetRenderTarget(RenderTarget2D, Color?)

Sets up subsequent draw calls to use a custom render target.

```cs
System.IDisposable SetRenderTarget(Microsoft.Xna.Framework.Graphics.RenderTarget2D renderTarget, Microsoft.Xna.Framework.Color? clearColor);
```

##### Parameters

**`renderTarget`** &nbsp; [RenderTarget2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.RenderTarget2D.html)  
The new render target.

**`clearColor`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)>  
Color to clear the `renderTarget` with after making it active, or `null` to skip clearing.

##### Returns

[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

  A disposable instance which, when disposed, will revert to the previous render target(s).

##### Remarks

This will also reset any active transforms for the new render target, e.g. those resulting from [Translate(Vector2)](ispritebatch.md#translatevector2). Previously-active transforms will be restored when the render target is reverted by calling [Dispose()](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable.dispose) on the result.

-----

#### Transform(Transform, TransformOrigin)

Applies an arbitrary transformation to subsequent operations.

```cs
void Transform(StardewUI.Graphics.Transform transform, StardewUI.Graphics.TransformOrigin origin);
```

##### Parameters

**`transform`** &nbsp; [Transform](transform.md)  
The transform properties (scale, rotation and translation).

**`origin`** &nbsp; [TransformOrigin](transformorigin.md)  
The origin (i.e. center) of the transformation, or `null` to use the [Default](transformorigin.md#default) origin.

-----

#### Translate(Vector2)

Applies a translation transformation to subsequent operations.

```cs
void Translate(Microsoft.Xna.Framework.Vector2 translation);
```

##### Parameters

**`translation`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The translation vector.

-----

#### Translate(float, float)

Applies a translation transformation to subsequent operations.

```cs
void Translate(float x, float y);
```

##### Parameters

**`x`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The translation's X component.

**`y`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The translation's Y component.

-----

