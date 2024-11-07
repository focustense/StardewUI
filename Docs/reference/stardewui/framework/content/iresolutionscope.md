---
title: IResolutionScope
description: Defines a scope in which certain types of external and potentially ambiguous binding attributes may be resolved.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IResolutionScope

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Content](index.md)  
Assembly: StardewUI.dll  

</div>

Defines a scope in which certain types of external and potentially ambiguous binding attributes may be resolved.

```cs
public interface IResolutionScope
```

## Remarks

In general, resolution scopes are used where a document may include an unqualified or short-form reference, for which such a reference means to look in the same mod that originally provided that document.

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetTranslation(string)](#gettranslationstring) | Attempts to obtain a translation value with the given key. | 
| [GetTranslationValue(string)](#gettranslationvaluestring) | Attempts to obtain the string value of a translation with the given key. | 

## Details

### Methods

#### GetTranslation(string)

Attempts to obtain a translation value with the given key.

```cs
StardewModdingAPI.Translation GetTranslation(string key);
```

##### Parameters

**`key`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The qualified or unqualified translation key. Unqualified keys are identical to their name in the translation file (i.e. in `i18n/default.json`), while qualified keys include a prefix with the specific mod, e.g. `authorname.ModName:TranslationKey`.

##### Returns

Translation

  One of: 

  - The translation, if available in the current language
  - The default-language (usually English) string, if the `key` exists but no translation is available;
  - A translation with placeholder text, if the `key` resolves to a known mod (or if the scope points to a default mod) but the mod is missing that specific key;
  - `null`, if the `key` is unqualified and no default scope is available.

-----

#### GetTranslationValue(string)

Attempts to obtain the string value of a translation with the given key.

```cs
string GetTranslationValue(string key);
```

##### Parameters

**`key`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The qualified or unqualified translation key. Unqualified keys are identical to their name in the translation file (i.e. in `i18n/default.json`), while qualified keys include a prefix with the specific mod, e.g. `authorname.ModName:TranslationKey`.

##### Returns

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

  One of: 

  - The translation, if available in the current language
  - The default-language (usually English) string, if the `key` exists but no translation is available;
  - A translation with placeholder text, if the `key` cannot be resolved to a known translation in a known mod.

##### Remarks

Unless specifically overridden, calling this is normally the same as [GetTranslation(string)](iresolutionscope.md#gettranslationstring), except that it returns [string](https://learn.microsoft.com/en-us/dotnet/api/system.string) instead of Translation, of which the latter is not instantiable outside of SMAPI's internals. To maintain consistency, code that _reads_ translations should always use `GetTranslationValue`, while code that _provides_ translations (i.e. implementations of [IResolutionScope](iresolutionscope.md)) should only implement `GetTranslation`, and leave this method alone.

-----

