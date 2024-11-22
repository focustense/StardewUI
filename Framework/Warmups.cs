using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewUI.Data;
using StardewUI.Events;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Sources;
using StardewUI.Framework.Views;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewUI.Widgets;
using StardewUI.Widgets.Keybinding;
using StardewValley.ItemTypeDefinitions;

namespace StardewUI.Framework;

/// <summary>
/// Helpers for warming up various caches that can sometimes be slow to lazy-load.
/// </summary>
internal static class Warmups
{
    internal static void WarmupAttributeBindingFactory(
        AttributeBindingFactory attributeBindingFactory,
        EventBindingFactory eventBindingFactory,
        ValueSourceFactory valueSourceFactory
    )
    {
        // The generic version of Warmup should take almost no time at all, so it's worth it to register everything we
        // can reasonably expect at compile time in order to avoid costly reflection later.
        //
        // None of these entries have any *required* version, and all can be removed safely; they simply provide a
        // slight performance boost (saving a few frames, or several frames) the first time bindings are created.
        attributeBindingFactory.Warmup<string, string>();
        RegisterStringAndSelfWithNullable<bool>();
        RegisterStringAndSelfWithNullable<sbyte>();
        RegisterStringAndSelfWithNullable<byte>();
        RegisterStringAndSelfWithNullable<short>();
        RegisterStringAndSelfWithNullable<ushort>();
        RegisterStringAndSelfWithNullable<int>();
        RegisterStringAndSelfWithNullable<uint>();
        RegisterStringAndSelfWithNullable<long>();
        RegisterStringAndSelfWithNullable<ulong>();
        RegisterStringAndSelfWithNullable<Half>();
        RegisterStringAndSelfWithNullable<float>();
        RegisterStringAndSelfWithNullable<double>();
        RegisterStringAndSelfWithNullable<Alignment>();
        RegisterStringAndSelf<Bounds>();
        RegisterStringAndSelfWithNullable<Color>();
        RegisterStringAndSelfWithNullable<Direction>();
        RegisterStringAndSelf<Edges>();
        RegisterStringAndSelf<GridItemLayout>();
        RegisterStringAndSelfWithNullable<ImageFit>();
        RegisterStringAndSelfWithNullable<LayoutParameters>();
        RegisterStringAndSelfWithNullable<Length>();
        RegisterStringAndSelfWithNullable<Orientation>();
        RegisterStringAndSelfWithNullable<Point>();
        RegisterStringAndSelfWithNullable<Rectangle>();
        RegisterStringAndSelfWithNullable<SimpleRotation>();
        RegisterStringAndSelf<SpriteFont>();
        RegisterStringAndSelfWithNullable<Vector2>();
        RegisterStringAndSelfWithNullable<Visibility>();

        // All conversions in the RootValueConverterFactory are expected to happen at some point.
        // TODO: We can probably do a better job of this with a source generator. For now it's copypasta.
        RegisterConversion<int, Edges>();
        RegisterConversion<(int, int), Edges>();
        RegisterConversion<Tuple<int, int>, Edges>();
        RegisterConversion<Point, Edges>();
        RegisterConversion<Vector2, Edges>();
        RegisterConversion<(int, int, int, int), Edges>();
        RegisterConversion<Tuple<int, int, int, int>, Edges>();
        RegisterConversion<(Point, Point), Edges>();
        RegisterConversion<Tuple<Point, Point>, Edges>();
        RegisterConversion<(Vector2, Vector2), Edges>();
        RegisterConversion<Tuple<Vector2, Vector2>, Edges>();
        RegisterConversion<Vector4, Edges>();
        RegisterConversion<Edges, (int, int, int, int)>();
        RegisterConversion<Edges, Tuple<int, int, int, int>>();
        RegisterConversion<Edges, Vector4>();

        RegisterConversion<Bounds, (float, float, float, float)>();
        RegisterConversion<Bounds, Tuple<float, float, float, float>>();
        RegisterConversion<Bounds, (Vector2, Vector2)>();
        RegisterConversion<Bounds, Tuple<Vector2, Vector2>>();
        RegisterConversion<Bounds, Vector4>();
        RegisterConversion<Bounds, Rectangle>();

        RegisterConversion<ParsedItemData, Sprite>();
        RegisterConversion<Texture2D, Sprite>();
        RegisterConversion<Tuple<Texture2D, Rectangle>, Sprite>();
        attributeBindingFactory.Warmup<Sprite, Sprite>();

        RegisterConversion<string, TooltipData>();
        RegisterConversion<(string, string), TooltipData>();
        RegisterConversion<Tuple<string, string>, TooltipData>();
        RegisterConversion<ParsedItemData, TooltipData>();
        RegisterConversion<Item, TooltipData>();

        // In addition to the fast, closed generics, we want to do at least one reflection-based warmup for each of the
        // reflection-heavy services in order to cache the hidden state of MakeGenericMethod/MakeGenericType that makes
        // subsequent iterations faster.
        //
        // Since these are themselves fairly expensive, they are done on a worker thread, sequentially, to limit the
        // possibility of interference with the main thread during game start.
        Task.Run(() =>
        {
            ReflectionEventDescriptor.Warmup();
            ReflectionMethodDescriptor.Warmup();
            ThisPropertyDescriptor.Warmup();
            valueSourceFactory.Warmup();
            ConvertedValueSource.Warmup(typeof(object), typeof(object));
            attributeBindingFactory.Warmup(typeof(object), typeof(object));
            eventBindingFactory.Warmup(typeof(BubbleEventArgs));
            eventBindingFactory.Warmup(typeof(ButtonEventArgs));
            eventBindingFactory.Warmup(typeof(ClickEventArgs));
            eventBindingFactory.Warmup(typeof(PointerEventArgs));
            eventBindingFactory.Warmup(typeof(PointerMoveEventArgs));
            eventBindingFactory.Warmup(typeof(WheelEventArgs));
            WarmupViewNodes();
            RepeaterNode.Warmup();
            NullableConverterFactory.Warmup();
            LazyExpressionFieldDescriptor.Warmup();
        });

        void RegisterConversion<TSource, TDestination>()
        {
            attributeBindingFactory.Warmup<TSource, TDestination>();
            ConvertedValueSource.Warmup<TSource, TDestination>();
        }

        void RegisterStringAndSelf<T>()
            where T : notnull
        {
            attributeBindingFactory.Warmup<T, T>();
            attributeBindingFactory.Warmup<string, T>();
            ConvertedValueSource.Warmup<string, T>();
            attributeBindingFactory.Warmup<T, string>();
            ConvertedValueSource.Warmup<T, string>();
        }

        void RegisterStringAndSelfWithNullable<T>()
            where T : struct
        {
            RegisterStringAndSelf<T>();
            attributeBindingFactory.Warmup<T, T?>();
            attributeBindingFactory.Warmup<T?, T>();
            attributeBindingFactory.Warmup<T?, T?>();
            attributeBindingFactory.Warmup<string, T?>();
        }
    }

    private static void WarmupViewNodes()
    {
        ViewNode.Warmup<Banner>();
        ViewNode.Warmup<Button>();
        ViewNode.Warmup<CheckBox>();
        ViewNode.Warmup<DynamicDropDownList>();
        ViewNode.Warmup<Expander>();
        ViewNode.Warmup<Frame>();
        ViewNode.Warmup<GhostView>();
        ViewNode.Warmup<Grid>();
        ViewNode.Warmup<Image>();
        ViewNode.Warmup<KeybindView>();
        ViewNode.Warmup<KeybindListEditor>();
        ViewNode.Warmup<Label>();
        ViewNode.Warmup<Lane>();
        ViewNode.Warmup<Marquee>();
        ViewNode.Warmup<NineGridPlacementEditor>();
        ViewNode.Warmup<ScrollableFrameView>();
        ViewNode.Warmup<ScrollableView>();
        ViewNode.Warmup<Scrollbar>();
        ViewNode.Warmup<ScrollContainer>();
        ViewNode.Warmup<Slider>();
        ViewNode.Warmup<Spacer>();
        ViewNode.Warmup<Tab>();
        ViewNode.Warmup<TextInput>();
        ViewNode.Warmup<TinyNumberLabel>();
    }
}
