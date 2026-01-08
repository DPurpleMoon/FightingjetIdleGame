using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Shop/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Identity")]
    public string weaponName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Economy Settings")]
    public double baseCost = 100;
    public float costMultiplier = 1.2f;
    [Tooltip("Level 0 indicates the weapon is not owned.")]
    public int currentLevel = 0;

    [Header("Combat Stats")]
    public double baseDamage = 10;
    public double damageMultiplier = 2;
    [Tooltip("Time between shots in seconds.")]
    public float fireRate = 0.5f;

    // Calculates the cost for the next purchase or upgrade
    // Formula: BaseCost * (Multiplier ^ Level)
    public double GetCost()
    {
        return baseCost * System.Math.Pow(costMultiplier, currentLevel);
    }

    // Calculates current damage output based on level
    // Formula: BaseDamage + ((Level - 1) * Multiplier)
    public double GetDamage()
    {
        if (currentLevel == 0) return 0;
        return baseDamage + ((currentLevel - 1) * damageMultiplier);
    }

    // Increments the level of the weapon
    public void LevelUp()
    {
        currentLevel++;
    }
}