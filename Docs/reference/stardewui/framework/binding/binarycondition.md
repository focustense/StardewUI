---
title: BinaryCondition
description: A condition based on the comparison of two values.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BinaryCondition

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

A condition based on the comparison of two values.

```cs
public class BinaryCondition : StardewUI.Framework.Binding.ICondition, 
    System.IDisposable
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ BinaryCondition

**Implements**  
[ICondition](icondition.md), [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Remarks

Passes whenever both values are equal. Used for `*switch` and `*case` attributes. 

 Any type may be used for either operand, but a conversion must be available from one of the types to the other type in order for the condition to ever pass. If the two types are different, and conversion both ways is possible, then priority will be given to the type implementing [IEquatable&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1) on itself; if the best type is still ambiguous, then left->right conversion will be chosen over right->left.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [BinaryCondition(IValueSourceFactory, IValueConverterFactory, IResolutionScope, IAttribute, IResolutionScope, IAttribute)](#binaryconditionivaluesourcefactory-ivalueconverterfactory-iresolutionscope-iattribute-iresolutionscope-iattribute) | A condition based on the comparison of two values. | 

### Properties

 | Name | Description |
| --- | --- |
| [LeftContext](#leftcontext) | The context used to derive the LHS value, if the left attribute is a context binding. | 
| [LeftContextSelector](#leftcontextselector) | Optional source for automatically updating the [LeftContext](binarycondition.md#leftcontext). | 
| [Passed](#passed) | Whether or not the condition was passing as of the last [Update()](icondition.md#update). | 
| [RightContext](#rightcontext) | The context used to derive the RHS value, if the right attribute is a context binding. | 
| [RightContextSelector](#rightcontextselector) | Optional source for automatically updating the [RightContext](binarycondition.md#rightcontext). | 

### Methods

 | Name | Description |
| --- | --- |
| [Dispose()](#dispose) |  | 
| [Update()](#update) | Re-evaluates the condition and updates the [Passed](icondition.md#passed) state. | 

## Details

### Constructors

#### BinaryCondition(IValueSourceFactory, IValueConverterFactory, IResolutionScope, IAttribute, IResolutionScope, IAttribute)

A condition based on the comparison of two values.

```cs
public BinaryCondition(StardewUI.Framework.Sources.IValueSourceFactory valueSourceFactory, StardewUI.Framework.Converters.IValueConverterFactory valueConverterFactory, StardewUI.Framework.Content.IResolutionScope leftScope, StardewUI.Framework.Dom.IAttribute leftAttribute, StardewUI.Framework.Content.IResolutionScope rightScope, StardewUI.Framework.Dom.IAttribute rightAttribute);
```

##### Parameters

**`valueSourceFactory`** &nbsp; [IValueSourceFactory](../sources/ivaluesourcefactory.md)  
The factory responsible for creating [IValueSource&lt;T&gt;](../sources/ivaluesource-1.md) instances from attribute data.

**`valueConverterFactory`** &nbsp; [IValueConverterFactory](../converters/ivalueconverterfactory.md)  
The factory responsible for creating [IValueConverter&lt;TSource, TDestination&gt;](../converters/ivalueconverter-2.md) instances, used to convert bound values to the types required by the target view.

**`leftScope`** &nbsp; [IResolutionScope](../content/iresolutionscope.md)  
Scope for resolving any externalized `leftAttribute` values, such as translation keys.

**`leftAttribute`** &nbsp; [IAttribute](../dom/iattribute.md)  
The attribute containing the expression for the LHS operand.

**`rightScope`** &nbsp; [IResolutionScope](../content/iresolutionscope.md)  
Scope for resolving any externalized `rightAttribute` values, such as translation keys.

**`rightAttribute`** &nbsp; [IAttribute](../dom/iattribute.md)  
The attribute containing the expression for the RHS operand.

##### Remarks

Passes whenever both values are equal. Used for `*switch` and `*case` attributes. 

 Any type may be used for either operand, but a conversion must be available from one of the types to the other type in order for the condition to ever pass. If the two types are different, and conversion both ways is possible, then priority will be given to the type implementing [IEquatable&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1) on itself; if the best type is still ambiguous, then left->right conversion will be chosen over right->left.

-----

### Properties

#### LeftContext

The context used to derive the LHS value, if the left attribute is a context binding.

```cs
public StardewUI.Framework.Binding.BindingContext LeftContext { get; set; }
```

##### Property Value

[BindingContext](bindingcontext.md)

-----

#### LeftContextSelector

Optional source for automatically updating the [LeftContext](binarycondition.md#leftcontext).

```cs
public Func<StardewUI.Framework.Binding.BindingContext> LeftContextSelector { get; set; }
```

##### Property Value

[Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-1)<[BindingContext](bindingcontext.md)>

##### Remarks

If specified, then this selector will be automatically run on every [Update()](binarycondition.md#update) and assigned to the [LeftContext](binarycondition.md#leftcontext); otherwise, the [LeftContext](binarycondition.md#leftcontext) must be set explicitly in order to change the evaluation context for the left-hand value.

-----

#### Passed

Whether or not the condition was passing as of the last [Update()](icondition.md#update).

```cs
public bool Passed { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### RightContext

The context used to derive the RHS value, if the right attribute is a context binding.

```cs
public StardewUI.Framework.Binding.BindingContext RightContext { get; set; }
```

##### Property Value

[BindingContext](bindingcontext.md)

-----

#### RightContextSelector

Optional source for automatically updating the [RightContext](binarycondition.md#rightcontext).

```cs
public Func<StardewUI.Framework.Binding.BindingContext> RightContextSelector { get; set; }
```

##### Property Value

[Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-1)<[BindingContext](bindingcontext.md)>

##### Remarks

If specified, then this selector will be automatically run on every [Update()](binarycondition.md#update) and assigned to the [RightContext](binarycondition.md#rightcontext); otherwise, the [RightContext](binarycondition.md#rightcontext) must be set explicitly in order to change the evaluation context for the right-hand value.

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

