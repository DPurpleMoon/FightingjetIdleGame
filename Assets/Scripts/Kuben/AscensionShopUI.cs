using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AscensionShopUI : MonoBehaviour
{
    [Header("Data")]
    public SpecialItemData itemData; // Drag AscensionItem here

    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI bonusText;
    public Button buyButton;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyClicked);

        // Update when Money changes (to gray out button?) or when Ascension happens (to update price)
        if (CurrencyManager.Instance != null) CurrencyManager.Instance.OnCurrencyChanged += (v) => UpdateDisplay();
        if (AscensionManager.Instance != null) AscensionManager.Instance.OnAscensionChanged += UpdateDisplay;

        UpdateDisplay();
    }

    private void OnBuyClicked()
    {
        ShopManager.Instance.TryBuyAscension();
    }

    private void UpdateDisplay()
    {
        if (itemData == null || AscensionManager.Instance == null) return;

        double currentCost = itemData.GetCost();

        // Calculate the NEXT bonus
        float currentMult = (float)AscensionManager.Instance.GetAscensionMultiplier();
        float nextMult = currentMult + AscensionManager.Instance.bonusPerToken;

        titleText.text = itemData.itemName;
        costText.text = $"${currentCost:N0}";
        bonusText.text = $"Reset for Bonus:\n{currentMult:F1}x -> <color=green>{nextMult:F1}x</color>";
    }
}