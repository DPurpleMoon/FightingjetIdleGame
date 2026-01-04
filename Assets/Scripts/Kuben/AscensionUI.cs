using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AscensionUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI currentTokenText; // "Tokens: 5 (Multiplier: 1.5x)"
    public TextMeshProUGUI pendingTokenText; // "Ascend now for: +2 Tokens"
    public Button ascendButton;

    private void Start()
    {
        ascendButton.onClick.AddListener(OnAscendClicked);

        // Listen to events
        AscensionManager.Instance.OnAscensionChanged += UpdateDisplay;
        // Also update whenever Idle Stats change (because Stage progress changes pending tokens)
        IdleManager.Instance.OnIdleStatsChanged += UpdateDisplay;

        UpdateDisplay();
    }

    private void Update()
    {
        UpdateDisplay(); 
    }

    private void OnAscendClicked()
    {
        AscensionManager.Instance.Ascend();
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (AscensionManager.Instance == null) return;

        // 1. Show Current Stat
        int tokens = AscensionManager.Instance.ascensionTokens;
        double mult = AscensionManager.Instance.GetAscensionMultiplier();
        currentTokenText.text = $"Tokens: {tokens}\nBonus: {mult:F2}x";

        // 2. Show Pending Reward
        int pending = AscensionManager.Instance.GetPendingTokens();

        if (pending > 0)
        {
            pendingTokenText.text = $"Ascend to Gain:\n+{pending} Tokens";
            ascendButton.interactable = true;
        }
        else
        {
            pendingTokenText.text = $"Reach Stage {AscensionManager.Instance.unlockStage} to Ascend";
            ascendButton.interactable = false;
        }
    }
}