using HarmonyLib;
using LethalMuseum.Helpers;
using LethalMuseum.Objects;

namespace LethalMuseum.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManager_Patches
{
    [HarmonyPatch(nameof(RoundManager.DespawnPropsAtEndOfRound)), HarmonyPostfix]
    private static void DespawnPropsAtEndOfRound_Postfix(RoundManager __instance)
    {
        Logger.Info("DESPAWN PROPS AT THE END OF ROUND");
        Logger.Info(Tracker.Instance == null);
        Tracker.Instance?.ResetCollected();
    }
}