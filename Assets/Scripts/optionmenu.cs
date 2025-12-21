using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class optionmenu : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject pausePanel;
    public GameObject pauseSettingsPanel;

    [Header("Settings UI")]
    public Slider volumeSlider;
    public TMP_Dropdown qualityDropdown;

    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        
        if (pauseSettingsPanel != null)
        {
            pauseSettingsPanel.SetActive(false);
        }

        LoadSettings();

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (qualityDropdown != null)
        {
            qualityDropdown.onValueChanged.AddListener(SetQuality);
        }

        Debug.Log("PauseMenu Ready - NO Time.timeScale");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC Pressed");
            
            if (isPaused)
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
    }

    public void ResumeGame()
    {
        Debug.Log("Resume");
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        
        if (pauseSettingsPanel != null)
        {
            pauseSettingsPanel.SetActive(false);
        }
        
        isPaused = false;
    }

    public void PauseGame()
    {
        Debug.Log("Pause");
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        
        isPaused = true;
    }

    public void OpenPauseSettings()
    {
        Debug.Log("Open Settings");
        
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
        Debug.Log("Close Settings");
        
        if (pauseSettingsPanel != null)
        {
            pauseSettingsPanel.SetActive(false);
        }
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
        PlayerPrefs.Save();
    }

    public void LoadMainMenu()
    {
        Debug.Log("Main Menu");
        SceneManager.LoadScene("startmenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}