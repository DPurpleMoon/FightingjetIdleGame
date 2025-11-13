using UnityEngine;
using TMPro;

public class CurrencyUIController : MonoBehaviour
{
    public TextMeshProUGUI currencyText;

    void OnEnable()
    {
        AscensionManager.OnAscension += RefreshUI;
    }

    void OnDisable()
    {
        AscensionManager.OnAscension -= RefreshUI;
    }

    void Update()
    {
        if (CurrencyManager.Instance != null && currencyText != null)
            currencyText.text = "Currency: " + CurrencyManager.Instance.Currency;
    }

    void RefreshUI()
    {
        if (currencyText != null && CurrencyManager.Instance != null)
            currencyText.text = "Currency: " + CurrencyManager.Instance.Currency;
    }
}
