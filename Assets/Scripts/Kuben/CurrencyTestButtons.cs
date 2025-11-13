using UnityEngine;

// Simple test script for adding/spending currency via UI buttons.
public class CurrencyTestButtons : MonoBehaviour
{
    public void AddCurrency()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.AddCurrency(10); // Adds 10 currency
    }

    public void SpendCurrency()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.SpendCurrency(5); // Spends 5 currency
    }
}