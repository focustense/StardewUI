﻿using HarmonyLib;
using StardewModdingAPI.Events;
using StardewUI.Framework.Api;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Patches;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework;

internal sealed class ModEntry : Mod
{
    // Initialized in Entry
    private ModConfig config = null!;
    private IViewNodeFactory viewNodeFactory = null!;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        UI.Initialize(helper, Monitor);
        config = helper.ReadConfig<ModConfig>();

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

        viewNodeFactory = CreateViewNodeFactory();

        helper.Events.Content.AssetRequested += Content_AssetRequested;
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
        return new ViewEngine(mod.Helper.GameContent, viewNodeFactory, mod.Helper.Events.Content, Monitor);
    }

    private void Content_AssetRequested(object? sender, AssetRequestedEventArgs e)
    {
        if (e.NameWithoutLocale.StartsWith(UiSpriteProvider.AssetNamePrefix))
        {
            e.LoadFrom(() => UiSpriteProvider.GetSprite(e.Name.Name), AssetLoadPriority.Exclusive);
        }
    }

    private IViewNodeFactory CreateViewNodeFactory()
    {
        var viewFactory = new ViewFactory();
        var assetCache = new AssetCache(Helper.GameContent, Helper.Events.Content);
        var valueSourceFactory = new ValueSourceFactory(assetCache);
        var valueConverterFactory = new ValueConverterFactory();
        var attributeBindingFactory = new AttributeBindingFactory(valueSourceFactory, valueConverterFactory);
        var eventBindingFactory = new EventBindingFactory(valueSourceFactory, valueConverterFactory);
        var viewBinder = new ReflectionViewBinder(attributeBindingFactory, eventBindingFactory);
        return new ViewNodeFactory(viewFactory, valueSourceFactory, valueConverterFactory, viewBinder, assetCache);
    }
}
