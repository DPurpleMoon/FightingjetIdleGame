using UnityEngine;
using UnityEngine.UI;

// Example script to set up test buttons for currency manipulation.
public class ButtonClickSetup : MonoBehaviour
{
    public Button addCurrencyButton;    // Button to add currency
    public Button spendCurrencyButton;  // Button to spend currency

    void Start()
    {
        addCurrencyButton.onClick.AddListener(() =>
        {
            CurrencyManager.Instance.AddCurrency(10);
        });

        spendCurrencyButton.onClick.AddListener(() =>
        {
            CurrencyManager.Instance.SpendCurrency(5);
        });
    }
}