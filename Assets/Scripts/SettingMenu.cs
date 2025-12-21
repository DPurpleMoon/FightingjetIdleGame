using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Settings UI")]
    public Slider volumeSlider;
    public TMP_Dropdown qualityDropdown;
    public GameObject settingsPanel;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        int savedQuality = PlayerPrefs.GetInt("Quality", 1);
        
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }
        
        if (qualityDropdown != null)
        {
            qualityDropdown.value = savedQuality;
        }
        
        SetVolume(savedVolume);
        SetQuality(savedQuality);
        
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        
        if (qualityDropdown != null)
        {
            qualityDropdown.onValueChanged.AddListener(SetQuality);
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
        Debug.Log("Volume set to: " + volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
        PlayerPrefs.Save();
        Debug.Log("Quality set to: " + qualityIndex);
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