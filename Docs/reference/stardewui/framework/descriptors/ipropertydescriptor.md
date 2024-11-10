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
| [IsAutoProperty](#isautoproperty) | Whether or not the property is likely auto-implemented. | 
| [IsField](#isfield) | Whether or not the underlying member is a field, rather than a real property. | 
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

#### IsAutoProperty

Whether or not the property is likely auto-implemented.

```cs
bool IsAutoProperty { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Auto-property detection is heuristic, relying on the method's IL instructions and the name of its backing field. This can often be interpreted as a signal that the property won't receive property-change notifications, since to do so (whether explicitly or via some weaver/source generator) requires an implementation that is different from the auto-generated getter and setter. 

 Caveats: This only works as a negative signal (a value of `false` does not prove that the property _will_ receive notifications, even if the declaring type implements [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged)), and is somewhat fuzzy even as a negative signal; it is theoretically possible for a source generator or IL weaver to leave behind all the markers of an auto property and still emit notifications, although no known libraries actually do so.

-----

#### IsField

Whether or not the underlying member is a field, rather than a real property.

```cs
bool IsField { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

For binding convenience, fields and properties are both called "properties" for descriptors, as the external access pattern is the same; however, mutable fields can never reliably emit property-change notifications regardless of whether the declaring type implements [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged), so this is usually used to emit some warning.

-----

#### ValueType

The property's value type.

```cs
System.Type ValueType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

