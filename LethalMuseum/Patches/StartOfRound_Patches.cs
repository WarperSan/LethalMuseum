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
        Tracker.Instance?.ResetCollected();
    }
    
    [HarmonyPatch(nameof(StartOfRound.AllPlayersHaveRevivedClientRpc)), HarmonyPostfix]
    private static void AllPlayersHaveRevivedClientRpc_Postfix(StartOfRound __instance) => Tracker.Instance?.ResetCollected();
}