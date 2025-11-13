using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[DefaultExecutionOrder(-90)]
public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    // Catalog: assign all EquipmentData assets the game knows about in inspector.
    public List<EquipmentData> equipmentCatalog = new List<EquipmentData>();

    // Player-owned equipment (transient list, saved/loaded)
    public List<EquipmentData> allEquipments = new List<EquipmentData>();
    public EquipmentData equippedEquipment;

    private const string PrefKey_OwnedEquipments = "OwnedEquipments";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadOwnedEquipments();
    }

    // Attempt to buy equipment
    public bool BuyEquipment(EquipmentData equipment)
    {
        if (equipment == null || CurrencyManager.Instance == null) return false;

        if (CurrencyManager.Instance.SpendCurrency(equipment.price))
        {
            if (!allEquipments.Contains(equipment))
            {
                allEquipments.Add(equipment);
                SaveOwnedEquipments();
            }
            return true;
        }
        return false;
    }

    public bool UpgradeEquipment(EquipmentData equipment)
    {
        if (equipment == null || CurrencyManager.Instance == null) return false;
        if (CurrencyManager.Instance.SpendCurrency(equipment.upgradePrice))
        {
            equipment.upgradeLevel++;
            SaveOwnedEquipments(); // save in case you want upgrade levels persisted in the future
            return true;
        }
        return false;
    }

    public void EquipEquipment(EquipmentData equipment)
    {
        if (equipment == null) return;
        if (allEquipments.Contains(equipment))
        {
            equippedEquipment = equipment;
            SaveOwnedEquipments(); // optional; if you want to persist equipped choice
        }
    }

    // --- Persistence by name (simple JSON list) ---
    [System.Serializable]
    class EquipmentSave
    {
        public List<string> ownedNames = new List<string>();
        public string equippedName = "";
    }

    public void SaveOwnedEquipments()
    {
        var save = new EquipmentSave();
        save.ownedNames = allEquipments.Select(e => e.equipmentName).ToList();
        save.equippedName = equippedEquipment != null ? equippedEquipment.equipmentName : "";

        string json = JsonUtility.ToJson(save);
        PlayerPrefs.SetString(PrefKey_OwnedEquipments, json);
        PlayerPrefs.Save();
    }

    public void LoadOwnedEquipments()
    {
        allEquipments.Clear();
        equippedEquipment = null;

        string json = PlayerPrefs.GetString(PrefKey_OwnedEquipments, "");
        if (string.IsNullOrEmpty(json)) return;

        var save = JsonUtility.FromJson<EquipmentSave>(json);
        if (save == null) return;

        // Resolve names via equipmentCatalog assigned in inspector
        foreach (var name in save.ownedNames)
        {
            var found = equipmentCatalog.FirstOrDefault(e => e != null && e.equipmentName == name);
            if (found != null && !allEquipments.Contains(found))
                allEquipments.Add(found);
        }

        if (!string.IsNullOrEmpty(save.equippedName))
        {
            var eq = equipmentCatalog.FirstOrDefault(e => e != null && e.equipmentName == save.equippedName);
            if (eq != null && allEquipments.Contains(eq))
                equippedEquipment = eq;
        }
    }

    // Clear ownership (used by ascension reset)
    public void ClearOwnership()
    {
        allEquipments.Clear();
        equippedEquipment = null;
        PlayerPrefs.DeleteKey(PrefKey_OwnedEquipments);
    }
}
