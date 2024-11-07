---
title: StardewUI.Framework.Sources
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Framework.Sources Namespace

## Classes

| Name | Description |
| --- | --- |
| [AssetValueSource&lt;T&gt;](assetvaluesource-1.md) | Value source that looks up an asset registered with SMAPI's content manager. |
| [ConstantValueSource&lt;T&gt;](constantvaluesource-1.md) | Value source with a constant value, generally used to hold the literal (text) value of an attribute. |
| [ContextPropertyValueSource&lt;T&gt;](contextpropertyvaluesource-1.md) | Value source that obtains its value from a context (or "model") property. |
| [ConvertedValueSource](convertedvaluesource.md) | Helpers for creating instances of the generic [ConvertedValueSource&lt;TSource, T&gt;](convertedvaluesource-2.md) when some of the types are unknown at compile time. |
| [ConvertedValueSource&lt;TSource, T&gt;](convertedvaluesource-2.md) | A value source that wraps another [IValueSource&lt;T&gt;](ivaluesource-1.md) and performs automatic conversion. |
| [TranslationValueSource](translationvaluesource.md) | Value source that reads the localized string from a translation key. |
| [ValueSourceFactory](valuesourcefactory.md) | Default implementation of the [IValueSourceFactory](ivaluesourcefactory.md) supporting all binding types. |

## Interfaces

| Name | Description |
| --- | --- |
| [IValueSource](ivaluesource.md) | Holds the type-independent data of an [IValueSource&lt;T&gt;](ivaluesource-1.md). |
| [IValueSource&lt;T&gt;](ivaluesource-1.md) | Abstract representation of the source of any value, generally as used in a data binding. |
| [IValueSourceFactory](ivaluesourcefactory.md) | Provides methods to look up runtime value types and build appropriate sources based on their binding information. |

