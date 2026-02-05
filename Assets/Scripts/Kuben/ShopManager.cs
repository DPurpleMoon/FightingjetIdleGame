using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[DefaultExecutionOrder(-50)]
public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public GameObject manObj;

    [Header("Configuration")]
    public List<WeaponData> availableWeapons;
    public WeaponData equippedWeapon;

    [Header("Special Items")]
    public SpecialItemData ascensionItem;

    public event Action OnShopChanged;

    private void Awake()
    {
        if (Instance == null) { Instance = this;}
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (availableWeapons != null && availableWeapons.Count > 0) 
        {
            LoadShop();
            CheckForStarterWeapon();
        }
    }

    private void CheckForStarterWeapon()
    {
        if (equippedWeapon == null)
        {
            WeaponData starter = availableWeapons[0];

            if (starter != null)
            {
                if (starter.currentLevel == 0)
                {
                    starter.currentLevel = 1;
                }

                EquipWeapon(starter);
            }
        }
    }

    public void TryBuyWeapon(WeaponData weapon)
    {
        if (weapon == null) return;
        double cost = weapon.GetCost();

        if (CurrencyManager.Instance.SpendCurrency(cost))
        {
            weapon.LevelUp();
            // Auto-equip first purchase
            if (weapon.currentLevel == 1 && equippedWeapon == null) EquipWeapon(weapon);
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

    public void TryBuyAscension()
    {
        if (ascensionItem == null) return;
        double cost = ascensionItem.GetCost();

        if (CurrencyManager.Instance.Currency >= cost)
        {
            AscensionManager.Instance.OpenAscensionPrompt(cost);
        }
    }

    public void NotifyUI() => OnShopChanged?.Invoke();

    public void SaveShop()
    {
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        object[] WeaponUnlocked = new object[] {};
        object[] WeaponDetails = new object[] {};
        foreach (WeaponData weapon in availableWeapons)
            WeaponDetails = new object[] {weapon.name, weapon.currentLevel};
            WeaponUnlocked = WeaponUnlocked.Append(WeaponDetails).ToArray();
        SaveLoad.SaveGame("PurchasedWeapons", WeaponUnlocked);

        int index = (equippedWeapon != null) ? availableWeapons.IndexOf(equippedWeapon) : -1;
        SaveLoad.SaveGame("EquipWeapon", index);
    }

    public void LoadShop()
    {
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        object[] WeaponUnlocked = (object[])SaveLoad.LoadGame("PurchasedWeapons");
        foreach (WeaponData weapon in availableWeapons)
            foreach (object[] WeaponDetails in WeaponUnlocked)
            {
                if (WeaponDetails[0] == weapon.name)
                {
                    weapon.currentLevel = (int)WeaponDetails[1];
                }
            }

        int indexToLoad = (int)SaveLoad.LoadGame("EquipWeapon");
        if (indexToLoad == null)
        {
            indexToLoad = -1;
        }
        if (indexToLoad != -1 && indexToLoad < availableWeapons.Count)
            equippedWeapon = availableWeapons[indexToLoad];

        NotifyUI();
    }

    [ContextMenu("Reset Shop")]
    public void ResetAllWeapons()
    {
        foreach (WeaponData weapon in availableWeapons) weapon.currentLevel = 0;
        equippedWeapon = null;
        SaveShop();
        NotifyUI();
    }
}