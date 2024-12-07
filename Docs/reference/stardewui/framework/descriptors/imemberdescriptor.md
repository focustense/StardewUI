---
title: IMemberDescriptor
description: Describes a single member (property, method, or event) of a bindable object, such as a view.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IMemberDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Describes a single member (property, method, or event) of a bindable object, such as a view.

```cs
public interface IMemberDescriptor
```

## Members

### Properties

 | Name | Description |
| --- | --- |
| [DeclaringType](#declaringtype) | The type on which the member is declared. | 
| [Name](#name) | The member name. | 

## Details

### Properties

#### DeclaringType

The type on which the member is declared.

```cs
System.Type DeclaringType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Name

The member name.

```cs
string Name { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

