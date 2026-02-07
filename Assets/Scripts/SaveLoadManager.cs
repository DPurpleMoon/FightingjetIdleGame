using UnityEngine;
using System;
using System.IO;

 
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    
    private string saveFilePath;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void SaveGame(string DataType, object objectdata)
    {
        try
        {
            GameData Savedata = new GameData();
            saveFilePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
            if (File.Exists(saveFilePath))
            {
                string jsonString = File.ReadAllText(saveFilePath);
                Savedata = JsonUtility.FromJson<GameData>(jsonString);
            }
            if (DataType == "Brightness")
            {
                Savedata.brightness = (float)objectdata;
            }
            else if (DataType == "MusicVolume")
            {
                Savedata.musicVolume = (float)objectdata;
            }
            else if (DataType == "SFXVolume")
            {
                Savedata.sfxVolume = (float)objectdata;
            }
            else if (DataType == "Currency")
            {
                Savedata.currency = (double)objectdata;
            }
            else if (DataType == "EquipWeapon")
            {
                Savedata.equippedWeaponName = (int)objectdata;
            }
            else if (DataType == "PurchasedWeapons")
            {
                Savedata.purchasedWeapon = (string)objectdata;
            }
            else if (DataType == "LevelScore")
            {
                Savedata.levelScore = (string)objectdata;
            }
            else if (DataType == "Ascension")
            {
                Savedata.AscensionLevel = (int)objectdata;
            }
            else if (DataType == "LogTime")
            {
                Savedata.logoutTime = (string)objectdata;
            }
            else if (DataType == "IdleLevel")
            {
                Savedata.idleLevel = (int)objectdata;
            }
            else if (DataType == "CurrentStage")
            {
                Savedata.CurrentStage = (int)objectdata;
            }
            else
            {
                Debug.LogError("Wrong DataType argument given!");
            }

            string json = JsonUtility.ToJson(Savedata, true);  
            
            File.WriteAllText(saveFilePath, json);
            
            Debug.Log("Game saved successfully to: " + saveFilePath);
            Debug.Log("Saved data: " + json);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save game: " + e.Message);
        }
    }
    
    
    public object LoadGame(string DataType)
    {
        try
        {
            saveFilePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
            if (!File.Exists(saveFilePath))
            {
                Debug.Log("No save file found. Starting new game.");
                return null;
            }
            string json = File.ReadAllText(saveFilePath);
            GameData Savedata = JsonUtility.FromJson<GameData>(json);
            
            if (Savedata == null)
            {
                Debug.LogError("Failed to parse save data!");
                return null;
            }
            
            if (DataType == "Brightness")
            {
                return Savedata.brightness;
            }
            else if (DataType == "MusicVolume")
            {
                return Savedata.musicVolume;
            }
            else if (DataType == "SFXVolume")
            {
                return Savedata.sfxVolume;
            }
            else if (DataType == "Currency")
            {
                return Savedata.currency;
            }
            else if (DataType == "EquipWeapon")
            {
                return Savedata.equippedWeaponName;
            }
            else if (DataType == "PurchasedWeapons")
            {
                return Savedata.purchasedWeapon;
            }
            else if (DataType == "LevelScore")
            {
                return Savedata.levelScore;
            }
            else if (DataType == "Ascension")
            {
                return Savedata.AscensionLevel;
            }
            else if (DataType == "LogTime")
            {
                return Savedata.logoutTime;
            }
            else if (DataType == "IdleLevel")
            {
                return Savedata.idleLevel;
            }
            else if (DataType == "CurrentStage")
            {
                return Savedata.CurrentStage;
            }
            else
            {
                Debug.LogError("Wrong DataType argument given!");
            }
            Debug.Log("Game loaded successfully!");
            Debug.Log("Loaded data: " + json);
            return null;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load game: " + e.Message);
            return null;
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

    public void NewSave()
    {
        GameData Savedata = new GameData();
        saveFilePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
        string json = JsonUtility.ToJson(Savedata, true);   
        File.WriteAllText(saveFilePath, json);
    }
     
    public bool SaveExists()
    {
        return File.Exists(saveFilePath);
    }
}

 
[Serializable]
public class GameData
{
    public float brightness; // yes
    public float musicVolume; // yes
    public float sfxVolume; // yes
    public double currency; // yes
    public int equippedWeaponName; // yes
    public int AscensionLevel; // yes
    public string logoutTime; // yes
    public int idleLevel; // yes
    public string purchasedWeapon; // json // yes
    public string levelScore; // json
    public int CurrentStage;
}