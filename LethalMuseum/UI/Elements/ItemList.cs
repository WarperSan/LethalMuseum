using System;
using LethalMuseum.Objects;
using LethalMuseum.Objects.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace LethalMuseum.UI.Elements;

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
    
    internal Action<bool>? OnActiveChanged;
    
    internal void SetItem(ItemEntry item)
    {
        gameObject.name = item.ID;

        if (icon != null)
        {
            icon.enabled = item.HasCustomIcon;
            icon.sprite = item.Icon;
            icon.color = item.IsBase ? Constants.BLACKED_COLOR : Color.white;
        }

        if (text != null)
        {
            text.text = item.Name;
            text.enabled = !item.HasCustomIcon || item.IsBase;
        }

        if (toggle != null)
            toggle.isOn = Register.IsEnabled(item.ID);
    }
    
    private void ToggleItem(bool isActive)
    {
        OnActiveChanged?.Invoke(isActive);
    }
}