using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AscensionUIController : MonoBehaviour
{
    public TextMeshProUGUI ascensionDescriptionText;
    public Button ascendButton;
    public ConfirmationModal confirmationModal; // assign prefab instance in inspector

    void Start()
    {
        if (AscensionManager.Instance != null)
            AscensionManager.Instance.LoadAscension();

        ascendButton.onClick.RemoveAllListeners();
        ascendButton.onClick.AddListener(OnAscendClicked);
        AscensionManager.OnAscension += UpdateAscensionUI;
        UpdateAscensionUI();
    }

    void OnDestroy()
    {
        AscensionManager.OnAscension -= UpdateAscensionUI;
    }

    public void UpdateAscensionUI()
    {
        if (AscensionManager.Instance == null || AscensionManager.Instance.ascensionData == null) return;

        ascensionDescriptionText.text = AscensionManager.Instance.ascensionData.description +
            $"\nPermanent Boosts: {AscensionManager.Instance.permanentStatBoost}" +
            $"\nIncome Multiplier: x{AscensionManager.Instance.GetAscensionMultiplier():0.00}";

        ascendButton.interactable = AscensionManager.Instance.ascensionAvailable;
    }

    void OnAscendClicked()
    {
        if (AscensionManager.Instance == null || !AscensionManager.Instance.ascensionAvailable) return;

        if (confirmationModal != null)
        {
            string title = "Confirm Ascension";
            string message = "Ascending will reset your progress (currency & owned equipment) but grant a permanent income boost. Continue?";
            confirmationModal.Show(title, message, () =>
            {
                // Confirm callback
                AscensionManager.Instance.Ascend();
                UpdateAscensionUI();
            });
        }
        else
        {
            // No modal assigned — perform ascend directly (not recommended)
            AscensionManager.Instance.Ascend();
            UpdateAscensionUI();
        }
    }
}
