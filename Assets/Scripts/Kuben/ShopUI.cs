using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Item Data")]
    public WeaponData weaponData;

    [Header("UI Components")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;

    [Header("Interaction")]
    public Button buyUpgradeButton;
    public Button equipButton;

    private void Start()
    {
        buyUpgradeButton.onClick.AddListener(OnBuyClicked);
        equipButton.onClick.AddListener(OnEquipClicked);

        // Subscribe to global shop events to ensure all cards update simultaneously
        ShopManager.Instance.OnShopChanged += UpdateDisplay;

        UpdateDisplay();
    }

    private void OnDestroy()
    {
        if (ShopManager.Instance != null)
            ShopManager.Instance.OnShopChanged -= UpdateDisplay;
    }

    private void OnBuyClicked()
    {
        ShopManager.Instance.TryBuyWeapon(weaponData);
    }

    private void OnEquipClicked()
    {
        ShopManager.Instance.EquipWeapon(weaponData);
    }

    public void UpdateDisplay()
    {
        if (weaponData != null)
        {
            // Set Static Information
            if (iconImage != null) iconImage.sprite = weaponData.icon;
            nameText.text = weaponData.weaponName;

            double cost = weaponData.GetCost();

            // Determine if the item is Unlocked (Buy) or Owned (Upgrade)
            if (weaponData.currentLevel == 0)
            {
                // Item is not owned
                levelText.text = "LOCKED";
                buyUpgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "BUY: " + cost.ToString("N0");

                // Disable equip button
                equipButton.gameObject.SetActive(false);
            }
            else
            {
                // Item is owned
                levelText.text = "Lvl " + weaponData.currentLevel;
                buyUpgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "UPGRADE: " + cost.ToString("N0");

                // Enable equip button
                equipButton.gameObject.SetActive(true);
                UpdateEquipButtonState();
            }
        }
    }

    // Toggles visual state of the Equip button based on active weapon
    private void UpdateEquipButtonState()
    {
        if (ShopManager.Instance.equippedWeapon == weaponData)
        {
            // Current weapon is equipped
            equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "EQUIPPED";
            equipButton.interactable = false;
            equipButton.image.color = Color.blueViolet;
        }
        else
        {
            // Current weapon is idle
            equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "EQUIP";
            equipButton.interactable = true;
            equipButton.image.color = Color.white;
        }
    }
}