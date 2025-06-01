using LethalMuseum.Objects;
using LethalMuseum.Objects.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace LethalMuseum.UI.Elements;

public class ItemBoard : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image? icon;
    [SerializeField] private TMP_Text? text;
    [SerializeField] private Image? collectedBackground;

    #endregion

    private ItemEntry? shownItem;

    internal void SetItem(ItemEntry item)
    {
        shownItem = item;
        UpdateSelf();
    }

    internal void OnItemUpdate(string id)
    {
        if (!shownItem.HasValue || shownItem.Value.ID != id)
            return;
        
        UpdateSelf();
    }
    
    private void UpdateSelf()
    {
        if (!shownItem.HasValue)
            return;

        if (icon != null)
        {
            icon.enabled = IsIconEnabled(shownItem.Value);
            icon.sprite = shownItem.Value.Icon;
            icon.color = shownItem.Value.IsBase ? new Color(0.3f, 0.3f, 0.3f) : Color.white;
        }

        if (text != null)
        {
            text.enabled = IsTextEnabled(shownItem.Value);
            text.text = shownItem.Value.Name;
        }

        if (collectedBackground != null)
            collectedBackground.enabled = Tracker.Instance?.IsCollected(shownItem.Value.ID) ?? false;
    }

    private static bool IsIconEnabled(ItemEntry item) => item.IsBase || item.HasCustomIcon;

    private static bool IsTextEnabled(ItemEntry item) => item.IsBase || !item.HasCustomIcon;
}