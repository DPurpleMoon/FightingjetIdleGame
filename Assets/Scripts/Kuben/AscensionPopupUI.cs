using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AscensionPopupUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI infoText;
    public Button confirmButton;
    public Button declineButton;

    private void Start()
    {
        confirmButton.onClick.AddListener(OnConfirm);
        declineButton.onClick.AddListener(OnDecline);
    }

    private void OnEnable()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (AscensionManager.Instance == null) return;

        // Just showing static warning text here, simpler for this version
        infoText.text = "<b><color=red>WARNING!</color></b>\n\n" +
                        "Buying this will <b>RESET</b> all Weapons, Money, and Stage Progress.\n\n" +
                        "You will restart at Stage 1, but with a permanent Income boost.\n\n" +
                        "Are you sure?";
    }

    private void OnConfirm()
    {
        AscensionManager.Instance.ConfirmAscend();
    }

    private void OnDecline()
    {
        AscensionManager.Instance.ClosePopup();
    }
}