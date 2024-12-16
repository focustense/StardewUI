using HarmonyLib;
using StardewModdingAPI.Events;
using StardewUI.Framework.Api;
using StardewUI.Framework.Behaviors;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Diagnostics;
using StardewUI.Framework.Patches;
using StardewUI.Framework.Sources;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewValley.Menus;

namespace StardewUI.Framework;

internal sealed class ModEntry : Mod
{
    // Initialized in Entry
    private AssetCache assetCache = null!;
    private ModConfig config = null!;
    private RootResolutionScopeFactory resolutionScopeFactory = null!;
    private SpriteMaps spriteMaps = null!;
    private IViewNodeFactory viewNodeFactory = null!;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        UI.Initialize(helper, Monitor);
        config = helper.ReadConfig<ModConfig>();
        Trace.Writer = new TraceWriter(ModManifest, () => config.Tracing);

        try
        {
            Patcher.Patch(ModManifest.UniqueID);
        }
        catch (Exception ex)
        {
            Monitor.Log(
                $"Harmony patching failed; parts of StardewUI may function incorrectly or not at all.\n{ex}",
                LogLevel.Error
            );
        }

        spriteMaps = new(helper);

        helper.Events.Content.AssetRequested += Content_AssetRequested;
        helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
        helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
        helper.Events.Input.ButtonsChanged += Input_ButtonsChanged;
    }

    public override object? GetApi(IModInfo modInfo)
    {
        var traverse = new Traverse(modInfo);
        var mod = traverse.Property<IMod>("Mod").Value;
        if (mod is null)
        {
            Monitor.Log(
                $"Could not obtain the mod entry for mod {modInfo.Manifest.UniqueID}. API is disabled for this mod.",
                LogLevel.Error
            );
            return null;
        }
        var viewEngine = new ViewEngine(assetCache, mod.Helper, viewNodeFactory);
        resolutionScopeFactory.AddSourceResolver(viewEngine.SourceResolver);
        return viewEngine;
    }

    private void Content_AssetRequested(object? sender, AssetRequestedEventArgs e)
    {
        if (e.NameWithoutLocale.StartsWith(UiSpriteProvider.AssetNamePrefix))
        {
            // Built-in UI sprites point to Game1-instanced textures, so they should not and cannot be localized.
            e.LoadFrom(() => UiSpriteProvider.GetSprite(e.NameWithoutLocale.Name), AssetLoadPriority.Exclusive);
        }
        else if (e.DataType == typeof(Sprite) && e.NameWithoutLocale.StartsWith(ItemSpriteProvider.AssetNamePrefix))
        {
            e.LoadFrom(() => ItemSpriteProvider.GetSprite(e.NameWithoutLocale.Name), AssetLoadPriority.Exclusive);
        }
        else if (e.NameWithoutLocale.StartsWith(SpriteMaps.AssetNamePrefix))
        {
            var spriteMapName = e.NameWithoutLocale.Name[(SpriteMaps.AssetNamePrefix.Length)..];
            if (spriteMapName.StartsWith("buttons", StringComparison.OrdinalIgnoreCase))
            {
                var mapArgString = spriteMapName[7..];
                if (!string.IsNullOrEmpty(mapArgString))
                {
                    if (mapArgString.StartsWith(':'))
                    {
                        mapArgString = mapArgString[1..];
                    }
                    else
                    {
                        Monitor.Log(
                            $"Invalid button sprite map arguments '{mapArgString}'. "
                                + "Must either be omitted or start with a ':' character.",
                            LogLevel.Warn
                        );
                        // Invalid format.
                        return;
                    }
                }
                var mapArgs = !string.IsNullOrEmpty(mapArgString) ? mapArgString.Split('-') : [];
                string? keyboardThemeName = mapArgs.Length > 0 ? mapArgs[0] : null;
                string? mouseThemeName = mapArgs.Length > 1 ? mapArgs[1] : null;
                float sliceScale = mapArgs.Length > 2 ? float.Parse(mapArgs[2]) : 1f;
                e.LoadFrom(
                    () => spriteMaps.GetButtonSpriteMap(keyboardThemeName, mouseThemeName, sliceScale),
                    AssetLoadPriority.Exclusive
                );
            }
            else if (spriteMapName.StartsWith("directions", StringComparison.OrdinalIgnoreCase))
            {
                e.LoadFrom(() => spriteMaps.GetDirectionSpriteMap(), AssetLoadPriority.Exclusive);
            }
        }
    }

    private IViewNodeFactory CreateViewNodeFactory()
    {
        var loadOrder = UI.ResolveLoadOrder();
        var addonViewFactories = loadOrder
            .Reverse()
            .Select(addon => addon.ViewFactory)
            .Where(factory => factory is not null)
            .Cast<IViewFactory>();
        var rootViewFactory = new RootViewFactory(addonViewFactories);
        assetCache = new AssetCache(Helper.GameContent, Helper.Events.Content);
        var valueSourceFactory = new ValueSourceFactory(assetCache);
        var addonValueConverterFactories = loadOrder
            .Reverse()
            .Select(addon => addon.ValueConverterFactory)
            .Where(factory => factory is not null)
            .Cast<IValueConverterFactory>();
        var rootValueConverterFactory = new RootValueConverterFactory(addonValueConverterFactories);
        var attributeBindingFactory = new AttributeBindingFactory(valueSourceFactory, rootValueConverterFactory);
        var eventBindingFactory = new EventBindingFactory(valueSourceFactory, rootValueConverterFactory);
        var viewBinder = new ReflectionViewBinder(attributeBindingFactory, eventBindingFactory);
        var addonBehaviorFactories = loadOrder
            .Reverse()
            .Select(addon => addon.BehaviorFactory)
            .Where(factory => factory is not null)
            .Cast<IBehaviorFactory>();
        var rootBehaviorFactory = new RootBehaviorFactory(addonBehaviorFactories);
        resolutionScopeFactory = new(Helper.ModRegistry);
        if (config.Performance.EnableReflectionWarmup)
        {
            Warmups.WarmupAttributeBindingFactory(attributeBindingFactory, eventBindingFactory, valueSourceFactory);
        }
        return new ViewNodeFactory(
            rootViewFactory,
            valueSourceFactory,
            rootValueConverterFactory,
            viewBinder,
            assetCache,
            resolutionScopeFactory,
            rootBehaviorFactory
        );
    }

    [EventPriority(EventPriority.Normal + 1)]
    private void GameLoop_GameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        // This is done in GameLaunched instead of Entry in order to give mods a chance to register add-ons from their
        // own Entry methods.
        //
        // If add-on mods do the right thing and declare a SMAPI dependency on StardewUI, it guarantees that our Entry
        // method will run before theirs, which is the opposite of what we want for add-ons; we need to run after they
        // have completed their registrations.
        //
        // For the same reason, this handler should be kept at slightly below normal priority in case naive add-on mods
        // decide to do their registrations in their own GameLaunched handler instead of in their Entry, and don't
        // adjust their own priority.
        //
        // SMAPI promises to run this event before the first update tick, so it is still safe to do initialization here
        // and not risk exceptions in the UpdateTicked handler.
        viewNodeFactory = CreateViewNodeFactory();
    }

    private void GameLoop_UpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        ContextUpdateTracker.Instance.Reset();
        assetCache.Update(Game1.currentGameTime.ElapsedGameTime);
    }

    private void Input_ButtonsChanged(object? sender, ButtonsChangedEventArgs e)
    {
        if (config.Tracing.ToggleHotkeys.JustPressed())
        {
            Trace.IsTracing = !Trace.IsTracing;
            Helper.Input.SuppressActiveKeybinds(config.Tracing.ToggleHotkeys);
        }

        if (
            Game1.activeClickableMenu is TitleMenu
            && TitleMenu.subMenu is DocumentViewMenu { CloseAction: not null } viewMenu
        )
        {
            foreach (var button in e.Pressed)
            {
                if (ButtonResolver.GetButtonAction(button) != ButtonAction.Cancel)
                {
                    continue;
                }
                if (button.TryGetKeyboard(out var key))
                {
                    viewMenu.receiveKeyPress(key);
                }
                else if (button.TryGetController(out var controllerButton))
                {
                    viewMenu.receiveGamePadButton(controllerButton);
                }
            }
        }
    }
}

file static class EnumerableExtensions
{
    public static IEnumerable<T> Reverse<T>(this IReadOnlyList<T> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            yield return list[i];
        }
    }
}
