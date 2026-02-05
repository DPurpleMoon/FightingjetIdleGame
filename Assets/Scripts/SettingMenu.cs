using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SettingsMenu : MonoBehaviour
{
    [Header("Settings UI")]
    public Slider brightnessSlider;
    public Slider musicVolumeSlider; 
    public Slider sfxVolumeSlider;   
    public GameObject settingsPanel;
    
    [Header("Save/Load UI (Optional)")]
    public Button saveButton;
    public Button loadButton;
    public TextMeshProUGUI statusText;

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

        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = savedMusicVolume;
        }
        SetMusicVolume(savedMusicVolume);
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = savedSFXVolume;
        }
        SetSFXVolume(savedSFXVolume);
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }
        
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveGame);
        }
        
        if (loadButton != null)
        {
            loadButton.onClick.AddListener(LoadGame);
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

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(volume);
        }
        
        Debug.Log("Music volume set to: " + volume);
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(volume);
        }
        
        Debug.Log("SFX volume set to: " + volume);
    }
    
    public void SaveGame()
    {
        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.SaveGame();
            ShowStatus("Game Saved!");
 
        }
        else
        {
            Debug.LogError("SaveLoadManager not found!");
            ShowStatus("Save Failed!");
        }
    }
    
    public void LoadGame()
    {
        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.LoadGame();
            
            float brightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
            if (brightnessSlider != null)
                brightnessSlider.value = brightness;
                
            float music = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            if (musicVolumeSlider != null)
                musicVolumeSlider.value = music;
                
            float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.value = sfx;
            
            ShowStatus("Game Loaded!");
        }
        else
        {
            Debug.LogError("SaveLoadManager not found!");
            ShowStatus("Load Failed!");
        }
    }
    
    void ShowStatus(string message)
    {
        Debug.Log(message);
        
        if (statusText != null)
        {
            statusText.text = message;
            StartCoroutine(ClearStatusAfterDelay(2f));
        }
    }
    
    IEnumerator ClearStatusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (statusText != null)
        {
            statusText.text = "";
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        StartMenu startMenu = FindFirstObjectByType<StartMenu>();
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