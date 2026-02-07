using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class StartMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject startMenuPanel;
    public GameObject settingsPanel;
    public GameObject creditPanel;

    public GameObject manObj;
    public Button ContinueButton;

    void Start()
    {
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(true);
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        
        if (creditPanel != null)
        {
            creditPanel.SetActive(false);
        }

        if (AudioManager.Instance != null)
        {
        AudioManager.Instance.PlayStartMenuMusic();
        }
        if (!SaveLoad.SaveExists())
        {
            ContinueButton.gameObject.SetActive(false);
        }
        else
        {
            ContinueButton.gameObject.SetActive(true);
        }
    }

    public void OpenCredits()
    {
        startMenuPanel.SetActive(false);
        creditPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        startMenuPanel.SetActive(true);
        creditPanel.SetActive(false);
    }

    public void NewGame()
    {
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        SaveLoad.NewSave();
        SaveLoad.SaveGame("CurrentStage", 1);
        SaveLoad.SaveGame("Brightness", 1f);
        SaveLoad.SaveGame("MusicVolume", 0.7f);
        SaveLoad.SaveGame("SFXVolume", 0.5f);
        string logTime = DateTime.Now.ToBinary().ToString();
        SaveLoad.SaveGame("LogTime", logTime);
        SceneManager.LoadScene("StageList");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameplayMusic();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("StageList");

         if (AudioManager.Instance != null)
    {
        AudioManager.Instance.PlayGameplayMusic();
    }
    }

    public void OpenSettings()
    {
        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(false);
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            StartMenu startMenu = FindFirstObjectByType<StartMenu>();
            if (startMenu != null)
            {
                startMenu.OpenSettings();
            }
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        
        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}