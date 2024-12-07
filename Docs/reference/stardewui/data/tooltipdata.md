---
title: TooltipData
description: Provides data for all known variants of a menu tooltip.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TooltipData

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Data](index.md)  
Assembly: StardewUI.dll  

</div>

Provides data for all known variants of a menu tooltip.

```cs
[StardewUI.DuckType]
public record TooltipData : IEquatable<StardewUI.Data.TooltipData>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ TooltipData

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[TooltipData](tooltipdata.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TooltipData(string, string, Item, Int32?, Int32, string, Int32, CraftingRecipe, IList&lt;Item&gt;)](#tooltipdatastring-string-item-int-int-string-int-craftingrecipe-ilistitem) | Provides data for all known variants of a menu tooltip. | 

### Properties

 | Name | Description |
| --- | --- |
| [AdditionalCraftingMaterials](#additionalcraftingmaterials) | List of additional items required for crafting that are not included in the [CraftingRecipe](tooltipdata.md#craftingrecipe). | 
| [CraftingRecipe](#craftingrecipe) | Crafting recipe to show, if the tooltip is for a craftable item. | 
| [CurrencyAmount](#currencyamount) | Amount of money associated with the tooltip action, generally a buy or sell price. | 
| [CurrencySymbol](#currencysymbol) |  The currency associated with any [CurrencyAmount](tooltipdata.md#currencyamount); has no effect unless [CurrencyAmount](tooltipdata.md#currencyamount) is also specified. 

 The meaning of each value is dependent on game implementation, but at the time of writing the available options are: `0` = coins, `1` = star tokens (silver star), `2` = casino tokens, and `4` = Qi gems. 

 | 
| [EqualityContract](#equalitycontract) |  | 
| [Item](#item) | The specific game item, if any, that is the "topic" of this tooltip, used to show additional item-specific information such as buffs, durations and recovery values. | 
| [RequiredItemAmount](#requireditemamount) | The number of items required, e.g. for trade, when [RequiredItemId](tooltipdata.md#requireditemid) is specified. | 
| [RequiredItemId](#requireditemid) | Item ID to show as a required item, usually used as an alternative to [CurrencySymbol](tooltipdata.md#currencysymbol) for non-currency trades, such as the Desert Trader. | 
| [Text](#text) | The primary description text to display. Tooltips converted from a simple [string](https://learn.microsoft.com/en-us/dotnet/api/system.string) will have this field populated. | 
| [Title](#title) | Bolded title to display above the [Text](tooltipdata.md#text), with a separator in between. | 

### Methods

 | Name | Description |
| --- | --- |
| [ConstrainTextWidth(Int32)](#constraintextwidthint) | Constraints the tooltip to a specified pixel width by breaking lines for the [Text](tooltipdata.md#text) and [Title](tooltipdata.md#title). | 

## Details

### Constructors

#### TooltipData(string, string, Item, int?, int, string, int, CraftingRecipe, IList&lt;Item&gt;)

Provides data for all known variants of a menu tooltip.

```cs
public TooltipData(string Text, string Title, StardewValley.Item Item, int? CurrencyAmount, int CurrencySymbol, string RequiredItemId, int RequiredItemAmount, StardewValley.CraftingRecipe CraftingRecipe, System.Collections.Generic.IList<StardewValley.Item> AdditionalCraftingMaterials);
```

##### Parameters

**`Text`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The primary description text to display. Tooltips converted from a simple [string](https://learn.microsoft.com/en-us/dotnet/api/system.string) will have this field populated.

**`Title`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Bolded title to display above the [Text](tooltipdata.md#text), with a separator in between.

**`Item`** &nbsp; Item  
The specific game item, if any, that is the "topic" of this tooltip, used to show additional item-specific information such as buffs, durations and recovery values.

**`CurrencyAmount`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)>  
Amount of money associated with the tooltip action, generally a buy or sell price.

**`CurrencySymbol`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
 The currency associated with any [CurrencyAmount](tooltipdata.md#currencyamount); has no effect unless [CurrencyAmount](tooltipdata.md#currencyamount) is also specified. 

 The meaning of each value is dependent on game implementation, but at the time of writing the available options are: `0` = coins, `1` = star tokens (silver star), `2` = casino tokens, and `4` = Qi gems. 



**`RequiredItemId`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Item ID to show as a required item, usually used as an alternative to [CurrencySymbol](tooltipdata.md#currencysymbol) for non-currency trades, such as the Desert Trader.

**`RequiredItemAmount`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The number of items required, e.g. for trade, when [RequiredItemId](tooltipdata.md#requireditemid) is specified.

**`CraftingRecipe`** &nbsp; CraftingRecipe  
Crafting recipe to show, if the tooltip is for a craftable item.

**`AdditionalCraftingMaterials`** &nbsp; [IList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1)<Item>  
List of additional items required for crafting that are not included in the [CraftingRecipe](tooltipdata.md#craftingrecipe).

-----

### Properties

#### AdditionalCraftingMaterials

List of additional items required for crafting that are not included in the [CraftingRecipe](tooltipdata.md#craftingrecipe).

```cs
public System.Collections.Generic.IList<StardewValley.Item> AdditionalCraftingMaterials { get; set; }
```

##### Property Value

[IList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1)<Item>

-----

#### CraftingRecipe

Crafting recipe to show, if the tooltip is for a craftable item.

```cs
public StardewValley.CraftingRecipe CraftingRecipe { get; set; }
```

##### Property Value

CraftingRecipe

-----

#### CurrencyAmount

Amount of money associated with the tooltip action, generally a buy or sell price.

```cs
public int? CurrencyAmount { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)>

-----

#### CurrencySymbol

 The currency associated with any [CurrencyAmount](tooltipdata.md#currencyamount); has no effect unless [CurrencyAmount](tooltipdata.md#currencyamount) is also specified. 

 The meaning of each value is dependent on game implementation, but at the time of writing the available options are: `0` = coins, `1` = star tokens (silver star), `2` = casino tokens, and `4` = Qi gems. 



```cs
public int CurrencySymbol { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Item

The specific game item, if any, that is the "topic" of this tooltip, used to show additional item-specific information such as buffs, durations and recovery values.

```cs
public StardewValley.Item Item { get; set; }
```

##### Property Value

Item

-----

#### RequiredItemAmount

The number of items required, e.g. for trade, when [RequiredItemId](tooltipdata.md#requireditemid) is specified.

```cs
public int RequiredItemAmount { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### RequiredItemId

Item ID to show as a required item, usually used as an alternative to [CurrencySymbol](tooltipdata.md#currencysymbol) for non-currency trades, such as the Desert Trader.

```cs
public string RequiredItemId { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Text

The primary description text to display. Tooltips converted from a simple [string](https://learn.microsoft.com/en-us/dotnet/api/system.string) will have this field populated.

```cs
public string Text { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Title

Bolded title to display above the [Text](tooltipdata.md#text), with a separator in between.

```cs
public string Title { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

### Methods

#### ConstrainTextWidth(int)

Constraints the tooltip to a specified pixel width by breaking lines for the [Text](tooltipdata.md#text) and [Title](tooltipdata.md#title).

```cs
public StardewUI.Data.TooltipData ConstrainTextWidth(int maxWidth);
```

##### Parameters

**`maxWidth`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The desired maximum pixel width for the displayed tooltip.

##### Returns

[TooltipData](tooltipdata.md)

  A [TooltipData](tooltipdata.md) instance with any necessary line breaks added to its text properties in order to keep the displayed width equal to or less than `maxWidth`.

-----

