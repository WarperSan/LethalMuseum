using LethalMuseum.Objects;
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

        UpdateAllItems();
    }
    
    #region Animations

    [Header("Animations")]
    [SerializeField] private Animator? animator;

    private void OpenForm()
    {
        animator?.SetTrigger(OpenMenu);
    }

    private void CloseForm()
    {
        animator?.SetTrigger(CloseMenu);
    }

    private static readonly int OpenMenu = Animator.StringToHash("openMenu");
    private static readonly int CloseMenu = Animator.StringToHash("closeMenu");

    #endregion

    #region Items

    private void AddItem(Item item)
    {
        if (itemListContainer == null || itemPrefab == null)
            return;

        var newItem = Instantiate(itemPrefab, itemListContainer, false);

        if (newItem.TryGetComponent(out ItemList itemList))
        {
            itemList.SetItem(item);
            itemList.OnActiveChanged?.AddListener(isActive => Register.SetItemEnable(item, isActive));
        }
    }

    private void SetItems(Item[] items)
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
        foreach (var item in Register.GetAll())
            Register.SetItemEnable(item, isActive);
        
        UpdateAllItems();
    }

    private void onScrapsToggled(bool isActive)
    {
        foreach (var item in Register.GetAll())
        {
            if (!item.isScrap)
                continue;
            
            Register.SetItemEnable(item, isActive);
        }
        
        UpdateAllItems();
    }
    
    private void onToolsToggled(bool isActive)
    {
        foreach (var item in Register.GetAll())
        {
            if (item.isScrap)
                continue;
            
            Register.SetItemEnable(item, isActive);
        }
        
        UpdateAllItems();
    }
    
    private void onVariantsToggled(bool isActive)
    {
        
    }
    
    private void onOneHandedToggled(bool isActive)
    {
        foreach (var item in Register.GetAll())
        {
            if (item.twoHanded)
                continue;
            
            Register.SetItemEnable(item, isActive);
        }
        
        UpdateAllItems();
    }
    
    private void onTwoHandedToggled(bool isActive)
    {
        foreach (var item in Register.GetAll())
        {
            if (!item.twoHanded)
                continue;
            
            Register.SetItemEnable(item, isActive);
        }
        
        UpdateAllItems();
    }
    
    private void onConductiveToggled(bool isActive)
    {
        foreach (var item in Register.GetAll())
        {
            if (!item.isConductiveMetal)
                continue;
            
            Register.SetItemEnable(item, isActive);
        }
        
        UpdateAllItems();
    }
    
    private void onBatteryToggled(bool isActive)
    {
        foreach (var item in Register.GetAll())
        {
            if (!item.requiresBattery)
                continue;
            
            Register.SetItemEnable(item, isActive);
        }
        
        UpdateAllItems();
    }

    #endregion
}