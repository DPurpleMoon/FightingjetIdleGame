using UnityEngine;
using TMPro;

public class IdleUpgradeManager : MonoBehaviour
{
    public IdleData idleData;
    public TextMeshProUGUI idleRateText;

    void OnEnable()
    {
        AscensionManager.OnAscension += UpdateIdleRateText;
    }

    void OnDisable()
    {
        AscensionManager.OnAscension -= UpdateIdleRateText;
    }

    void Start()
    {
        UpdateIdleRateText();
    }

    public void UpdateIdleRateText()
    {
        int currentStage = IdleManager.Instance != null ? IdleManager.Instance.currentStage : 1;
        if (idleRateText != null && idleData != null)
        {
            float rate = idleData.GetCurrentIdleRate(currentStage);
            float ascMultiplier = AscensionManager.Instance != null ? AscensionManager.Instance.GetAscensionMultiplier() : 1f;
            idleRateText.text = $"Idle Rate: {rate * ascMultiplier:0.00} / sec";
        }
    }
}
