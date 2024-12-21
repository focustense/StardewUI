---
title: ReferenceViewDefaults
description: View defaults based on a reference view.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ReferenceViewDefaults

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

View defaults based on a reference view.

```cs
public class ReferenceViewDefaults : 
    StardewUI.Framework.Descriptors.IViewDefaults
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ReferenceViewDefaults

**Implements**  
[IViewDefaults](iviewdefaults.md)

## Remarks

References views are dummy views created explicitly for the purposes of deriving default values, and never displayed or written to.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ReferenceViewDefaults(IView)](#referenceviewdefaultsiview) | View defaults based on a reference view. | 

### Methods

 | Name | Description |
| --- | --- |
| [GetDefaultValue&lt;T&gt;(string)](#getdefaultvaluetstring) | Gets the default value for the named property. | 

## Details

### Constructors

#### ReferenceViewDefaults(IView)

View defaults based on a reference view.

```cs
public ReferenceViewDefaults(StardewUI.IView view);
```

##### Parameters

**`view`** &nbsp; [IView](../../iview.md)  
The reference view whose properties are set to the assumed defaults.

##### Remarks

References views are dummy views created explicitly for the purposes of deriving default values, and never displayed or written to.

-----

### Methods

#### GetDefaultValue&lt;T&gt;(string)

Gets the default value for the named property.

```cs
public T GetDefaultValue<T>(string propertyName);
```

##### Parameters

**`propertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The property name.

##### Returns

`T`

  The default value of the specified property for a newly-created view.

-----

