---
title: ICondition
description: A condition used in a ConditionalNode.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface ICondition

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

A condition used in a [ConditionalNode](conditionalnode.md).

```cs
public interface ICondition : System.IDisposable
```

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Properties

 | Name | Description |
| --- | --- |
| [Context](#context) | The context for evaluating the condition; i.e. the context of the node to which the condition applies. | 
| [Passed](#passed) | Whether or not the condition was passing as of the last [Update()](icondition.md#update). | 

### Methods

 | Name | Description |
| --- | --- |
| [Update()](#update) | Re-evaluates the condition and updates the [Passed](icondition.md#passed) state. | 

## Details

### Properties

#### Context

The context for evaluating the condition; i.e. the context of the node to which the condition applies.

```cs
StardewUI.Framework.Binding.BindingContext Context { get; set; }
```

##### Property Value

[BindingContext](bindingcontext.md)

-----

#### Passed

Whether or not the condition was passing as of the last [Update()](icondition.md#update).

```cs
bool Passed { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

### Methods

#### Update()

Re-evaluates the condition and updates the [Passed](icondition.md#passed) state.

```cs
void Update();
```

-----

