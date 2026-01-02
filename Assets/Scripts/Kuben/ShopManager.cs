using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the purchasing, upgrading, and equipping logic for weapons.
/// Handles data persistence for shop items.
/// </summary>
[DefaultExecutionOrder(-50)]
public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("Configuration")]
    public List<WeaponData> availableWeapons;
    public WeaponData equippedWeapon;

    public event Action OnShopChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Validate that the weapon list is populated
        if (availableWeapons == null || availableWeapons.Count == 0)
        {
            Debug.LogError("ShopManager Error: Available Weapons list is empty. Please assign WeaponData objects in the Inspector.");
            return;
        }

        LoadShop();
    }

    // --- SAVE & LOAD ---

    public void SaveShop()
    {
        // 1. Save individual weapon levels
        foreach (WeaponData weapon in availableWeapons)
        {
            PlayerPrefs.SetInt("Shop_" + weapon.name, weapon.currentLevel);
        }

        // 2. Save the index of the currently equipped weapon
        if (equippedWeapon != null)
        {
            int index = availableWeapons.IndexOf(equippedWeapon);
            PlayerPrefs.SetInt("Shop_Equipped_Index", index);
            Debug.Log($"Shop Saved. Equipped Index: {index} ({equippedWeapon.weaponName})");
        }
        else
        {
            PlayerPrefs.SetInt("Shop_Equipped_Index", -1);
            Debug.Log("Shop Saved. No weapon equipped.");
        }

        PlayerPrefs.Save();
    }

    public void LoadShop()
    {
        // 1. Load weapon levels
        foreach (WeaponData weapon in availableWeapons)
        {
            int savedLevel = PlayerPrefs.GetInt("Shop_" + weapon.name, 0);
            weapon.currentLevel = savedLevel;
        }

        // 2. Load equipped weapon by index
        int indexToLoad = PlayerPrefs.GetInt("Shop_Equipped_Index", -1);

        if (indexToLoad != -1)
        {
            if (indexToLoad < availableWeapons.Count)
            {
                equippedWeapon = availableWeapons[indexToLoad];
                Debug.Log($"Shop Loaded. Equipped: {equippedWeapon.weaponName}");
            }
            else
            {
                Debug.LogError($"Shop Load Error: Saved index [{indexToLoad}] is out of bounds. List count: {availableWeapons.Count}");
            }
        }
        else
        {
            Debug.Log("Shop Loaded. No weapon previously equipped.");
        }

        NotifyUI();
    }

    // --- TRANSACTION LOGIC ---

    public void TryBuyWeapon(WeaponData weapon)
    {
        if (weapon == null) return;
        double cost = weapon.GetCost();

        if (CurrencyManager.Instance.SpendCurrency(cost))
        {
            weapon.LevelUp();

            // Auto-equip logic for first-time purchase
            if (weapon.currentLevel == 1 && equippedWeapon == null)
            {
                EquipWeapon(weapon);
            }

            SaveShop();
            NotifyUI();
        }
    }

    public void EquipWeapon(WeaponData weapon)
    {
        equippedWeapon = weapon;
        SaveShop();
        NotifyUI();
    }

    public void NotifyUI() => OnShopChanged?.Invoke();

    private void OnApplicationQuit()
    {
        SaveShop();
    }

    // --- RESET ---
    [ContextMenu("Reset shop (Ascension)")]
    public void ResetAllWeapons()
    {
        // 1. Reset All Weapon Levels 
        foreach (WeaponData weapon in availableWeapons)
        {
            weapon.currentLevel = 0;
        }
        // 2. Remove equiped weapons 
        equippedWeapon = null;
        // 3. Save the changes 
        SaveShop() ;
        NotifyUI();
        Debug.Log("[Shop Sysytem] All weapon levels reset succesfully");
    }

    private void Update()
    {
        // --- Development Testing Controls ---
        // W: Reset Weapons Levels
        
        if (Keyboard.current == null) return;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            ResetAllWeapons();
            Debug.Log($"[Debug] All Weapon Level Have Been Reset Succesfully");
        }
    }
}
