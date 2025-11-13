using UnityEngine;

// ScriptableObject for storing equipment upgrade data.
[CreateAssetMenu(fileName = "EquipmentUpgradeData", menuName = "Equipment/New Equipment Upgrade")]
public class EquipmentUpgradeData : ScriptableObject
{
    public string upgradeName;
    public int upgradeLevel;
    public int price;
    public int damageIncrease;
    public int healIncrease;
}