using UnityEngine;
using System;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the Prestige/Rebirth system.
/// Calculates tokens based on progress and applies permanent multipliers.
/// </summary>
[DefaultExecutionOrder(-40)] // Runs after other managers
public class AscensionManager : MonoBehaviour
{
    public static AscensionManager Instance;

    [Header("Ascension Data")]
    public int ascensionTokens = 0;   // Permanent Currency
    public float bonusPerToken = 0.10f; // Each token = +10% stats

    [Header("Settings")]
    public int unlockStage = 10;      // Minimum stage to ascend
    public int stagesPerToken = 5;    // e.g., Stage 50 = 10 Tokens

    // Events
    public event Action OnAscensionChanged;

    private const string TokenKey = "Ascension_Tokens";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAscension();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- MATH HELPERS ---

    // 1. Calculate how many tokens the player WOULD get if they ascend now
    public int GetPendingTokens()
    {
        int currentStage = IdleManager.Instance.currentStage;

        // Safety: If below stage 10, you get nothing
        if (currentStage < unlockStage) return 0;

        // Formula: Stage / 5
        return currentStage / stagesPerToken;
    }

    // 2. Get the global multiplier (e.g., 5 tokens * 0.10 = +50% -> 1.5x multiplier)
    public double GetAscensionMultiplier()
    {
        // Base is 1.0 (100%) + (Tokens * 10%)
        return 0.0 + (ascensionTokens * bonusPerToken);
    }

    // --- THE BIG RED BUTTON ---

    public void Ascend()
    {
        int tokensToGain = GetPendingTokens();

        if (tokensToGain <= 0)
        {
            Debug.Log("Not enough progress to ascend!");
            return;
        }

        // A. Add Tokens to permanent bank
        ascensionTokens += tokensToGain;
        SaveAscension();

        // B. RESET EVERYTHING ELSE
        CurrencyManager.Instance.ResetCurrency();
        ShopManager.Instance.ResetAllWeapons();
        IdleManager.Instance.ResetIdleProgress();

        // Important: Reset Stage back to 1
        IdleManager.Instance.currentStage = 1;
        IdleManager.Instance.SaveIdleData(); // Force save the stage reset

        // C. Notify UI
        OnAscensionChanged?.Invoke();

        Debug.Log($"<color=yellow>ASCENSION COMPLETE! Gained {tokensToGain} Tokens.</color>");
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
    [ContextMenu("Add 10 Tokens")]
    public void CheatTokens() { ascensionTokens += 10; SaveAscension(); OnAscensionChanged?.Invoke(); }

    public void ResetAscensionTokens()
    {
        ascensionTokens = 0;
        SaveAscension();
    }

    public void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ResetAscensionTokens();
            Debug.Log($"[Debug] Reset Ascension Tokens Succesfully");
        }
    }
}