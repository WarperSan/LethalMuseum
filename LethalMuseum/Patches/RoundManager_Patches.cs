using HarmonyLib;
using LethalMuseum.Objects;

namespace LethalMuseum.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManager_Patches
{
    [HarmonyPatch(nameof(RoundManager.CollectNewScrapForThisRound)), HarmonyPostfix]
    public static void CollectNewScrapForThisRound_Postfix(GrabbableObject scrapObject) => Tracker.Instance?.Collect(scrapObject);
}