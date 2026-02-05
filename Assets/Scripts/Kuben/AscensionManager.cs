using UnityEngine;
using System;

[DefaultExecutionOrder(-40)]
public class AscensionManager : MonoBehaviour
{
    public static AscensionManager Instance;

    [Header("Ascension Data")]
    public int ascensionTokens = 0;   
    public float bonusPerToken = 0.15f; // +10% Income per Token

    [Header("UI Reference")]
    public GameObject ascensionPopup;

    private double pendingCost = 0;
    public event Action OnAscensionChanged;

    public GameObject manObj;

    private void Awake()
    {
        if (Instance == null) { Instance = this; LoadAscension(); }
        else Destroy(gameObject);
        manObj = GameObject.Find("SaveLoadManager");
    }

    // Multiplier = 1.0 + (Tokens * 0.10)
    public double GetAscensionMultiplier()
    {
        return 1.0 + (ascensionTokens * bonusPerToken);
    }

    public void OpenAscensionPrompt(double cost)
    {
        pendingCost = cost;
        if (ascensionPopup != null) ascensionPopup.SetActive(true);
    }

    public void ConfirmAscend()
    {
        if (CurrencyManager.Instance.SpendCurrency(pendingCost))
        {
            ascensionTokens++;
            SaveAscension();

            // Reset Game State
            if (ShopManager.Instance != null) ShopManager.Instance.ResetAllWeapons();
            if (IdleManager.Instance != null)
            {
                CurrencyManager.Instance.ResetCurrency();
                IdleManager.Instance.ResetIdleProgress();
                IdleManager.Instance.currentStage = 1; 
                IdleManager.Instance.SaveIdleData();
                IdleManager.Instance.UpdateStageMultiplier();
            }

            OnAscensionChanged?.Invoke();
            ClosePopup();
        }
        else
        {
            ClosePopup();
        }
    }

    public void ClosePopup()
    {
        if (ascensionPopup != null) ascensionPopup.SetActive(false);
    }

    public void SaveAscension()
    {
        "AscensionTokens"
        PlayerPrefs.SetInt(TokenKey, ascensionTokens);
        PlayerPrefs.Save();
    }

    public void LoadAscension()
    {
        ascensionTokens = PlayerPrefs.GetInt(TokenKey, 0);
    }

    [ContextMenu("Reset Ascension")]
    public void ResetAscensionTokens() 
    { 
        ascensionTokens = 0; 
        SaveAscension(); 
        OnAscensionChanged?.Invoke(); 
    }
}