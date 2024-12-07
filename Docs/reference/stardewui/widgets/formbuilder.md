---
title: FormBuilder
description: Fluent builder style API for creating form-like tables within a view.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class FormBuilder

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Fluent builder style API for creating form-like tables within a view.

```cs
public class FormBuilder
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ FormBuilder

## Remarks

Useful for "settings" style views.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [FormBuilder(Int32, Int32)](#formbuilderint-int) | Fluent builder style API for creating form-like tables within a view. | 

### Methods

 | Name | Description |
| --- | --- |
| [AddCustom(IView)](#addcustomiview) | Adds a custom row, which is simply a horizontal [Lane](lane.md) consisting of the specified views, not including any label - i.e. the first view is flush with the labels on other rows. | 
| [AddField(string, string, IView)](#addfieldstring-string-iview) | Adds a labeled control row, i.e. a field. | 
| [AddSection(string)](#addsectionstring) | Starts a new section by adding header text. | 
| [Build()](#build) | Builds the form. | 
| [CreateSectionHeading(string)](#createsectionheadingstring) | Creates the banner used as a section heading. | 
| [SetMargin(Edges)](#setmarginedges) | Configures the margin for the entire form. | 
| [SetPadding(Edges)](#setpaddingedges) | Configures the padding for the entire form. | 

## Details

### Constructors

#### FormBuilder(int, int)

Fluent builder style API for creating form-like tables within a view.

```cs
public FormBuilder(int labelWidth, int fieldIndent);
```

##### Parameters

**`labelWidth`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The width of the label column. Same for all rows.

**`fieldIndent`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Pixel amount by which to indent the field rows, relative to any heading rows.

##### Remarks

Useful for "settings" style views.

-----

### Methods

#### AddCustom(IView)

Adds a custom row, which is simply a horizontal [Lane](lane.md) consisting of the specified views, not including any label - i.e. the first view is flush with the labels on other rows.

```cs
public StardewUI.Widgets.FormBuilder AddCustom(StardewUI.IView views);
```

##### Parameters

**`views`** &nbsp; [IView](../iview.md)  
The views to display in this row.

##### Returns

[FormBuilder](formbuilder.md)

  The current builder instance.

##### Remarks

Might be used for a row of confirmation buttons, a paragraph of help text, etc.

-----

#### AddField(string, string, IView)

Adds a labeled control row, i.e. a field.

```cs
public StardewUI.Widgets.FormBuilder AddField(string title, string description, StardewUI.IView view);
```

##### Parameters

**`title`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The field title, displayed on the left side.

**`description`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Description of the field's value or purpose, displayed as a tooltip when hovering over the title text.

**`view`** &nbsp; [IView](../iview.md)  
The view for modifying the field's value.

##### Returns

[FormBuilder](formbuilder.md)

  The current builder instance.

-----

#### AddSection(string)

Starts a new section by adding header text.

```cs
public StardewUI.Widgets.FormBuilder AddSection(string title);
```

##### Parameters

**`title`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The section title.

##### Returns

[FormBuilder](formbuilder.md)

  The current builder instance.

-----

#### Build()

Builds the form.

```cs
public StardewUI.IView Build();
```

##### Returns

[IView](../iview.md)

  The view containing the form layout.

-----

#### CreateSectionHeading(string)

Creates the banner used as a section heading.

```cs
public static StardewUI.IView CreateSectionHeading(string title);
```

##### Parameters

**`title`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The section title.

##### Returns

[IView](../iview.md)

  Section heading view.

##### Remarks

This is the standalone version of [AddSection(string)](formbuilder.md#addsectionstring) that can be used by any views wanting to provide form-style section headings outside of an actual form.

-----

#### SetMargin(Edges)

Configures the margin for the entire form.

```cs
public StardewUI.Widgets.FormBuilder SetMargin(StardewUI.Layout.Edges margin);
```

##### Parameters

**`margin`** &nbsp; [Edges](../layout/edges.md)  
Margin outside the form.

##### Returns

[FormBuilder](formbuilder.md)

  The current builder instance.

-----

#### SetPadding(Edges)

Configures the padding for the entire form.

```cs
public StardewUI.Widgets.FormBuilder SetPadding(StardewUI.Layout.Edges padding);
```

##### Parameters

**`padding`** &nbsp; [Edges](../layout/edges.md)  
Padding inside the form.

##### Returns

[FormBuilder](formbuilder.md)

  The current builder instance.

-----

