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
    [SerializeField] private Toggle? variantsToggle;
    [SerializeField] private Toggle? oneHandedToggle;
    [SerializeField] private Toggle? twoHandedToggle;
    [SerializeField] private Toggle? conductiveToggle;
    [SerializeField] private Toggle? batteryToggle;

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
        scrapsToggle?.onValueChanged.AddListener(onScrapsToggled);
        toolsToggle?.onValueChanged.AddListener(onToolsToggled);
        variantsToggle?.onValueChanged.AddListener(onVariantsToggled);
        oneHandedToggle?.onValueChanged.AddListener(onOneHandedToggled);
        twoHandedToggle?.onValueChanged.AddListener(onTwoHandedToggled);
        conductiveToggle?.onValueChanged.AddListener(onConductiveToggled);
        batteryToggle?.onValueChanged.AddListener(onBatteryToggled);
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

    private void AddItem(ItemEntry item)
    {
        if (itemListContainer == null || itemPrefab == null)
            return;

        var newItem = Instantiate(itemPrefab, itemListContainer, false);

        if (newItem.TryGetComponent(out ItemList itemList))
        {
            itemList.SetItem(item);
            itemList.OnActiveChanged?.AddListener(isActive => Register.SetItemEnable(item.ID, isActive));
        }
    }

    private void SetItems(ItemEntry[] items)
    {
        if (itemListContainer == null)
            return;
        
        foreach (Transform child in itemListContainer)
            Destroy(child.gameObject);

        noItemText?.SetActive(items.Length == 0);

        foreach (var item in items)
            AddItem(item);
    }

    private void UpdateAllItems()
    {
        var loadedItems = Register.GetAll();
        SetItems(loadedItems);
    }
    
    #endregion

    #region Toggles

    private void onAllToggled(bool isActive)
    {
        Register.ApplyFilter(_ => true, isActive);
        UpdateAllItems();
        
        scrapsToggle?.SetIsOnWithoutNotify(isActive);
        toolsToggle?.SetIsOnWithoutNotify(isActive);
        variantsToggle?.SetIsOnWithoutNotify(isActive);
        oneHandedToggle?.SetIsOnWithoutNotify(isActive);
        twoHandedToggle?.SetIsOnWithoutNotify(isActive);
        conductiveToggle?.SetIsOnWithoutNotify(isActive);
        batteryToggle?.SetIsOnWithoutNotify(isActive);
    }

    private void onScrapsToggled(bool isActive)
    {
        Register.ApplyFilter(item => item.Item.isScrap, isActive);
        UpdateAllItems();
    }
    
    private void onToolsToggled(bool isActive)
    {
        Register.ApplyFilter(item => !item.Item.isScrap, isActive);
        UpdateAllItems();
    }
    
    private void onVariantsToggled(bool isActive)
    {
        Register.ApplyFilter(item => item.IsVariant, isActive);
        UpdateAllItems();
    }
    
    private void onOneHandedToggled(bool isActive)
    {
        Register.ApplyFilter(item => !item.Item.twoHanded, isActive);
        UpdateAllItems();
    }
    
    private void onTwoHandedToggled(bool isActive)
    {
        Register.ApplyFilter(item => item.Item.twoHanded, isActive);
        UpdateAllItems();
    }
    
    private void onConductiveToggled(bool isActive)
    {
        Register.ApplyFilter(item => item.Item.isConductiveMetal, isActive);
        UpdateAllItems();
    }
    
    private void onBatteryToggled(bool isActive)
    {
        Register.ApplyFilter(item => item.Item.requiresBattery, isActive);
        UpdateAllItems();
    }

    #endregion
}