using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;

namespace LethalMuseum.Dependencies.InputUtils;

internal static class Dependency
{
    public static bool Enabled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LethalCompanyInputUtils.PluginInfo.PLUGIN_GUID);

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool AllowKeybind(InputAction.CallbackContext ctx)
    {
        var localPlayer = StartOfRound.Instance?.localPlayerController;

        if (localPlayer == null)
            return true;

        if (localPlayer.inTerminalMenu)
            return false;

        if (localPlayer.isTypingChat)
            return false;

        return true;
    }
}