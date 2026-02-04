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
    
    [Header("Save/Load UI (Optional)")]
    [Tooltip("Button to save game - optional")]
    public Button saveButton;
    [Tooltip("Button to load game - optional")]
    public Button loadButton;
    [Tooltip("Text to show save/load status - optional")]
    public TextMeshProUGUI statusText;
    
    public StageScrollingData Data; 

    void Start()
    {
        Debug.Log("=== OPTIONMENU START BEGIN ===");
        
        try
        {
             if (Data == null)
            {
                Debug.LogError("Data is NULL!");
                enabled = false;
                return;
            }
            
             Data.isPaused = false;
            
             if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            
            if (pauseSettingsPanel != null)
            {
                pauseSettingsPanel.SetActive(false);
            }

             ResumeGameplay();
            
             LoadSettings();

             if (brightnessSlider != null)
            {
                brightnessSlider.onValueChanged.AddListener(SetBrightness);
            }

            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            }

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
        try
        {
             float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
            if (brightnessSlider != null)
            {
                brightnessSlider.SetValueWithoutNotify(savedBrightness);
            }
            SetBrightness(savedBrightness);

             float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.SetValueWithoutNotify(savedMusicVolume);
            }
            SetMusicVolume(savedMusicVolume);

             float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.SetValueWithoutNotify(savedSFXVolume);
            }
            SetSFXVolume(savedSFXVolume);
            
            Debug.Log("Settings loaded successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError("EXCEPTION IN LoadSettings: " + e.Message);
        }
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
            LoadSettings(); 
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

    public void PauseGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        
        if (Data != null)
        {
            Data.isPaused = true;
        }
        
        PauseGameplay();
    }

    public void ResumeGame()
    {
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
    }

    void ResumeGameplay()
    {
        Time.timeScale = 1f;
    }

    public void OpenPauseSettings()
    {
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
        try
        {
            PlayerPrefs.SetFloat("Brightness", brightness);
            PlayerPrefs.Save();
            
            if (BrightnessManager.Instance != null)
            {
                BrightnessManager.Instance.ApplyBrightness(brightness);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("EXCEPTION in SetBrightness: " + e.Message);
        }
    }

    public void SetMusicVolume(float volume)
    {
        try
        {
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save();
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(volume);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("EXCEPTION in SetMusicVolume: " + e.Message);
        }
    }

    public void SetSFXVolume(float volume)
    {
        try
        {
            PlayerPrefs.SetFloat("SFXVolume", volume);
            PlayerPrefs.Save();
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetSFXVolume(volume);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("EXCEPTION in SetSFXVolume: " + e.Message);
        }
    }

    public void LoadMainMenu()
    {
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
        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.SaveGame();
        }
        
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}