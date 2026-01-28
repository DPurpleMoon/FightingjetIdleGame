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
    public float costMultiplier = 1.3f; 
    public int currentLevel = 0;

    [Header("Combat Stats")]
    public double baseDamage = 10;
    public double damageMultiplier = 2.5; 
    [Tooltip("Time between shots (Lower is faster)")]
    public float fireRate = 0.5f; 
    [Tooltip("Bullet Travel Speed")]
    public float fireForce = 20f; 

    // Cost = Base * (1.3 ^ Level)
    public double GetCost()
    {
        return baseCost * System.Math.Pow(costMultiplier, currentLevel);
    }

    // Damage = Base + ((Level-1) * 2.5)
    public double GetDamage()
    {
        if (currentLevel == 0) return 0;
        return baseDamage + ((currentLevel - 1) * damageMultiplier);
    }

    public void LevelUp()
    {
        currentLevel++;
    }
}