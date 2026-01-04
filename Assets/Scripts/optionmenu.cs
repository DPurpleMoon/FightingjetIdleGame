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

        LoadSettings();

        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
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
            
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.Sleep();
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
            
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.Sleep();
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
                
                Rigidbody2D rb = bg.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.Sleep();
                }
            }
        }

        List<GameObject> bullets = new List<GameObject>{};
        bullets.AddRange(GameObject.FindGameObjectsWithTag("PlayerBullet"));
        bullets.AddRange(GameObject.FindGameObjectsWithTag("EnemyBullet"));
        foreach (GameObject bullet in bullets)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.Sleep();
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
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.WakeUp();
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
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.WakeUp();
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
                Rigidbody2D rb = bg.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.WakeUp();
                }
            }
        }

        List<GameObject> bullets = new List<GameObject>{};
        bullets.AddRange(GameObject.FindGameObjectsWithTag("PlayerBullet"));
        bullets.AddRange(GameObject.FindGameObjectsWithTag("EnemyBullet"));
        foreach (GameObject bullet in bullets)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.WakeUp();
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

    public void LoadMainMenu()
    {
        ResumeGameplay(); 
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