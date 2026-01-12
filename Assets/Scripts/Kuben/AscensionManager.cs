using UnityEngine;
using System;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-40)]
public class AscensionManager : MonoBehaviour
{
    public static AscensionManager Instance;

    [Header("Ascension Data")]
    public int ascensionTokens = 0;   // This is now your "Ascension Count"
    public float bonusPerToken = 0.10f; // +10% per ascension

    [Header("UI Reference")]
    public GameObject ascensionPopup; // Drag the Confirmation Panel here

    // Internal state
    private double pendingCost = 0;

    // Events
    public event Action OnAscensionChanged;
    private const string TokenKey = "Ascension_Tokens";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadAscension();
        }
        else Destroy(gameObject);
    }

    // --- MATH ---

    public double GetAscensionMultiplier()
    {
        // FIX: Start at 1.0 (100%), add bonus on top
        // 0 Ascensions = 1.0x
        // 1 Ascension = 1.1x
        return 1.0 + (ascensionTokens * bonusPerToken);
    }

    // --- INTERACTION ---

    // 1. Called by ShopManager when buying the item
    public void OpenAscensionPrompt(double cost)
    {
        pendingCost = cost;

        if (ascensionPopup != null)
        {
            ascensionPopup.SetActive(true);
        }
        else
        {
            Debug.LogError("Ascension Popup not assigned in Inspector!");
        }
    }

    // 2. Called by the "YES" button on the popup
    public void ConfirmAscend()
    {
        // A. Pay the price
        if (CurrencyManager.Instance.SpendCurrency(pendingCost))
        {
            // B. Increase Rank
            ascensionTokens++;
            SaveAscension();

            // C. RESET EVERYTHING
            if (ShopManager.Instance != null) ShopManager.Instance.ResetAllWeapons();
            if (IdleManager.Instance != null)
            {
                IdleManager.Instance.ResetIdleProgress();
                IdleManager.Instance.currentStage = 1; // Force Stage 1
                IdleManager.Instance.SaveIdleData();
                IdleManager.Instance.UpdateStageMultiplier(); // Reload Stage 1 stats
            }

            // D. Refresh UI
            OnAscensionChanged?.Invoke();

            ClosePopup();
            Debug.Log($"<color=yellow>ASCENSION SUCCESS! New Multiplier: {GetAscensionMultiplier()}x</color>");
        }
        else
        {
            Debug.Log("Error: Currency spent failed (this shouldn't happen if checked before).");
            ClosePopup();
        }
    }

    public void ClosePopup()
    {
        if (ascensionPopup != null) ascensionPopup.SetActive(false);
    }

    // --- SAVE & LOAD ---

    public void SaveAscension()
    {
        PlayerPrefs.SetInt(TokenKey, ascensionTokens);
        PlayerPrefs.Save();
    }

    public void LoadAscension()
    {
        ascensionTokens = PlayerPrefs.GetInt(TokenKey, 0);
    }

    // Debug
    [ContextMenu("Reset Ascension")]
    public void ResetAscensionTokens() { ascensionTokens = 0; SaveAscension(); OnAscensionChanged?.Invoke(); }
}