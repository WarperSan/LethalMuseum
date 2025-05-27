using LethalMuseum.Objects;
using LethalMuseum.Objects.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    
    internal UnityEvent<bool>? OnActiveChanged;
    
    internal void SetItem(ItemEntry item)
    {
        if (icon != null)
            icon.sprite = item.Icon;

        if (text != null)
            text.text = item.Name;

        if (toggle != null)
            toggle.isOn = Register.IsEnabled(item.ID);
    }
    
    private void ToggleItem(bool isActive)
    {
        OnActiveChanged?.Invoke(isActive);
    }
}