using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        // Initialize the display with current currency value
        if (CurrencyManager.Instance != null)
        {
            UpdateCoinText(CurrencyManager.Instance.Currency);
        }
    }

    private void OnEnable()
    {
        // Subscribe to the event to listen for updates
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged += UpdateCoinText;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or errors when object is disabled
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateCoinText;
        }
    }

    // Updates the text UI element. Formats the number with commas (N0).
    private void UpdateCoinText(double amount)
    {
        coinText.text = "$" + amount.ToString("N0");
    }
}