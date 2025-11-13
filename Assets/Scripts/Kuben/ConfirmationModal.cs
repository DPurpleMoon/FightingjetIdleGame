using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ConfirmationModal : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;
    public Button confirmButton;
    public Button cancelButton;
    public CanvasGroup canvasGroup; // optional for show/hide

    private UnityAction onConfirmAction;

    void Awake()
    {
        // Ensure buttons clear listeners
        if (confirmButton != null) confirmButton.onClick.AddListener(OnConfirmClicked);
        if (cancelButton != null) cancelButton.onClick.AddListener(OnCancelClicked);
        Hide();
    }

    public void Show(string title, string message, UnityAction onConfirm)
    {
        if (titleText != null) titleText.text = title;
        if (messageText != null) messageText.text = message;

        onConfirmAction = onConfirm;
        if (canvasGroup != null) canvasGroup.alpha = 1f;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        onConfirmAction = null;
        if (canvasGroup != null) canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    void OnConfirmClicked()
    {
        onConfirmAction?.Invoke();
        Hide();
    }

    void OnCancelClicked()
    {
        Hide();
    }
}
