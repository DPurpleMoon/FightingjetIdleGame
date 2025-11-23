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

    public void ResetCurrency()
    {
        Currency = 0;
        SaveCurrency(); 
    }

    public void AddCurrency(int amount)
    {
        if (amount <= 0) return; 

        Currency += amount; 
        SaveCurrency(); 
    }

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

    public void SaveCurrency()
    {
        PlayerPrefs.SetInt(CurrencyKey, Currency);
        PlayerPrefs.Save(); 
    }

    public void LoadCurrency()
    {
        Currency = PlayerPrefs.GetInt(CurrencyKey, 0);
    }
}
