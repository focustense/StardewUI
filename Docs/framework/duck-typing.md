---
title: Duck Typing
---

# Duck Typing :material-test-tube:{ title="Experimental" }

When attempting a [data type conversion](starml.md#type-conversions) and no explicit conversion is available, StardewUI will attempt a [duck typing](https://en.wikipedia.org/wiki/Duck_typing) conversion, allowing you to provide your own data types in your [context](binding-context.md) data that have no direct knowledge of (or dependency on) StardewUI's internal types but simply have the same or similar structure.

## Rules

Not all StardewUI types allow this conversion. Consult the [API reference](../reference/stardewui/index.md) for types that declare the [`[DuckType]` attribute](../reference/stardewui/ducktypeattribute.md); these are the types available as _target_ (destination) types for duck-type conversion, while all other types will ignore it.

In order for a user-defined type to be converted to a StardewUI type, such as [`Sprite`](../reference/stardewui/graphics/sprite.md) or [`Edges`](../reference/stardewui/layout/edges.md), the following requirements must be met:

1. The target type must have either a default constructor, or a constructor whose non-optional arguments all correspond to property names (case-insensitive) on the source type;
2. If relying on the default constructor, there must be at least one source property with the same name as a target property and a type that is convertible to the target property's type;
3. There must _not_ be any source properties whose names match a constructor argument, but whose types cannot be converted to the argument type;
4. The conversion cannot be recursive, e.g. a `class Foo { public Foo Child; }` is not eligible for duck type conversion. This is the case even if the reference is indirect, e.g. a `Foo.Bar.Foo` reference.

If a property name cannot match—for example, it might need to be a certain name to implement some interface, deserialize from JSON or XML, etc.—then the [`[DuckProperty]`](../reference/stardewui/duckpropertyattribute.md) attribute can be used to "rename" it for StardewUI, either for all conversions or when converting to a specific type.

Like duck-typing in general, the rules tend to be easier to understand at an intuitive level than they are formally. Some examples of valid and invalid conversions are below; these all use the [`Edges`](../reference/stardewui/layout/edges.md) type as a target, which is defined as:

```cs
[DuckType]
public record Edges(int Left = 0, int Top = 0, int Right = 0, int Bottom = 0)
{
    // Other constructors
}
```

This is a useful type to look at in order to understand constructor matching because it has multiple constructors. Note that these examples use class names that are different from StardewUI's types, but this is only for reader clarity; it is allowed for user-defined types to have the exact same name as the target type.

### Allowed Conversions

All of the following conversions to `Edges` will succeed:

=== ":material-check-bold: Identical"

    !!! success "Valid"

        ```cs
        public record MyEdges(int Left, int Top, int Bottom, int Left);
        ```
        
        Includes all `Edges` constructor arguments, with the exact same types. The simplest and most obvious conversion.

=== ":material-check-bold: Renamed"

    !!! success "Valid"

        ```cs
        public struct MyEdges
        {
            [DuckProperty("Left")]
            public int X;

            [DuckProperty("Top")]
            public int Y;

            public int Width { get; set; }

            public int Height { get; set; }

            [DuckProperty("Right")]
            public int X2 => X + Width;

            [DuckProperty("Bottom")]
            public int Y2 => Y + Height;
        }
        ```
        
        In this structure (`struct` types are supported) made to resemble an XNA `Rectangle`, all the properties are available, but all have the wrong names; we use `[DuckProperty]` to make them recognizable to StardewUI. Fields (`X` and `Y`) and computed properties (`X2` and `Y2`) are also supported in addition to the usual auto-properties.

=== ":material-check-bold: Partial"

    !!! success "Valid"

        ```cs
        public record MyEdges(int Left, int Right);
        ```
        
        This works because `Edges` specifies default values for all properties, including the ones "missing" here, `Top` and `Bottom`. At least some properties (`Left` and `Right`) do match, so the constructor still counts. On conversion, `Top` and `Bottom` will have their default values of `0`, while `Left` and `Right` will be copied from `MyEdges`.

=== ":material-check-bold: Alternate"

    !!! success "Valid"

        ```cs
        public record MyEdges(int Horizontal, int Vertical);
        ```

        Here we don't have any of the original properties, but `Edges` has a [different constructor](../reference/stardewui/layout/edges.md#edgesint-int) that does match. The converted result will be the same as if it had been created using `new Edges(Horizontal: x, Vertical: y)`.

=== ":material-check-bold: Convertible"

    !!! success "Valid"

        ```cs
        public class MyEdges
        {
            public int? Horizontal { get; set; }
            public string Vertical { get; set; } = "";
        }
        ```
        
        Above, we've given different (and perhaps a little contrived) types to `Horizontal` and `Vertical`, but they are _convertible_ types; any `Nullable<T>` can convert to a `T`, and any `string` can be parsed into a numeric type. Therefore, because these properties (a) have the same _names_ as constructor arguments and (b) can be converted _to_ the argument types after the constructor is chosen, the conversion is allowed.

### Ignored Conversions

These types are _not_ allowed to be converted to `Edges`. You won't receive a specific error related to duck-typing; the log will simply say that no conversion is available.

=== ":material-bug: Missing"

    !!! failure "Broken"

        ```cs
        public record MyEdges(int Horizontal);
        ```
        
        This attempt at conversion is a little like the successful "Partial" or "Alternate" examples above. The problem is that the `Horizontal` property selects for the `(int Horizontal, int Vertical)` constructor, and unlike the "Partial" example, that alternate constructor does **not** have default parameters. We need a match for `Vertical`, and we don't have one; therefore the conversion is not supported.
        
        Recall that constructors are matched on argument _names_, not types, so the existence of an `Edges(int all)` parameter doesn't provide an alternative match.

=== ":material-bug: Incompatible"

    !!! failure "Broken"

        ```cs
        public record MyEdges(int Left, int Top, int Right, bool Bottom);
        ```
        
        Almost identical to the very first example except that `int Bottom` has been changed to `bool Bottom`. While it's a subtle change, it's enough to disable conversion, because `bool` has no conversion to `int`.
        
        If we _removed_ `Bottom` entirely, this would work, because `Bottom` is optional. But because it's been specified, _and_ it has a non-convertible type, it excludes the entire constructor.
        
        Another way we could make this work is by "renaming" `Bottom` so that StardewUI doesn't see it, e.g. by adding a `[DuckProperty("HasBottom")]` attribute to the `Bottom` parameter/property.

=== ":material-bug: Insufficient"

    !!! failure "Broken"

        ```cs
        public class MyEdges
        {
            public string Foo { get; set; } = "";
            public float Bar { get; set; }
        }
        ```
        
        This can technically match the `Edges` constructor, despite matching none of its actual parameters, because all of the constructor parameters are optional.
        
        Since allowing such conversions would imply that literally _any_ type with a default parameterless constructor could be the target of a duck-type conversion from _any_ other type, we explicitly block the scenario; at least one constructor argument or property on the target type must match a property on the source.

=== ":material-bug: Prickly Priority"

    !!! failure "Broken"

        ```cs
        public class MyEdges
        {
            public int Left { get; set; }
            public bool Horizontal { get; set; }
            public bool Vertical { get; set; }
        }
        ```

        This final example is designed to be a little tricky. We have `Left`, so that should match the default constructor: `Edges(int Left = 0, int Top = 0, int Right = 0, int Bottom = 0)`, shouldn't it?
        
        Unfortunately, the existence of `Horizontal` and `Vertical` also matches a different constructor whose parameters have incompatible types. But more importantly, recalling that constructors are _matched by_ argument names and only then _validated by_ their argument types, the `Edges(int horizontal, int vertical)` constructor is considered a _better_ match here, despite ultimately proving incompatible, because it matches _more_ properties than the 4-argument version. StardewUI won't backtrack; once it decides on the best constructor, it will stick to that constructor even if some argument conversions fail.
        
        While it's very debatable what the right course of action might have been in this specific case, it is impossible for StardewUI to always guess right because the intent of the (hypothetical) author here is _ambiguous_. Were we _trying_ to use the `Horizontal` and `Vertical` edges, and just got the types wrong? Or are they actually totally irrelevant and `Left` is the only value that matters?
        
        You can clear this up by renaming some properties, either directly or via `[DuckProperty]`.

## Enums

Enumeration types are a special case for duck typing that use their own converter. One `enum` can be implicitly converted to any other `enum` as long as the two enums have _at least one_ field (name) that matches. For example:

!!! success

    ```cs
    enum EnumOne { Foo, Bar, Baz }
    
    enum EnumTwo { Foo, Bar, Quux }
    ```

These two enums don't match perfectly, but they have some overlap. If a name is shared, e.g. `Bar`, then `EnumOne.Bar` will convert to `EnumTwo.Bar`. If a name is not shared, e.g. `Baz`, then it will convert to the default value for the target enum, which is always the ordinal value `0`. 

On the other hand, if the two enums have no fields in common:

!!! failure

    ```cs
    enum EnumOne { A, B, C }
    
    enum EnumTwo { X, Y, Z }
    ```

Then conversion between these types is not allowed.

Enums are **not** required to have the same ordinal values; all matching is done on the names, and is case-insensitive.

You can use this feature to incorporate strongly-typed equivalents to enums such as [`Alignment`](../reference/stardewui/layout/alignment.md) in a data model, even if they are part of some larger model. For example, it is possible to mirror a [`NineGridPlacement`](../reference/stardewui/layout/ninegridplacement.md) entirely with a duck type:

!!! success

    ```cs
    public enum MyAlignment { Start, Middle, End }
    
    public record MyNineGridPlacement(
        MyAlignment HorizontalAlignment,
        MyAlignment VerticalAlignment,
        Point Offset);
    ```
    
This type will fully convert to a `NineGridPlacement` without any additional logic.

## Reverse Conversion

So far, this page has primarily described converting from user types to StardewUI types.

Recalling that the destination type must have `[DuckType]` in order to be eligible – what if you want to use duck typing in a [two-way or output binding](starml.md#binding-modifiers), and don't want to take on a hard dependency (assembly reference) to StardewUI?

The good news is that `DuckTypeAttribute` is itself treated as a duck type. To mark one of your own types as being eligible for conversion from a StardewUI framework type, simply copy it and use your copy as the annotation:

```cs
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct,
    AllowMultiple = false,
    Inherited = false)]
public class DuckTypeAttribute : Attribute { }

[DuckType]
public class Foo { ... }
```

In this particular case, you must keep the exact name `DuckTypeAttribute`. Renaming it to something else will cause it to be ignored by StardewUI.