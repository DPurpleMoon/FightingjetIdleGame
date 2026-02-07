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

    public GameObject manObj;

    void Start()
    {
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        float savedBrightness = (float)SaveLoad.LoadGame("Brightness");
        if (savedBrightness == null)
        {
            savedBrightness = 1f;
        }
        if (brightnessSlider != null)
        {
            brightnessSlider.value = savedBrightness;
        }
        SetBrightness(savedBrightness);
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }

        float savedMusicVolume = (float)SaveLoad.LoadGame("MusicVolume");
        if (savedMusicVolume == null)
        {
            savedMusicVolume = 0.5f;
        }
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = savedMusicVolume;
        }
        SetMusicVolume(savedMusicVolume);
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        float savedSFXVolume = (float)SaveLoad.LoadGame("SFXVolume");
        if (savedSFXVolume == null)
        {
            savedSFXVolume = 0.7f;
        }
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
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        SaveLoad.SaveGame("Brightness", brightness);
        
        if (BrightnessManager.Instance != null)
        {
            BrightnessManager.Instance.ApplyBrightness(brightness);
        }
        
        Debug.Log("Brightness set to: " + brightness);
    }

    public void SetMusicVolume(float volume)
    {
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        SaveLoad.SaveGame("MusicVolume", volume);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(volume);
        }
        
        Debug.Log("Music volume set to: " + volume);
    }

    public void SetSFXVolume(float volume)
    {
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        SaveLoad.SaveGame("SFXVolume", volume);
        
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
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        float Brightness = (float)SaveLoad.LoadGame("Brightness");
        if (Brightness == null)
        {
            Brightness = 1f;
        }
        if (brightnessSlider != null)
        {
            brightnessSlider.value = Brightness;
        }
        float music = (float)SaveLoad.LoadGame("MusicVolume");
        if (music == null)
        {
            music = 0.5f;
        }
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = music;
        }
        
        float sfx = (float)SaveLoad.LoadGame("SFXVolume");
        if (sfx == null)
        {
            sfx = 0.7f;
        }
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = sfx;    
        }
        ShowStatus("Game Loaded!");
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

        StartMenu startMenu = FindFirstObjectByType<StartMenu>(FindObjectsInactive.Include);
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