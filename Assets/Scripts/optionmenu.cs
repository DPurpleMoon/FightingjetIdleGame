using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class optionmenu : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject pausePanel;
    public GameObject pauseSettingsPanel;

    [Header("Settings UI")]
    public Slider brightnessSlider;
    public Slider musicVolumeSlider;  
    public Slider sfxVolumeSlider;
    public StageScrollingData Data; 
    public GameObject ScrollGameObject;
    public StageScrollingController Stage;

    public GameObject manObj;

    void Start()
    {
        Debug.Log("=== OPTIONMENU START BEGIN ===");
        
        try
        {
            Debug.Log("1. Checking Data...");
            if (Data == null)
            {
                Debug.LogError("Data is NULL!");
                enabled = false;
                return;
            }
            
            Debug.Log("2. Setting isPaused to false...");
            Data.isPaused = false;
            
            Debug.Log("3. Deactivating panels...");
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            
            if (pauseSettingsPanel != null)
            {
                pauseSettingsPanel.SetActive(false);
            }

            Debug.Log("4. Calling ResumeGameplay...");
            ResumeGameplay();
            
            Debug.Log("5. Loading settings...");
            LoadSettings();

            Debug.Log("6. Adding slider listeners...");
            if (brightnessSlider != null)
            {
                brightnessSlider.onValueChanged.AddListener(SetBrightness);
                Debug.Log("   - Brightness listener added");
            }

            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
                Debug.Log("   - Music listener added");
            }

            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
                Debug.Log("   - SFX listener added");
            }
            
            Debug.Log("=== OPTIONMENU START COMPLETE ===");
        }
        catch (System.Exception e)
        {
            Debug.LogError("EXCEPTION IN START: " + e.Message);
            Debug.LogError("Stack trace: " + e.StackTrace);
        }
    }

    void Update()
    {
        if (Data == null) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC pressed. isPaused: " + Data.isPaused);
            
            if (Data.isPaused)
            {
                if (pauseSettingsPanel != null && pauseSettingsPanel.activeSelf)
                {
                    ClosePauseSettings();
                }
                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    void LoadSettings()
    {
        Debug.Log("LoadSettings: START");
        
        try
        {
            SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
            float savedBrightness = (float)SaveLoad.LoadGame("Brightness");
            if (savedBrightness == null)
            {
                savedBrightness = 0.7f;
                SaveLoad.SaveGame("Brightness", 0.7f);
            }
            Debug.Log("LoadSettings: Brightness = " + savedBrightness);
            
            if (brightnessSlider != null)
            {
                brightnessSlider.SetValueWithoutNotify(savedBrightness);
            }
            
            Debug.Log("LoadSettings: Calling SetBrightness...");
            SetBrightness(savedBrightness);
            Debug.Log("LoadSettings: SetBrightness complete");
            float savedMusicVolume = (float)SaveLoad.LoadGame("MusicVolume");
            if (savedMusicVolume == null)
            {
                savedMusicVolume = 0.5f;
                SaveLoad.SaveGame("MusicVolume", 0.5f);
            }
            Debug.Log("LoadSettings: Music volume = " + savedMusicVolume);
            
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.SetValueWithoutNotify(savedMusicVolume);
            }
            
            Debug.Log("LoadSettings: Calling SetMusicVolume...");
            SetMusicVolume(savedMusicVolume);
            Debug.Log("LoadSettings: SetMusicVolume complete");

            float savedSFXVolume = (float)SaveLoad.LoadGame("SFXVolume");
            if (savedSFXVolume == null)
            {
                savedSFXVolume = 0.7f;
                SaveLoad.SaveGame("SFXVolume", 0.7f);
            }
            Debug.Log("LoadSettings: SFX volume = " + savedSFXVolume);
            
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.SetValueWithoutNotify(savedSFXVolume);
            }
            
            Debug.Log("LoadSettings: Calling SetSFXVolume...");
            SetSFXVolume(savedSFXVolume);
            Debug.Log("LoadSettings: SetSFXVolume complete");
            
            Debug.Log("LoadSettings: COMPLETE");
        }
        catch (System.Exception e)
        {
            Debug.LogError("EXCEPTION IN LoadSettings: " + e.Message);
            Debug.LogError("Stack trace: " + e.StackTrace);
        }
    }

    public void PauseGame()
{
    Debug.Log("PauseGame called");
    
    if (pausePanel != null)
    {
        pausePanel.SetActive(true);
    }
    if (pauseSettingsPanel != null)
    {
        pauseSettingsPanel.SetActive(false);
    }
    
    if (Data != null)
    {
        Data.isPaused = true;
    }
    
    PauseGameplay();
}

public void ResumeGame()
{
    Debug.Log("ResumeGame called");
    
    if (pausePanel != null)
    {
        pausePanel.SetActive(false);
    }
    
    if (pauseSettingsPanel != null)
    {
        pauseSettingsPanel.SetActive(false);
    }
    
    ResumeGameplay();
    
    if (Data != null)
    {
        Data.isPaused = false;
    }
}
    void PauseGameplay()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Paused - Time.timeScale = 0");
    }

    void ResumeGameplay()
    {
        Time.timeScale = 1f;
        Debug.Log("Game Resumed - Time.timeScale = 1");
    }

    public void OpenPauseSettings()
    {
        Debug.Log("OpenPauseSettings called");
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        
        if (pauseSettingsPanel != null)
        {
            pauseSettingsPanel.SetActive(true);
        }
    }

    public void ClosePauseSettings()
    {
        Debug.Log("ClosePauseSettings called");
        
        if (pauseSettingsPanel != null)
        {
            pauseSettingsPanel.SetActive(false);
        }
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void SetBrightness(float brightness)
    {
        Debug.Log("SetBrightness called with value: " + brightness);
        
        try
        {
            SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
            SaveLoad.SaveGame("Brightness", brightness);
            
            Debug.Log("SetBrightness: Checking BrightnessManager...");
            
            if (BrightnessManager.Instance != null)
            {
                Debug.Log("SetBrightness: BrightnessManager exists, applying...");
                BrightnessManager.Instance.ApplyBrightness(brightness);
                Debug.Log("SetBrightness: Applied successfully");
            }
            else
            {
                Debug.LogWarning("SetBrightness: BrightnessManager.Instance is null");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("EXCEPTION in SetBrightness: " + e.Message);
            Debug.LogError("Stack trace: " + e.StackTrace);
        }
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log("SetMusicVolume called with value: " + volume);
        
        try
        {
            SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
            SaveLoad.SaveGame("MusicVolume", volume);
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(volume);
            }
            else
            {
                Debug.LogWarning("AudioManager.Instance is null");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("EXCEPTION in SetMusicVolume: " + e.Message);
        }
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log("SetSFXVolume called with value: " + volume);
        
        try
        {
            SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
            SaveLoad.SaveGame("SFXVolume", volume);
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetSFXVolume(volume);
            }
            else
            {
                Debug.LogWarning("AudioManager.Instance is null");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("EXCEPTION in SetSFXVolume: " + e.Message);
        }
    }

    public void LoadMainMenu()
    {
        Debug.Log("LoadMainMenu called");
        
        Time.timeScale = 1f;  
        ResumeGameplay(); 
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayStartMenuMusic();
        }
        
        SceneManager.LoadScene("startmenu");
    }

    public void QuitGame()
    {
        StageScrollingController Stage = ScrollGameObject.GetComponent<StageScrollingController>();
        Stage.LeaveStage();
    }
}