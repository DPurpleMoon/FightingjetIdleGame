using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentItemUI : MonoBehaviour
{
    public TextMeshProUGUI equipmentNameText;
    public Image equipmentIconImage;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI upgradeLevelText;
    public Button buyButton;
    public Button upgradeButton;
    public Button equipButton;

    private EquipmentData equipment;

    public void SetEquipment(EquipmentData equip)
    {
        equipment = equip;
        if (equip == null) return;

        equipmentNameText.text = equip.equipmentName;
        equipmentIconImage.sprite = equip.equipmentIcon;
        priceText.text = "Price: " + equip.price;
        upgradeLevelText.text = "Level: " + equip.upgradeLevel;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => BuyEquipment());

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => UpgradeEquipment());

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() => EquipEquipment());

        // Optionally disable buy if already owned
        if (EquipmentManager.Instance != null && EquipmentManager.Instance.allEquipments.Contains(equipment))
            buyButton.interactable = false;
    }

    void BuyEquipment()
    {
        EquipmentManager.Instance.BuyEquipment(equipment);
        // Refresh UI after buying (disable buy button)
        if (buyButton != null)
            buyButton.interactable = false;
    }

    void UpgradeEquipment()
    {
        if (EquipmentManager.Instance.UpgradeEquipment(equipment))
            upgradeLevelText.text = "Level: " + equipment.upgradeLevel;
    }

    void EquipEquipment()
    {
        EquipmentManager.Instance.EquipEquipment(equipment);
    }
}
