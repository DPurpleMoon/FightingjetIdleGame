using UnityEngine;
using System;
using System.IO;

 
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    
     private string saveFilePath;
    
    void Awake()
    {
         if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
             saveFilePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
            Debug.Log("Save file location: " + saveFilePath);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    public void SaveGame()
    {
        try
        {
             GameData data = new GameData();
            
             data.brightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
            data.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            data.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
            
            
            

            
             if (StageScore.Instance != null)
            {
                data.highScore = StageScore.Instance.CurrentScore;
            }
            
             string json = JsonUtility.ToJson(data, true);  
            
             File.WriteAllText(saveFilePath, json);
            
            Debug.Log("Game saved successfully to: " + saveFilePath);
            Debug.Log("Saved data: " + json);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save game: " + e.Message);
        }
    }
    
    
    public void LoadGame()
    {
        try
        {
             if (!File.Exists(saveFilePath))
            {
                Debug.Log("No save file found. Starting new game.");
                return;
            }
            
             string json = File.ReadAllText(saveFilePath);
            
             GameData data = JsonUtility.FromJson<GameData>(json);
            
            if (data == null)
            {
                Debug.LogError("Failed to parse save data!");
                return;
            }
            
             PlayerPrefs.SetFloat("Brightness", data.brightness);
            PlayerPrefs.SetFloat("MusicVolume", data.musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", data.sfxVolume);
            PlayerPrefs.Save();
            
             if (BrightnessManager.Instance != null)
            {
                BrightnessManager.Instance.ApplyBrightness(data.brightness);
            }
            
             if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(data.musicVolume);
                AudioManager.Instance.SetSFXVolume(data.sfxVolume);
            }
            
           
            
            Debug.Log("Game loaded successfully!");
            Debug.Log("Loaded data: " + json);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load game: " + e.Message);
        }
    }
    
     
    public void DeleteSave()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                Debug.Log("Save file deleted.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete save: " + e.Message);
        }
    }
    
     
    public bool SaveExists()
    {
        return File.Exists(saveFilePath);
    }
}

 
[Serializable]
public class GameData
{
     public float brightness = 1.0f;
    public float musicVolume = 0.5f;
    public float sfxVolume = 0.7f;
    
     public int maxHealth = 3;
    public int currentHealth = 3;
    
     public int currency = 0;
    public string equippedWeaponName = "";
    public string[] purchasedWeapons = new string[0];
    
     public int highScore = 0;
    public string[] completedStages = new string[0];
}