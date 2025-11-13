using UnityEngine;

[DefaultExecutionOrder(-100)]
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int Currency { get; private set; } = 0;
    private const string CurrencyKey = "PlayerCurrency";

    void Awake()
    {
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

    // Reset currency to zero and save
    public void ResetCurrency()
    {
        Currency = 0;
        SaveCurrency();
    }

    // Add currency; gains are multiplied by ascension multiplier (if present)
    // If you don't want multiplier to apply for a particular add, pass applyAscensionMultiplier = false
    public void AddCurrency(int amount, bool applyAscensionMultiplier = true)
    {
        if (amount <= 0) return;

        float multiplier = 1f;
        if (applyAscensionMultiplier && AscensionManager.Instance != null)
            multiplier = AscensionManager.Instance.GetAscensionMultiplier();

        int finalAmount = Mathf.RoundToInt(amount * multiplier);
        Currency += finalAmount;
        SaveCurrency();
    }

    // Try to spend currency; returns true if successful (spending is not multiplied)
    public bool SpendCurrency(int amount)
    {
        if (amount <= 0) return false;
        if (Currency >= amount)
        {
            Currency -= amount;
            SaveCurrency();
            return true;
        }
        return false;
    }

    // Save current currency to PlayerPrefs
    public void SaveCurrency()
    {
        PlayerPrefs.SetInt(CurrencyKey, Currency);
        PlayerPrefs.Save();
    }

    // Load currency from PlayerPrefs
    public void LoadCurrency()
    {
        Currency = PlayerPrefs.GetInt(CurrencyKey, 0);
    }
}
