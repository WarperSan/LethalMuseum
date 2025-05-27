using System.Runtime.CompilerServices;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using UnityEngine;

namespace LethalMuseum.Dependencies.LethalConfig;

internal static class ConfigToUI
{
    public static bool Enabled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(PluginInfo.Guid);

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static void AddConfigs(Configuration? config)
    {
        if (config == null)
            return;

        if (LethalMuseum.MOD_ICON != null)
        {
            var sprite = Sprite.Create(
                LethalMuseum.MOD_ICON,
                new Rect(0f, 0f, LethalMuseum.MOD_ICON.width, LethalMuseum.MOD_ICON.height),
                new Vector2(0.5f, 0.5f)
            );
            LethalConfigManager.SetModIcon(sprite);
        }
        
        LethalConfigManager.SetModDescription("Adds a built-in tracker for Museum%.");
        
        LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(config.AllowBaby, new BoolCheckBoxOptions
        {
            Name = "Allow Baby",
            RequiresRestart = true
        }));
        
        LethalConfigManager.SkipAutoGenFor(config.Blacklist);
    }
}