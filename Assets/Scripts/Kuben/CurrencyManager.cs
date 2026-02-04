using UnityEngine;
using System;

[DefaultExecutionOrder(-100)]
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public double Currency { get; private set; } = 0;

    private const string CurrencyKey = "PlayerCurrency";
    public event Action<double> OnCurrencyChanged;

    private void Awake()
    {
        if (Instance == null) { Instance = this; LoadCurrency(); }
        else Destroy(gameObject);
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
        if (amount <= 0 || Currency < amount) return false;
        Currency -= amount;
        NotifyUI();
        return true;
    }

    private void NotifyUI() => OnCurrencyChanged?.Invoke(Currency);

    private void OnApplicationQuit() => SaveCurrency();
    private void OnApplicationPause(bool pause) { if (pause) SaveCurrency(); }

    public void SaveCurrency()
    {
        PlayerPrefs.SetString(CurrencyKey, Currency.ToString());
        PlayerPrefs.Save();
    }

    public void LoadCurrency()
    {
        string s = PlayerPrefs.GetString(CurrencyKey, "0");
        Currency = double.TryParse(s, out double result) ? result : 0;
    }
}