using Microsoft.Xna.Framework;
using StardewModdingAPI.Utilities;
using StardewUI.Animation;
using StardewUI.Data;
using StardewUI.Layout;
using StardewUI.Widgets;
using StardewValley.ItemTypeDefinitions;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Converter factory used as the root in StardewUI apps, including all built-in converters.
/// </summary>
internal class RootValueConverterFactory : ValueConverterFactory
{
    /// <summary>
    /// Initializes a new <see cref="RootValueConverterFactory"/> with the specified add-on factories.
    /// </summary>
    /// <param name="addonFactories">Converter factories registered by add-ons, in order of priority. All add-on
    /// factories are considered after the standard conversions, but before fallback conversions (duck-type,
    /// ToString).</param>
    public RootValueConverterFactory(IEnumerable<IValueConverterFactory> addonFactories)
    {
        RegisterDefaults();
        Factories.AddRange(addonFactories);
        RegisterFallbackDefaults();
    }

    // High-priority defaults that take precedence over user-defined conversions. These are all documented, tested,
    // etc., and users are likely to depend on their default behavior; therefore we register them first so that add-ons
    // cannot override them.
    private void RegisterDefaults()
    {
        // Automatically register string to primitive conversions.
        TryRegister<string, byte>(byte.Parse);
        TryRegister<string, sbyte>(sbyte.Parse);
        TryRegister<string, short>(short.Parse);
        TryRegister<string, ushort>(ushort.Parse);
        TryRegister<string, int>(int.Parse);
        TryRegister<string, uint>(uint.Parse);
        TryRegister<string, long>(long.Parse);
        TryRegister<string, ulong>(ulong.Parse);
        TryRegister<string, decimal>(decimal.Parse);
        TryRegister<string, float>(float.Parse);
        TryRegister<string, double>(double.Parse);
        TryRegister<string, bool>(bool.Parse);

        // Primitives have many permutations, are registered in their own function.
        RegisterPrimitives();

        // Convenience defaults for non-primitive types that are commonly specified as literals.
        TryRegister(new ColorConverter());
        TryRegister<string, Easing>(Easings.Parse);
        TryRegister<string, Edges>(Edges.Parse);
        TryRegister<string, FloatingPosition>(FloatingPosition.Parse);
        TryRegister<string, GridItemLayout>(GridItemLayout.Parse);
        TryRegister(new LayoutConverter());
        TryRegister<string, Length>(Length.Parse);
        TryRegister(new NamedFontConverter());
        TryRegister<string, NineGridPlacement>(NineGridPlacement.Parse);
        TryRegister(new PointConverter());
        TryRegister<string, Rectangle>(Parsers.ParseRectangle);
        TryRegister(new TransformConverter());
        TryRegister<string, Transition>(Transition.Parse);
        TryRegister<string, Vector2>(Parsers.ParseVector2);

        // Edges are better to bind as numbers, so we can use tuples and XNA equivalents in some cases.
        TryRegister<int, Edges>(all => new(all));
        TryRegister<(int, int), Edges>(t => new(t.Item1, t.Item2));
        TryRegister<Tuple<int, int>, Edges>(t => new(t.Item1, t.Item2));
        TryRegister<Point, Edges>(p => new(p.X, p.Y));
        TryRegister<Vector2, Edges>(v => new((int)v.X, (int)v.Y));
        TryRegister<(int, int, int, int), Edges>(t => new(t.Item1, t.Item2, t.Item3, t.Item4));
        TryRegister<Tuple<int, int, int, int>, Edges>(t => new(t.Item1, t.Item2, t.Item3, t.Item4));
        TryRegister<(Point, Point), Edges>(t => new(t.Item1.X, t.Item1.Y, t.Item2.X, t.Item2.Y));
        TryRegister<Tuple<Point, Point>, Edges>(t => new(t.Item1.X, t.Item1.Y, t.Item2.X, t.Item2.Y));
        TryRegister<(Vector2, Vector2), Edges>(t =>
            new((int)t.Item1.X, (int)t.Item1.Y, (int)t.Item2.X, (int)t.Item2.Y)
        );
        TryRegister<Tuple<Vector2, Vector2>, Edges>(t =>
            new((int)t.Item1.X, (int)t.Item1.Y, (int)t.Item2.X, (int)t.Item2.Y)
        );
        TryRegister<Vector4, Edges>(v => new((int)v.X, (int)v.Y, (int)v.Z, (int)v.W));
        // And the reverse conversions, where applicable...
        TryRegister<Edges, (int, int, int, int)>(e => (e.Left, e.Top, e.Right, e.Bottom));
        TryRegister<Edges, Tuple<int, int, int, int>>(e => Tuple.Create(e.Left, e.Top, e.Right, e.Bottom));
        TryRegister<Edges, Vector4>(e => new(e.Left, e.Top, e.Right, e.Bottom));

        // Bounds are similar to edges, except we never accept them as inputs, so only need reverse conversions.
        TryRegister<Bounds, (float, float, float, float)>(b => (b.Left, b.Top, b.Right, b.Bottom));
        TryRegister<Bounds, Tuple<float, float, float, float>>(b => Tuple.Create(b.Left, b.Top, b.Right, b.Bottom));
        TryRegister<Bounds, (Vector2, Vector2)>(b => (b.Position, b.Size));
        TryRegister<Bounds, Tuple<Vector2, Vector2>>(b => Tuple.Create(b.Position, b.Size));
        TryRegister<Bounds, Vector4>(b => new(b.Left, b.Top, b.Right, b.Bottom));
        TryRegister<Bounds, Rectangle>(b => new(b.Position.ToPoint(), b.Size.ToPoint()));

        // Several converters for just the Sprite type, as it can be surprisingly complex and users won't have the exact
        // type.
        TryRegister(new ItemSpriteConverter());
        TryRegister(new TextureSpriteConverter());
        TryRegister(new TextureRectSpriteConverter());

        // SMAPI types
        TryRegister<string, Keybind>(s =>
            Keybind.TryParse(s, out var keybind, out _)
                ? keybind
                : throw new FormatException($"Invalid keybind format: '{s}'.")
        );
        TryRegister<string, KeybindList>(KeybindList.Parse);
        TryRegister<SButton, Keybind>(button => new(button));
        TryRegister<SButton, KeybindList>(button => new(button));
        TryRegister<Keybind, SButton>(ConvertKeybindToButton);
        TryRegister<KeybindList, Keybind>(list =>
            list.Keybinds.Length switch
            {
                0 => new(SButton.None),
                1 => list.Keybinds[0],
                _ => throw new ArgumentException(
                    $"Cannot convert {nameof(KeybindList)} with {list.Keybinds.Length} buttons to {nameof(Keybind)}.",
                    nameof(list)
                ),
            }
        );
        TryRegister<KeybindList, SButton>(list =>
            list.Keybinds.Length switch
            {
                0 => SButton.None,
                1 => ConvertKeybindToButton(list.Keybinds[0]),
                _ => throw new ArgumentException(
                    $"Cannot convert {nameof(KeybindList)} with {list.Keybinds.Length} buttons to {nameof(SButton)}.",
                    nameof(list)
                ),
            }
        );

        // Tooltips have a lot of different parameters; these are some common conversions.
        TryRegister<string, TooltipData>(text => new(text));
        TryRegister<(string, string), TooltipData>(t => new(t.Item2, t.Item1));
        TryRegister<Tuple<string, string>, TooltipData>(t => new(t.Item2, t.Item1));
        TryRegister<ParsedItemData, TooltipData>(data =>
            new(data.Description, data.DisplayName, ItemRegistry.Create(data.ItemId))
        );
        TryRegister<Item, TooltipData>(item => new(item.getDescription(), item.DisplayName, item));

        // Extra conversions for floats.
        TryRegister<Func<Vector2, Vector2, Vector2>, FloatingPosition>(f => new(f));

        // Only two types of visibility, so boolean conversions are common.
        TryRegister<bool, Visibility>(b => b ? Visibility.Visible : Visibility.Hidden);

        // Most enums are fine using the standard string-to-enum conversion.
        Register(new EnumNameConverterFactory());

        // If source and destination are the same, use a pass-through converter.
        Register(new IdentityValueConverterFactory());
        Register(new AnyCastConverterFactory());
        Register(new CastingValueConverterFactory());
        Register(new NullableConverterFactory(this));
    }

    private static SButton ConvertKeybindToButton(Keybind keybind)
    {
        return keybind.Buttons.Length switch
        {
            0 => SButton.None,
            1 => keybind.Buttons[0],
            _ => throw new ArgumentException(
                $"Cannot convert {nameof(Keybind)} with {keybind.Buttons.Length} buttons to {nameof(SButton)}.",
                nameof(keybind)
            ),
        };
    }

    // Converters that have lower priority than user-defined conversions; we only use these when there is definitely no
    // better option available.
    private void RegisterFallbackDefaults()
    {
        // Anything can generally be converted to a string using the default converter.
        Register(new StringConverterFactory());

        // Duck typing is the most expensive to even attempt, regardless of success. Don't do it unless we're sure
        // there aren't any other conversion options.
        Register(new DuckTypeEnumConverterFactory());
        Register(new DuckTypeClassConverterFactory(this));
    }

    private void RegisterPrimitives()
    {
        // Numeric conversions. These may be lossy, but the same types of conversions are supported between e.g. Vectors
        // and Points, so there is no reason not to allow them; in many instances they'd be assignable in code due to
        // implicit conversions.
        //
        // This would be far cleaner in later versions of .NET with generic math support. In .NET 6, we don't have that,
        // and a reflection/ILgen-based approach would be orders of magnitude slower. Since the number of combinations
        // is limited, albeit large, it's better to specify them explicitly this way.

        TryRegister<byte, sbyte>(v => (sbyte)v);
        TryRegister<byte, short>(v => v);
        TryRegister<byte, ushort>(v => v);
        TryRegister<byte, int>(v => v);
        TryRegister<byte, uint>(v => v);
        TryRegister<byte, long>(v => v);
        TryRegister<byte, ulong>(v => v);
        TryRegister<byte, decimal>(v => v);
        TryRegister<byte, float>(v => v);
        TryRegister<byte, double>(v => v);

        TryRegister<sbyte, byte>(v => (byte)v);
        TryRegister<sbyte, short>(v => v);
        TryRegister<sbyte, ushort>(v => (ushort)v);
        TryRegister<sbyte, int>(v => v);
        TryRegister<sbyte, uint>(v => (uint)v);
        TryRegister<sbyte, long>(v => v);
        TryRegister<sbyte, ulong>(v => (ulong)v);
        TryRegister<sbyte, decimal>(v => v);
        TryRegister<sbyte, float>(v => v);
        TryRegister<sbyte, double>(v => v);

        TryRegister<short, sbyte>(v => (sbyte)v);
        TryRegister<short, byte>(v => (byte)v);
        TryRegister<short, ushort>(v => (ushort)v);
        TryRegister<short, int>(v => v);
        TryRegister<short, uint>(v => (uint)v);
        TryRegister<short, long>(v => v);
        TryRegister<short, ulong>(v => (ulong)v);
        TryRegister<short, decimal>(v => v);
        TryRegister<short, float>(v => v);
        TryRegister<short, double>(v => v);

        TryRegister<ushort, sbyte>(v => (sbyte)v);
        TryRegister<ushort, byte>(v => (byte)v);
        TryRegister<ushort, short>(v => (short)v);
        TryRegister<ushort, int>(v => v);
        TryRegister<ushort, uint>(v => v);
        TryRegister<ushort, long>(v => v);
        TryRegister<ushort, ulong>(v => v);
        TryRegister<ushort, decimal>(v => v);
        TryRegister<ushort, float>(v => v);
        TryRegister<ushort, double>(v => v);

        TryRegister<int, sbyte>(v => (sbyte)v);
        TryRegister<int, byte>(v => (byte)v);
        TryRegister<int, short>(v => (short)v);
        TryRegister<int, ushort>(v => (ushort)v);
        TryRegister<int, uint>(v => (uint)v);
        TryRegister<int, long>(v => v);
        TryRegister<int, ulong>(v => (ulong)v);
        TryRegister<int, decimal>(v => v);
        TryRegister<int, float>(v => v);
        TryRegister<int, double>(v => v);

        TryRegister<uint, sbyte>(v => (sbyte)v);
        TryRegister<uint, byte>(v => (byte)v);
        TryRegister<uint, short>(v => (short)v);
        TryRegister<uint, ushort>(v => (ushort)v);
        TryRegister<uint, int>(v => (int)v);
        TryRegister<uint, long>(v => v);
        TryRegister<uint, ulong>(v => v);
        TryRegister<uint, decimal>(v => v);
        TryRegister<uint, float>(v => v);
        TryRegister<uint, double>(v => v);

        TryRegister<long, sbyte>(v => (sbyte)v);
        TryRegister<long, byte>(v => (byte)v);
        TryRegister<long, short>(v => (short)v);
        TryRegister<long, ushort>(v => (ushort)v);
        TryRegister<long, int>(v => (int)v);
        TryRegister<long, uint>(v => (uint)v);
        TryRegister<long, ulong>(v => (ulong)v);
        TryRegister<long, decimal>(v => v);
        TryRegister<long, float>(v => v);
        TryRegister<long, double>(v => v);

        TryRegister<ulong, sbyte>(v => (sbyte)v);
        TryRegister<ulong, byte>(v => (byte)v);
        TryRegister<ulong, short>(v => (short)v);
        TryRegister<ulong, ushort>(v => (ushort)v);
        TryRegister<ulong, int>(v => (int)v);
        TryRegister<ulong, uint>(v => (uint)v);
        TryRegister<ulong, long>(v => (long)v);
        TryRegister<ulong, decimal>(v => v);
        TryRegister<ulong, float>(v => v);
        TryRegister<ulong, double>(v => v);

        TryRegister<decimal, sbyte>(v => (sbyte)Math.Round(v));
        TryRegister<decimal, byte>(v => (byte)Math.Round(v));
        TryRegister<decimal, short>(v => (short)Math.Round(v));
        TryRegister<decimal, ushort>(v => (ushort)Math.Round(v));
        TryRegister<decimal, int>(v => (int)Math.Round(v));
        TryRegister<decimal, uint>(v => (uint)Math.Round(v));
        TryRegister<decimal, long>(v => (long)Math.Round(v));
        TryRegister<decimal, ulong>(v => (ulong)Math.Round(v));
        TryRegister<decimal, float>(v => (float)v);
        TryRegister<decimal, double>(v => (double)v);

        TryRegister<float, sbyte>(v => (sbyte)MathF.Round(v));
        TryRegister<float, byte>(v => (byte)MathF.Round(v));
        TryRegister<float, short>(v => (short)MathF.Round(v));
        TryRegister<float, ushort>(v => (ushort)MathF.Round(v));
        TryRegister<float, int>(v => (int)MathF.Round(v));
        TryRegister<float, uint>(v => (uint)MathF.Round(v));
        TryRegister<float, long>(v => (long)MathF.Round(v));
        TryRegister<float, ulong>(v => (ulong)MathF.Round(v));
        TryRegister<float, decimal>(v => (decimal)v);
        TryRegister<float, double>(v => v);

        TryRegister<double, sbyte>(v => (sbyte)Math.Round(v));
        TryRegister<double, byte>(v => (byte)Math.Round(v));
        TryRegister<double, short>(v => (short)Math.Round(v));
        TryRegister<double, ushort>(v => (ushort)Math.Round(v));
        TryRegister<double, int>(v => (int)Math.Round(v));
        TryRegister<double, uint>(v => (uint)Math.Round(v));
        TryRegister<double, long>(v => (long)Math.Round(v));
        TryRegister<double, ulong>(v => (ulong)Math.Round(v));
        TryRegister<double, decimal>(v => (decimal)v);
        TryRegister<double, float>(v => (float)v);
    }
}
