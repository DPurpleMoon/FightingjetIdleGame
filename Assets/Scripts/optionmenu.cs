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

    void Start()
    {
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
        
        Debug.Log("optionmenu Start - isPaused: " + Data.isPaused);
    }

    void Update()
    {
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
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
        if (brightnessSlider != null)
        {
            brightnessSlider.value = savedBrightness;
        }
        SetBrightness(savedBrightness);

        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = savedMusicVolume;
        }
        SetMusicVolume(savedMusicVolume);

        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = savedSFXVolume;
        }
        SetSFXVolume(savedSFXVolume);
    }

    public void PauseGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        
        PauseGameplay();
        
        Data.isPaused = true;
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
        
        Data.isPaused = false;
    }

    void PauseGameplay()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            MonoBehaviour[] scripts = enemy.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
        }

        GameObject[] background = GameObject.FindGameObjectsWithTag("Background");
        foreach (GameObject bg in background)
        {
            if (bg != null)
            {
                MonoBehaviour[] scripts = bg.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in scripts)
                {
                    script.enabled = false;
                }
            }
        }

        List<BulletSelfDestruct> bullets = new List<BulletSelfDestruct>{};
        bullets.AddRange(FindObjectsOfType<BulletSelfDestruct>());
        foreach (BulletSelfDestruct bullet in bullets)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                bullet.PauseBullet();
            }
        }

        GameObject spawner = GameObject.Find("EnemySpawnManager");
        if (spawner != null)
        {
            MonoBehaviour[] scripts = spawner.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
        }

        GameObject stageManager = GameObject.Find("InStageManager");
        if (stageManager != null)
        {
            MonoBehaviour[] scripts = stageManager.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
        }
    }

    void ResumeGameplay()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;
            }
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            MonoBehaviour[] scripts = enemy.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;
            }
        }

        GameObject[] background = GameObject.FindGameObjectsWithTag("Background");
        foreach (GameObject bg in background)
        {
            if (bg != null)
            {
                MonoBehaviour[] scripts = bg.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in scripts)
                {
                    script.enabled = true;
                }
            }
        }

        List<BulletSelfDestruct> bullets = new List<BulletSelfDestruct>{};
        bullets.AddRange(FindObjectsOfType<BulletSelfDestruct>());
        foreach (BulletSelfDestruct bullet in bullets)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                bullet.ResumeBullet();
            }
        }

        GameObject spawner = GameObject.Find("EnemySpawnManager");
        if (spawner != null)
        {
            MonoBehaviour[] scripts = spawner.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;
            }
        }

        GameObject stageManager = GameObject.Find("InStageManager");
        if (stageManager != null)
        {
            MonoBehaviour[] scripts = stageManager.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;
            }
        }
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
        PlayerPrefs.SetFloat("Brightness", brightness);
        PlayerPrefs.Save();
        
        if (BrightnessManager.Instance != null)
        {
            BrightnessManager.Instance.ApplyBrightness(brightness);
        }
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

    public void LoadMainMenu()
    {
        ResumeGameplay(); 
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayStartMenuMusic();
        }
        
        SceneManager.LoadScene("startmenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}