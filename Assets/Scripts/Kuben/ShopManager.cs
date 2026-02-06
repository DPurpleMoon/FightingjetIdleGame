using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;


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
        manObj = GameObject.Find("SaveLoadManager");
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
        var weaponList = new JsonObject();
        var weaponBuyDetail = new JsonArray();
        foreach (WeaponData weapon in availableWeapons)
            weaponBuyDetail.Add(new JsonObject { ["WeaponName"] = weapon.name, ["WeaponLevel"] = weapon.currentLevel});
        weaponList["weaponList"] = weaponBuyDetail;
        string weaponListString = JsonSerializer.Serialize(weaponList);
        SaveLoad.SaveGame("PurchasedWeapons", weaponListString);

        int index = (equippedWeapon != null) ? availableWeapons.IndexOf(equippedWeapon) : -1;
        SaveLoad.SaveGame("EquipWeapon", index);
    }

    public void LoadShop()
    {
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        string WeaponUnlockedString = (string)SaveLoad.LoadGame("PurchasedWeapons");
        if (string.IsNullOrEmpty(WeaponUnlockedString))
        {
            WeaponUnlockedString = "{\"weaponList\": [{\"WeaponName\": \"Basic Blaster\", \"WeaponLevel\": 1}]}";
        }
        JsonNode jsonNode = JsonNode.Parse(WeaponUnlockedString);
        JsonArray WeaponDataList = jsonNode?["weaponList"]?.AsArray();
        if (WeaponDataList != null)
        foreach (WeaponData weapon in availableWeapons)
        {
            foreach (var WeaponDetails in WeaponDataList)
            {
                if ((string)WeaponDetails?["WeaponName"] == (string)weapon.name)
                {
                    weapon.currentLevel = (int)WeaponDetails?["WeaponLevel"] ;
                }
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