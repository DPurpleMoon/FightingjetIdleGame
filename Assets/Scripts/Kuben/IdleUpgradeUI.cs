using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IdleUpgradeUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI rateText;
    public TextMeshProUGUI costText;
    public Button upgradeButton;

    private void Start()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
        }

        // Listen for changes
        if (IdleManager.Instance != null)
        {
            IdleManager.Instance.OnIdleStatsChanged += UpdateDisplay;
            UpdateDisplay();

        }
    }

    private void OnDestroy()
    {
        if (IdleManager.Instance != null)
        {
            IdleManager.Instance.OnIdleStatsChanged -= UpdateDisplay;

        }
    }

    private void OnUpgradeClicked()
    {
        IdleManager.Instance.BuyIdleUpgrade();
    }

    private void UpdateDisplay()
    {
        if (IdleManager.Instance == null) return;   

        double currentRate = IdleManager.Instance.GetCoinsPerSecond();
        double cost = IdleManager.Instance.GetUpgradeCost();
        int level = IdleManager.Instance.idleUpgradeLevel;

        levelText.text = $"Idle Level: {level}";
        rateText.text = $"Current Rate: ${currentRate:F1}/sec";
        costText.text = $"UPGRADE: ${cost:N0}";
    }
}