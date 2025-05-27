using System.Runtime.CompilerServices;
using UnityEngine;

namespace LethalMuseum.Dependencies.RuntimeIcons;

internal static class Dependency
{
    public static bool Enabled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(global::RuntimeIcons.MyPluginInfo.PLUGIN_GUID);
    
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool HasIconLoaded(Sprite sprite)
    {
        if (sprite == global::RuntimeIcons.RuntimeIcons.LoadingSprite)
            return false;
        
        if (sprite == global::RuntimeIcons.RuntimeIcons.LoadingSprite2)
            return false;

        return true;
    }
}