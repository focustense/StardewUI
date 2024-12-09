---
title: NegatedCondition
description: Wrapper for an ICondition that negates its outcome.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class NegatedCondition

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Wrapper for an [ICondition](icondition.md) that negates its outcome.

```cs
public class NegatedCondition : StardewUI.Framework.Binding.ICondition, 
    System.IDisposable
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ NegatedCondition

**Implements**  
[ICondition](icondition.md), [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [NegatedCondition(ICondition)](#negatedconditionicondition) | Wrapper for an [ICondition](icondition.md) that negates its outcome. | 

### Properties

 | Name | Description |
| --- | --- |
| [Context](#context) | The context for evaluating the condition; i.e. the context of the node to which the condition applies. | 
| [Passed](#passed) | Whether or not the condition was passing as of the last [Update()](icondition.md#update). | 

### Methods

 | Name | Description |
| --- | --- |
| [Dispose()](#dispose) |  | 
| [Update()](#update) | Re-evaluates the condition and updates the [Passed](icondition.md#passed) state. | 

## Details

### Constructors

#### NegatedCondition(ICondition)

Wrapper for an [ICondition](icondition.md) that negates its outcome.

```cs
public NegatedCondition(StardewUI.Framework.Binding.ICondition innerCondition);
```

##### Parameters

**`innerCondition`** &nbsp; [ICondition](icondition.md)

-----

### Properties

#### Context

The context for evaluating the condition; i.e. the context of the node to which the condition applies.

```cs
public StardewUI.Framework.Binding.BindingContext Context { get; set; }
```

##### Property Value

[BindingContext](bindingcontext.md)

-----

#### Passed

Whether or not the condition was passing as of the last [Update()](icondition.md#update).

```cs
public bool Passed { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

### Methods

#### Dispose()



```cs
public void Dispose();
```

-----

#### Update()

Re-evaluates the condition and updates the [Passed](icondition.md#passed) state.

```cs
public void Update();
```

-----

