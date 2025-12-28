using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Settings UI")]
    public Slider brightnessSlider;
    public GameObject settingsPanel;

    void Start()
    {
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
        
        if (brightnessSlider != null)
        {
            brightnessSlider.value = savedBrightness;
        }
        
        SetBrightness(savedBrightness);
        
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
    }

    public void SetBrightness(float brightness)
    {
        PlayerPrefs.SetFloat("Brightness", brightness);
        PlayerPrefs.Save();
        
        if (BrightnessManager.Instance != null)
        {
            BrightnessManager.Instance.ApplyBrightness(brightness);
        }
        
        Debug.Log("Brightness set to: " + brightness);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        
        StartMenu startMenu = FindObjectOfType<StartMenu>();
        if (startMenu != null)
        {
            startMenu.CloseSettings();
        }
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }
}