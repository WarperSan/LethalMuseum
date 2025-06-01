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
    
    public ItemEntry(Item original)
    {
        Item = original;
        ID = Identifier.GetID(original);
        MaterialIndex = -1;
        MeshIndex = -1;
    }
    
    private ItemEntry(ItemEntry baseEntry, int materialIndex, int meshIndex)
    {
        Item = baseEntry.Item;

        var id = Identifier.GetID(baseEntry.Item);

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
    /// Creates a mesh variant of this entry 
    /// </summary>
    public ItemEntry Mesh(int index) => new(this, -1, index);
    
    /// <summary>
    /// Creates a material variant of this entry 
    /// </summary>
    public ItemEntry Material(int index) => new(this, index, -1);

    /// <summary>
    /// Is this entry a variant or not?
    /// </summary>
    public bool IsVariant => MaterialIndex != -1 || MeshIndex != -1;

    /// <summary>
    /// Does this entry have any variant?
    /// </summary>
    public bool HasVariants => Item.materialVariants.Length > 0 || Item.meshVariants.Length > 0;

    /// <summary>
    /// Is this entry the base or not?
    /// </summary>
    public bool IsBase => HasVariants && !IsVariant;

    /// <summary>
    /// Icon of this entry
    /// </summary>
    public UnityEngine.Sprite Icon => GetIcon();

    /// <summary>
    /// Checks if this entry has a custom icon
    /// </summary>
    public bool HasCustomIcon => GetHasCustomIcon();
    
    /// <summary>
    /// Name of this entry
    /// </summary>
    public string Name => GetName();

    /// <summary>
    /// Checks if this entry is about a modded item
    /// </summary>
    public bool IsModded => Identifier.IsItemModded(Item);

    private UnityEngine.Sprite GetIcon()
    {
        if (!IsVariant)
            return Item.itemIcon;

        if (
            Dependencies.RuntimeIcons.Dependency.Enabled &&
            Dependencies.RuntimeIcons.Dependency.HasCustomIcon(this, out var icon) &&
            icon != null
        )
            return icon;

        return Item.itemIcon;
    }

    private string GetName()
    {
        if (HasCustomIcon && IsBase)
            return "Any";
        
        var originalName = Item.itemName ?? "Scrap";

        if (IsVariant)
            return originalName + " (Variant)";

        return originalName;
    }

    private bool GetHasCustomIcon()
    {
        var icon = Icon;
        
        if (icon == null)
            return false;
        
        if (icon.name == "ScrapItemIcon2")
            return false;

        if (Dependencies.RuntimeIcons.Dependency.Enabled && Dependencies.RuntimeIcons.Dependency.IsLoadingIcon(icon))
            return false;
        
        return true;
    }
}