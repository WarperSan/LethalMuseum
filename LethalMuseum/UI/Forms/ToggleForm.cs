using LethalMuseum.Objects;
using LethalMuseum.Objects.Models;
using LethalMuseum.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace LethalMuseum.UI.Forms;

public class ToggleForm : MonoBehaviour
{
    #region Fields
    
    [Header("Fields")]
    [SerializeField] private Transform? itemListContainer;
    [SerializeField] private GameObject? itemPrefab;
    [SerializeField] private GameObject? noItemText;

    [Header("Controls")]
    [SerializeField] private Toggle? allToggle;
    [SerializeField] private Toggle? scrapsToggle;
    [SerializeField] private Toggle? toolsToggle;
    [SerializeField] private Toggle? basesToggle;
    [SerializeField] private Toggle? variantsToggle;
    [SerializeField] private Toggle? oneHandedToggle;
    [SerializeField] private Toggle? twoHandedToggle;
    [SerializeField] private Toggle? conductiveToggle;
    [SerializeField] private Toggle? batteryToggle;
    [SerializeField] private Toggle? vanillaToggle;
    [SerializeField] private Toggle? moddedToggle;

    [Header("View")]
    [SerializeField] private Toggle? enabledToggle;
    [SerializeField] private Toggle? disabledToggle;

    [Header("Buttons")]
    [SerializeField] private Button? openBtn;
    [SerializeField] private Button? closeBtn;

    #endregion
    
    private void Start()
    {
        var menuManager = FindObjectOfType<MenuManager>();
        
        openBtn?.onClick.AddListener(OpenForm);
        openBtn?.onClick.AddListener(menuManager.PlayConfirmSFX);

        closeBtn?.onClick.AddListener(CloseForm);
        closeBtn?.onClick.AddListener(menuManager.PlayCancelSFX);
        
        allToggle?.onValueChanged.AddListener(onAllToggled);
        AddToggle(scrapsToggle, item => item.Item.isScrap);
        AddToggle(toolsToggle, item => !item.Item.isScrap);
        AddToggle(basesToggle, item => item.IsBase);
        AddToggle(variantsToggle, item => item.IsVariant);
        AddToggle(oneHandedToggle, item => !item.Item.twoHanded);
        AddToggle(twoHandedToggle, item => item.Item.twoHanded);
        AddToggle(conductiveToggle, item => item.Item.isConductiveMetal);
        AddToggle(batteryToggle, item => item.Item.requiresBattery);
        AddToggle(vanillaToggle, item => !item.IsModded);
        AddToggle(moddedToggle, item => item.IsModded);
        
        enabledToggle?.onValueChanged.AddListener(_ => UpdateAllItems());
        disabledToggle?.onValueChanged.AddListener(_ => UpdateAllItems());
    }
    
    #region Animations

    [Header("Animations")]
    [SerializeField] private Animator? animator;

    private void OpenForm()
    {
        UpdateAllItems();

        animator?.SetTrigger(OpenMenu);
    }

    private void CloseForm()
    {
        if (LethalMuseum.Configuration != null)
            LethalMuseum.Configuration.Blacklist.Value = Register.GenerateBlacklist();
        
        animator?.SetTrigger(CloseMenu);
    }

    private static readonly int OpenMenu = Animator.StringToHash("openMenu");
    private static readonly int CloseMenu = Animator.StringToHash("closeMenu");

    #endregion

    #region Items

    private void UpdateAllItems()
    {
        if (itemListContainer == null)
            return;

        var items = Register.GetAll();
        
        foreach (Transform child in itemListContainer)
            Destroy(child.gameObject);

        noItemText?.SetActive(items.Length == 0);

        foreach (var item in items)
        {
            if (!ShowItem(item))
                continue;

            AddItem(item);
        }
    }
    
    private void AddItem(ItemEntry item)
    {
        if (itemListContainer == null || itemPrefab == null)
            return;

        var newItem = Instantiate(itemPrefab, itemListContainer, false);

        newItem.name = item.Name;

        if (newItem.TryGetComponent(out ItemList itemList))
        {
            itemList.SetItem(item);
            itemList.OnActiveChanged += isActive =>
            {
                Register.SetItemEnable(item.ID, isActive);
                UpdateAllItems();
            };
        }
    }

    private bool ShowItem(ItemEntry item)
    {
        var isEnabled = Register.IsEnabled(item.ID);

        if (enabledToggle != null && isEnabled)
            return enabledToggle.isOn;

        if (disabledToggle != null && !isEnabled)
            return disabledToggle.isOn;

        return true;
    }
    
    #endregion

    #region Toggles

    private void AddToggle(Toggle? toggle, System.Func<ItemEntry, bool> condition)
    {
        toggle?.onValueChanged.AddListener(isActive =>
        {
            Register.ApplyFilter(condition, isActive);
            UpdateAllItems();
        });
    }

    private void onAllToggled(bool isActive)
    {
        Register.ApplyFilter(_ => true, isActive);
        UpdateAllItems();

        var toggles = new[]
        {
            scrapsToggle,
            toolsToggle,
            basesToggle,
            variantsToggle,
            oneHandedToggle,
            twoHandedToggle,
            conductiveToggle,
            batteryToggle,
            vanillaToggle,
            moddedToggle
        };

        foreach (var toggle in toggles)
            toggle?.SetIsOnWithoutNotify(isActive);
    }

    #endregion
}