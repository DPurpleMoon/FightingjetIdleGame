using UnityEngine;
using System;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the player's currency, handling storage, saving/loading, and modification events.
/// Execution Order is set to -100 to ensure this initializes before UI scripts.
/// </summary>
[DefaultExecutionOrder(-100)]
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public double Currency { get; private set; } = 0;

    private const string CurrencyKey = "PlayerCurrency";

    // Event triggered when currency value changes
    public event Action<double> OnCurrencyChanged;

    private void Awake()
    {
        // Singleton Pattern Implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCurrency();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetCurrency()
    {
        Currency = 0;
        NotifyUI();
        SaveCurrency();
    }

    public void AddCurrency(double amount)
    {
        if (amount <= 0) return;

        Currency += amount;
        NotifyUI();
    }

    public bool SpendCurrency(double amount)
    {
        if (amount <= 0) return false;

        if (Currency >= amount)
        {
            Currency -= amount;
            NotifyUI();
            return true;
        }
        return false;
    }

    // Invokes the event to update subscribers (UI)
    private void NotifyUI()
    {
        OnCurrencyChanged?.Invoke(Currency);
    }

    private void OnApplicationQuit()
    {
        SaveCurrency();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) SaveCurrency();
    }

    public void SaveCurrency()
    {
        // Convert to string for PlayerPrefs safety regarding double precision
        PlayerPrefs.SetString(CurrencyKey, Currency.ToString());
        PlayerPrefs.Save();
        Debug.Log("Game Data Saved.");
    }

    public void LoadCurrency()
    {
        string s = PlayerPrefs.GetString(CurrencyKey, "0");
        if (double.TryParse(s, out double result))
        {
            Currency = result;
        }
        else
        {
            Currency = 0;
        }
    }

    private void Update()
    {
        // --- Development Testing Controls ---
        // A: Add Funds
        // D: Deduct Funds
        // R: Reset Funds

        if (Keyboard.current == null) return;

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            AddCurrency(1000);
            Debug.Log($"[Debug] Currency Added. Current: {Currency}");
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            SpendCurrency(50);
            Debug.Log($"[Debug] Currency Spent. Current: {Currency}");
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetCurrency();
            Debug.Log($"[Debug] Currency Reset. Current: {Currency}");
        }
    }
}