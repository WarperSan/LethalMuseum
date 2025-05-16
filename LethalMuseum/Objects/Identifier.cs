using LethalMuseum.Dependencies.LethalLib;

namespace LethalMuseum.Objects;

/// <summary>
/// Class that handles the identifying of the items
/// </summary>
internal static class Identifier
{
    /// <summary>
    /// Fetches the specific ID of the given item
    /// </summary>
    public static string GetID(GrabbableObject grabbableObject)
    {
        return GetID(grabbableObject.itemProperties);
    }

    /// <summary>
    /// Fetches the generic ID of the given item
    /// </summary>
    public static string GetID(Item item)
    {
        if (ModdedItemIdentifier.GetModdedID(item, out var moddedID) && moddedID != null)
            return moddedID;
        
        return $"Vanilla/{item.itemName}";
    }
}