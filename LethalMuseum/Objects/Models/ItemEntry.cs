namespace LethalMuseum.Objects.Models;

/// <summary>
/// Class that represents an entry of an item
/// </summary>
public readonly struct ItemEntry
{
    /// <summary>
    /// Original item
    /// </summary>
    public readonly Item Item;
    
    /// <summary>
    /// Precomputed ID of this entry
    /// </summary>
    public readonly string ID;
    
    /// <summary>
    /// Index of the material variant
    /// </summary>
    public readonly int MaterialIndex;
    
    /// <summary>
    /// Index of the mesh variant
    /// </summary>
    public readonly int MeshIndex;

    internal ItemEntry(Item original, int materialIndex = -1, int meshIndex = -1)
    {
        Item = original;

        var id = Identifier.GetID(original);

        if (materialIndex != -1)
            ID = $"{id}/base/{materialIndex}";
        else if (meshIndex != -1)
            ID = $"{id}/{meshIndex}/base";
        else
            ID = id;

        MaterialIndex = materialIndex;
        MeshIndex = meshIndex;
    }

    /// <summary>
    /// Is this entry a variant or not?
    /// </summary>
    public bool IsVariant => MaterialIndex != -1 || MeshIndex != -1;
    
    /// <summary>
    /// Icon of this entry
    /// </summary>
    public UnityEngine.Sprite Icon => Item.itemIcon;

    /// <summary>
    /// Checks if this entry has a custom icon
    /// </summary>
    public bool HasCustomIcon => CheckCustomIcon();
    
    /// <summary>
    /// Name of this entry
    /// </summary>
    public string Name => GetName();

    private string GetName()
    {
        var originalName = Item.itemName ?? "Scrap";
        
        return IsVariant ? originalName + " (Variant)" : originalName;
    }

    private bool CheckCustomIcon()
    {
        var icon = Icon;
        
        if (icon == null)
            return false;
        
        if (icon.name == "ScrapItemIcon2")
            return false;

        if (Dependencies.RuntimeIcons.Dependency.Enabled && !Dependencies.RuntimeIcons.Dependency.HasIconLoaded(icon))
            return false;

        return true;
    }
}