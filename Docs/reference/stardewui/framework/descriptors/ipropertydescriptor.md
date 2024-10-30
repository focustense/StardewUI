---
title: IPropertyDescriptor
description: Describes a single property on a bindable object (i.e. a view).
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IPropertyDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Describes a single property on a bindable object (i.e. a view).

```cs
public interface IPropertyDescriptor : 
    StardewUI.Framework.Descriptors.IMemberDescriptor
```

**Implements**  
[IMemberDescriptor](imemberdescriptor.md)

## Members

### Properties

 | Name | Description |
| --- | --- |
| [CanRead](#canread) | Whether or not the property is readable, i.e. has a public getter. | 
| [CanWrite](#canwrite) | Whether or not the property is writable, i.e. has a public setter. | 
| [ValueType](#valuetype) | The property's value type. | 

## Details

### Properties

#### CanRead

Whether or not the property is readable, i.e. has a public getter.

```cs
bool CanRead { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### CanWrite

Whether or not the property is writable, i.e. has a public setter.

```cs
bool CanWrite { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### ValueType

The property's value type.

```cs
System.Type ValueType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

