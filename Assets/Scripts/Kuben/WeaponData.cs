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
    public GameObject bulletPrefab; // --- MUST BE ASSIGNED IN EDITOR ---
    public double baseDamage = 10;
    public double damageMultiplier = 2;
    [Tooltip("Time between shots in seconds.")]
    public float fireRate = 0.5f;
    public float fireForce = 20f;

    public double GetCost()
    {
        return baseCost * System.Math.Pow(costMultiplier, currentLevel);
    }

    public double GetDamage()
    {
        if (currentLevel == 0) return 0;
        double baseDmg = baseDamage + ((currentLevel - 1) * damageMultiplier);

        if (AscensionManager.Instance != null)
        {
            baseDmg *= AscensionManager.Instance.GetAscensionMultiplier();
        }
        return baseDmg;
    }

    public void LevelUp()
    {
        currentLevel++;
    }
}