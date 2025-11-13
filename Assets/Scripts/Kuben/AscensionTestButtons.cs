using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Simple test panel to simulate stage progression and ascension checks.
public class AscensionTestButtons : MonoBehaviour
{
    public TextMeshProUGUI stageText;               // UI text displaying current stage
    public Button increaseStageButton;              // Button to increase stage
    public Button checkAscensionButton;             // Button to check ascension availability
    public AscensionUIController ascensionUIController; // Reference to ascension UI panel

    private int currentStage = 1;                   // Current simulated stage

    void Start()
    {
        UpdateStageText();
        increaseStageButton.onClick.AddListener(IncreaseStage);
        checkAscensionButton.onClick.AddListener(CheckAscension);
    }

    // Increase the current stage (simulates player progress)
    void IncreaseStage()
    {
        currentStage++;
        IdleManager.Instance.currentStage = currentStage; // Synchronize stage!
        UpdateStageText();
    }

    // Check if ascension is available and update UI
    public void CheckAscension()
    {
        AscensionManager.Instance.CheckAscensionAvailable(currentStage);
        if (ascensionUIController != null)
            ascensionUIController.UpdateAscensionUI();
    }

    // Update stage number in UI
    void UpdateStageText()
    {
        stageText.text = "Current Stage: " + currentStage;
    }
}