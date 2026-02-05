using UnityEngine;
using System;

[DefaultExecutionOrder(-100)]
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public double Currency { get; private set; } = 0;
    public GameObject manObj;

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
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        SaveLoad.SaveGame("Currency", Currency);
    }

    public void LoadCurrency()
    {
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        double s = (double)SaveLoad.LoadGame("Currency");
        if (s == null)
        {
            s = 0;
        }
        Currency = s;
    }
}