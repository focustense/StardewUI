---
title: UnaryCondition
description: A condition based on a single value that is convertible to a Boolean.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class UnaryCondition

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

A condition based on a single value that is convertible to a [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean).

```cs
public class UnaryCondition : StardewUI.Framework.Binding.ICondition, 
    System.IDisposable
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ UnaryCondition

**Implements**  
[ICondition](icondition.md), [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Remarks

Passes whenever the value's boolean representation is `true`. Used for `*if` attributes.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [UnaryCondition(IValueSourceFactory, IValueConverterFactory, IResolutionScope, IAttribute)](#unaryconditionivaluesourcefactory-ivalueconverterfactory-iresolutionscope-iattribute) | A condition based on a single value that is convertible to a [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean). | 

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

#### UnaryCondition(IValueSourceFactory, IValueConverterFactory, IResolutionScope, IAttribute)

A condition based on a single value that is convertible to a [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean).

```cs
public UnaryCondition(StardewUI.Framework.Sources.IValueSourceFactory valueSourceFactory, StardewUI.Framework.Converters.IValueConverterFactory valueConverterFactory, StardewUI.Framework.Content.IResolutionScope resolutionScope, StardewUI.Framework.Dom.IAttribute attribute);
```

##### Parameters

**`valueSourceFactory`** &nbsp; [IValueSourceFactory](../sources/ivaluesourcefactory.md)  
The factory responsible for creating [IValueSource&lt;T&gt;](../sources/ivaluesource-1.md) instances from attribute data.

**`valueConverterFactory`** &nbsp; [IValueConverterFactory](../converters/ivalueconverterfactory.md)  
The factory responsible for creating [IValueConverter&lt;TSource, TDestination&gt;](../converters/ivalueconverter-2.md) instances, used to convert bound values to the types required by the target view.

**`resolutionScope`** &nbsp; [IResolutionScope](../content/iresolutionscope.md)  
Scope for resolving externalized attributes, such as translation keys.

**`attribute`** &nbsp; [IAttribute](../dom/iattribute.md)  
The attribute containing the conditional expression.

##### Remarks

Passes whenever the value's boolean representation is `true`. Used for `*if` attributes.

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
public bool Passed { get; private set; }
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

