---
title: ViewBehaviors
description: Wrapper for the entire set of behaviors attached to a single node/view.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewBehaviors

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Wrapper for the entire set of behaviors attached to a single node/view.

```cs
public class ViewBehaviors : System.IDisposable
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ViewBehaviors

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ViewBehaviors(IEnumerable&lt;IAttribute&gt;, IBehaviorFactory, IValueSourceFactory, IValueConverterFactory, IResolutionScope)](#viewbehaviorsienumerableiattribute-ibehaviorfactory-ivaluesourcefactory-ivalueconverterfactory-iresolutionscope) | Wrapper for the entire set of behaviors attached to a single node/view. | 

### Methods

 | Name | Description |
| --- | --- |
| [Dispose()](#dispose) |  | 
| [PreUpdate(TimeSpan)](#preupdatetimespan) | Runs on every update tick, before any bindings or views update. | 
| [SetContext(BindingContext)](#setcontextbindingcontext) | Updates or removes the binding context for all managed behaviors. | 
| [SetTarget(BehaviorTarget)](#settargetbehaviortarget) | Updates the attached target (view and state) for all managed behaviors. | 
| [Update(TimeSpan)](#updatetimespan) | Runs on every update tick. | 

## Details

### Constructors

#### ViewBehaviors(IEnumerable&lt;IAttribute&gt;, IBehaviorFactory, IValueSourceFactory, IValueConverterFactory, IResolutionScope)

Wrapper for the entire set of behaviors attached to a single node/view.

```cs
public ViewBehaviors(System.Collections.Generic.IEnumerable<StardewUI.Framework.Dom.IAttribute> behaviorAttributes, StardewUI.Framework.Behaviors.IBehaviorFactory behaviorFactory, StardewUI.Framework.Sources.IValueSourceFactory valueSourceFactory, StardewUI.Framework.Converters.IValueConverterFactory valueConverterFactory, StardewUI.Framework.Content.IResolutionScope resolutionScope);
```

##### Parameters

**`behaviorAttributes`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[IAttribute](../dom/iattribute.md)>  
List of all behavior attributes applied to the node.

**`behaviorFactory`** &nbsp; [IBehaviorFactory](../behaviors/ibehaviorfactory.md)  
Factory for creating behaviors.

**`valueSourceFactory`** &nbsp; [IValueSourceFactory](../sources/ivaluesourcefactory.md)  
The factory responsible for creating [IValueSource&lt;T&gt;](../sources/ivaluesource-1.md) instances from attribute data.

**`valueConverterFactory`** &nbsp; [IValueConverterFactory](../converters/ivalueconverterfactory.md)  
The factory responsible for creating [IValueConverter&lt;TSource, TDestination&gt;](../converters/ivalueconverter-2.md) instances, used to convert bound values to the data types required by individual behaviors.

**`resolutionScope`** &nbsp; [IResolutionScope](../content/iresolutionscope.md)  
Scope for resolving externalized attributes, such as translation keys.

-----

### Methods

#### Dispose()



```cs
public void Dispose();
```

-----

#### PreUpdate(TimeSpan)

Runs on every update tick, before any bindings or views update.

```cs
public void PreUpdate(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Time elapsed since last tick.

-----

#### SetContext(BindingContext)

Updates or removes the binding context for all managed behaviors.

```cs
public void SetContext(StardewUI.Framework.Binding.BindingContext context);
```

##### Parameters

**`context`** &nbsp; [BindingContext](bindingcontext.md)  
The new context.

-----

#### SetTarget(BehaviorTarget)

Updates the attached target (view and state) for all managed behaviors.

```cs
public void SetTarget(StardewUI.Framework.Behaviors.BehaviorTarget target);
```

##### Parameters

**`target`** &nbsp; [BehaviorTarget](../behaviors/behaviortarget.md)  
The new behavior target, or `null` to remove behaviors.

##### Remarks

If the target is `null` then all behaviors will be disabled/removed.

-----

#### Update(TimeSpan)

Runs on every update tick.

```cs
public void Update(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Time elapsed since last tick.

-----

