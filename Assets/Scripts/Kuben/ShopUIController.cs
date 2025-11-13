using System.Collections.Generic;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    public Transform equipmentListPanel;         // Parent transform for equipment items
    public GameObject equipmentItemPrefab;       // Prefab for shop entries
    public List<EquipmentData> availableEquipments; // Assign catalog items in inspector

    void OnEnable()
    {
        // refresh when ascension happens or other events that change ownership
        AscensionManager.OnAscension += RefreshShop;
    }

    void OnDisable()
    {
        AscensionManager.OnAscension -= RefreshShop;
    }

    void Start()
    {
        PopulateShop();
    }

    // Populate shop using catalog
    public void PopulateShop()
    {
        if (equipmentListPanel == null || equipmentItemPrefab == null) return;

        // cleanup existing children
        for (int i = equipmentListPanel.childCount - 1; i >= 0; i--)
            Destroy(equipmentListPanel.GetChild(i).gameObject);

        if (availableEquipments == null) return;

        foreach (var equipment in availableEquipments)
        {
            var itemGO = Instantiate(equipmentItemPrefab, equipmentListPanel);
            var itemUI = itemGO.GetComponent<EquipmentItemUI>();
            if (itemUI != null)
            {
                itemUI.SetEquipment(equipment);

                // Optionally show owned state
                bool owned = EquipmentManager.Instance != null && EquipmentManager.Instance.allEquipments.Contains(equipment);
                // If the prefab has a method or field to show Owned/Locked, call it here.
                // e.g., itemUI.SetOwnedState(owned);
            }
        }
    }

    public void RefreshShop()
    {
        PopulateShop();
    }
}
