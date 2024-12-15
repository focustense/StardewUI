---
title: RenderTargetPool
description: Pools RenderTarget2D instances so they can be reused across multiple frames.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class RenderTargetPool

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Pools [RenderTarget2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.RenderTarget2D.html) instances so they can be reused across multiple frames.

```cs
public class RenderTargetPool : System.IDisposable
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ RenderTargetPool

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Remarks

Targets are pooled by their size, since render targets have a fixed size and changing the size requires recreating the target. The pooled targets are considered to be managed by the pool, and are disposed along with the pool itself; typically a pool is associated with some long-lived UI object such as a menu, and then assigned to a transient instance like [PropagatedSpriteBatch](propagatedspritebatch.md). 

 Pools can be configured with a `slack` in order to increase long-term reuse at the expense of higher transient memory and/or VRAM usage due to the extra targets; these may be several megabytes if the areas to be captured are large. Slack can help accommodate dynamic views, e.g. different tabs with different scroll sizes, but should be used conservatively to avoid keeping long-dead targets. 

 The pool is effectively unbounded in the number of instances it can create, but once slack is exceeded, it will dispose an old instance before creating a new one, starting with the instance having the largest pixel size (i.e. taking up the most memory).

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [RenderTargetPool(GraphicsDevice, Int32)](#rendertargetpoolgraphicsdevice-int) | Pools [RenderTarget2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.RenderTarget2D.html) instances so they can be reused across multiple frames. | 

### Methods

 | Name | Description |
| --- | --- |
| [Acquire(Int32, Int32, RenderTarget2D)](#acquireint-int-rendertarget2d) | Obtains a pooled target with the specified dimensions, or creates a new target if there is no usable pooled instance. | 
| [Dispose()](#dispose) |  | 

## Details

### Constructors

#### RenderTargetPool(GraphicsDevice, int)

Pools [RenderTarget2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.RenderTarget2D.html) instances so they can be reused across multiple frames.

```cs
public RenderTargetPool(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int slack);
```

##### Parameters

**`graphicsDevice`** &nbsp; [GraphicsDevice](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html)  
The graphics device used for rendering.

**`slack`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Specifies the maximum number of unused pooled render targets to keep, when requesting a new target whose size is not in the pool, before disposing an older target.

##### Remarks

Targets are pooled by their size, since render targets have a fixed size and changing the size requires recreating the target. The pooled targets are considered to be managed by the pool, and are disposed along with the pool itself; typically a pool is associated with some long-lived UI object such as a menu, and then assigned to a transient instance like [PropagatedSpriteBatch](propagatedspritebatch.md). 

 Pools can be configured with a `slack` in order to increase long-term reuse at the expense of higher transient memory and/or VRAM usage due to the extra targets; these may be several megabytes if the areas to be captured are large. Slack can help accommodate dynamic views, e.g. different tabs with different scroll sizes, but should be used conservatively to avoid keeping long-dead targets. 

 The pool is effectively unbounded in the number of instances it can create, but once slack is exceeded, it will dispose an old instance before creating a new one, starting with the instance having the largest pixel size (i.e. taking up the most memory).

-----

### Methods

#### Acquire(int, int, RenderTarget2D)

Obtains a pooled target with the specified dimensions, or creates a new target if there is no usable pooled instance.

```cs
public System.IDisposable Acquire(int width, int height, out Microsoft.Xna.Framework.Graphics.RenderTarget2D target);
```

##### Parameters

**`width`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The target's pixel width.

**`height`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The target's pixel height.

**`target`** &nbsp; [RenderTarget2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.RenderTarget2D.html)  
Receives the pooled or created [RenderTarget2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.RenderTarget2D.html) which has the specified `width` and `height`.

##### Returns

[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

  An [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable) instance which, when disposed, will release the `target` back to the pool, without disposing the target itself.

-----

#### Dispose()



```cs
public void Dispose();
```

-----

