using UnityEngine;

// ScriptableObject for storing individual equipment item data.
[CreateAssetMenu(fileName = "EquipmentData", menuName = "Equipment/New Equipment")]
public class EquipmentData : ScriptableObject
{
    public string equipmentName;
    public Sprite equipmentIcon;
    public int baseDamage;
    public int baseHeal;
    public int price;
    public int upgradeLevel;
    public int upgradePrice;
}