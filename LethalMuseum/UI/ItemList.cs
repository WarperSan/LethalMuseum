using LethalMuseum.Dependencies.LethalLib;
using LethalMuseum.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace LethalMuseum.UI;

public class ItemList : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image? icon;
    [SerializeField] private TMP_Text? text;
    [SerializeField] private Toggle? toggle;

    #endregion
    
    private void Start()
    {
        toggle?.onValueChanged.AddListener(ToggleItem);
    }
    
    public UnityEvent<bool>? OnActiveChanged;
    
    internal void SetItem(Item item)
    {
        var showIcon = item.itemIcon != null && item.itemIcon.name != Constants.SCRAP_ICON_NAME;
        var showText = !showIcon;

        if (icon != null)
        {
            icon.enabled = showIcon;
            icon.sprite = item.itemIcon;
        }

        if (text != null)
        {
            text.enabled = showText;
            text.text = item.itemName;
        }

        if (toggle != null)
        {
            var id = ItemIdentifier.GetID(item);
            toggle.isOn = Register.IsAllowed(id);
        }
    }
    
    private void ToggleItem(bool isActive)
    {
        OnActiveChanged?.Invoke(isActive);
    }
}