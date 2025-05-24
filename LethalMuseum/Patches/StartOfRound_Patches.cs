using HarmonyLib;
using LethalMuseum.Helpers;
using LethalMuseum.Objects;

namespace LethalMuseum.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal class StartOfRound_Patches
{
    [HarmonyPatch(nameof(StartOfRound.ResetShip)), HarmonyPostfix]
    private static void ResetShip_Postfix()
    {
        Logger.Info("RESET SHIP");
        Logger.Info(Tracker.Instance == null);
        Tracker.Instance?.ResetCollected();
    }
}