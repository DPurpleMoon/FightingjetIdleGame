using TMPro;
using UnityEngine;

// Continuously updates the idle rate display for the player.
// Idle rate is affected only by stage level.
public class IdleRateUIController : MonoBehaviour
{
    public TextMeshProUGUI idleRateText;    // UI text for idle rate
    public IdleData idleData;               // Reference to idle data

    void Update()
    {
        int currentStage = IdleManager.Instance != null ? IdleManager.Instance.currentStage : 1;
        if (idleRateText != null && idleData != null)
            idleRateText.text = $"Idle Rate: {idleData.GetCurrentIdleRate(currentStage):0.00} / sec";
    }
}