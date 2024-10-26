---
title: DecoratorView&lt;T&gt;.DecoratedProperty&lt;T, TValue&gt;
description: Helper for propagating a single property to and from the inner view.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class DecoratorView&lt;T&gt;.DecoratedProperty&lt;T, TValue&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Helper for propagating a single property to and from the inner view.

```cs
protected class DecoratorView<T>.DecoratedProperty<T, TValue>
```

### Type Parameters

**`T`**  

**`TValue`**  
The type of value tracked.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ DecoratedProperty&lt;T, TValue&gt;

## Remarks

Decorated properties must be initialized in the decorator's constructor by calling [RegisterDecoratedProperty&lt;TValue&gt;(DecoratedProperty&lt;T, TValue&gt;)](decoratorview-1.md#registerdecoratedpropertytvaluedecoratedpropertyt-tvalue), and have the following behavior:

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [DecoratedProperty&lt;T, TValue&gt;(Func&lt;T, TValue&gt;, Action&lt;T, TValue&gt;, TValue)](#decoratedpropertyt-tvaluefunct-tvalue-actiont-tvalue-tvalue) | Helper for propagating a single property to and from the inner view. | 

### Methods

 | Name | Description |
| --- | --- |
| [Get()](#get) | Gets the current value from the inner view. | 
| [Set(TValue)](#settvalue) | Updates the property value, also updating the inner view if one exists. | 
| [Update()](#update) | Updates the inner view's property to the most recent value, if one has been set on the decorated property. | 

## Details

### Constructors

#### DecoratedProperty&lt;T, TValue&gt;(Func&lt;T, TValue&gt;, Action&lt;T, TValue&gt;, TValue)

Helper for propagating a single property to and from the inner view.

```cs
public DecoratedProperty<T, TValue>(Func<T, TValue> getValue, Action<T, TValue> setValue, TValue defaultValue);
```

##### Parameters

**`getValue`** &nbsp; [Func&lt;T, TValue&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2)  
Function to retrieve the current value from the inner view.

**`setValue`** &nbsp; [Action&lt;T, TValue&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2)  
Delegate to change the current value on the inner view.

**`defaultValue`** &nbsp; TValue  
The initial value to return from [Get()](decoratorview-1.decoratedproperty-1.md#get) if no view exists and the value has not been changed. This is never written to the view, it is only used by [Get()](decoratorview-1.decoratedproperty-1.md#get) and is effectively a hack to allow [DecoratedProperty&lt;T, TValue&gt;](decoratorview-1.decoratedproperty-1.md) to deal with value (struct) types.

##### Remarks

Decorated properties must be initialized in the decorator's constructor by calling [RegisterDecoratedProperty&lt;TValue&gt;(DecoratedProperty&lt;T, TValue&gt;)](decoratorview-1.md#registerdecoratedpropertytvaluedecoratedpropertyt-tvalue), and have the following behavior:

-----

### Methods

#### Get()

Gets the current value from the inner view.

```cs
public TValue Get();
```

##### Returns

`TValue`

  The value from the current view, if the view is non-null; otherwise, the default value configured for this property.

-----

#### Set(TValue)

Updates the property value, also updating the inner view if one exists.

```cs
public void Set(TValue value);
```

##### Parameters

**`value`** &nbsp; TValue  
The new value.

##### Remarks

If the inner view has not been created yet, then its corresponding property will be updated as soon as it is assigned to the [View](decoratorview-1.md#view).

-----

#### Update()

Updates the inner view's property to the most recent value, if one has been set on the decorated property.

```cs
public void Update();
```

##### Remarks

If there have been no calls to [Set(TValue)](decoratorview-1.decoratedproperty-1.md#settvalue), then the view is left untouched, to preserve any non-default settings on the inner view.

-----

