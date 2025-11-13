using System;
using UnityEngine;

[DefaultExecutionOrder(-70)]
public class AscensionManager : MonoBehaviour
{
    public static AscensionManager Instance;

    public AscensionData ascensionData;    // ScriptableObject with requirements & reward
    public bool ascensionAvailable = false;
    public int permanentStatBoost = 0;     // number of ascensions performed (or cumulative boost units)

    // Event fired after a successful ascension so UI/other systems refresh
    public delegate void OnAscensionHandler();
    public static event OnAscensionHandler OnAscension;

    private const string PrefKey_PermanentBoost = "PermanentStatBoost";

    void Awake()
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

    void Start()
    {
        LoadAscension();
        // ensure ascensionAvailable is correct at start if someone checks immediately
        if (ascensionData != null)
            CheckAscensionAvailable(IdleManager.Instance != null ? IdleManager.Instance.currentStage : 1);
    }

    // Check if player can ascend based on current stage
    public void CheckAscensionAvailable(int currentStage)
    {
        if (ascensionData == null) ascensionAvailable = false;
        else ascensionAvailable = (currentStage >= ascensionData.requiredStage);
    }

    // Perform ascension: apply boost, reset progress, save state
    public void Ascend()
    {
        if (!ascensionAvailable) return;

        permanentStatBoost += ascensionData.statBoostAmount;
        SaveAscension();

        ResetProgress();
        ascensionAvailable = false;

        // notify listeners (UI, shop, idle, currency display, etc.)
        OnAscension?.Invoke();
    }

    // Reset player progress except permanent stat boost
    public void ResetProgress()
    {
        if (CurrencyManager.Instance != null) CurrencyManager.Instance.ResetCurrency();

        if (EquipmentManager.Instance != null)
        {
            EquipmentManager.Instance.ClearOwnership(); // clears saved owned equipments
            EquipmentManager.Instance.allEquipments.Clear();
            EquipmentManager.Instance.equippedEquipment = null;
        }

        // Optionally clear other saved data like stage progress keys if you have them
    }

    // Save permanent stat boost to PlayerPrefs
    public void SaveAscension()
    {
        PlayerPrefs.SetInt(PrefKey_PermanentBoost, permanentStatBoost);
        PlayerPrefs.Save();
    }

    // Load permanent stat boost from PlayerPrefs
    public void LoadAscension()
    {
        permanentStatBoost = PlayerPrefs.GetInt(PrefKey_PermanentBoost, 0);
    }

    // Global multiplier: returns 1.0 + 0.1 * permanentStatBoost (10% per boost)
    public float GetAscensionMultiplier()
    {
        return 1f + 0.10f * permanentStatBoost;
    }
}
