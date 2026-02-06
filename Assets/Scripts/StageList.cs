using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.IO;

public static class JsonHelper {
    public static string[] FromJsonArray(string json) {
        string newJson = "{ \"Items\": " + json + "}";
        Wrapper wrapper = JsonUtility.FromJson<Wrapper>(newJson);
        return wrapper.Items;
    }

    [Serializable]
    private class Wrapper {
        public string[] Items;
    }
}
public class StageList : MonoBehaviour
{
    public GameObject StageButton;
    public Transform contentParent;
    public StageScrollingData Stage; 
    public TMP_Text StageText;
    public StageListData Data;
    public int CurrentStageNum;
    public GameObject manObj;
    void Start()
    {
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        Time.timeScale = 1f;
        string streamingpath = Application.streamingAssetsPath;
        string[] filePaths = Directory.GetFiles($"{streamingpath}/levellist/", "*.json", SearchOption.AllDirectories);
        

        CurrentStageNum = (int)SaveLoad.LoadGame("CurrentStage");;
        if (CurrentStageNum == null)
        {
            CurrentStageNum = 0;
        }
        Data.stagenum = CurrentStageNum;
        StageText.text = $"Stage {Data.stagenum}";

        foreach (string filepath in filePaths)
        {
        string fileName = Path.GetFileName(filepath);
        
        int index = fileName.IndexOf(".json");
        if (index >= 0)
            {
                fileName = fileName.Substring(0, index);
            }
        Data.maxstagenum = int.Parse(fileName.Substring(5));
        if (fileName == $"level{Data.stagenum}")
            {
            string JsonString = File.ReadAllText(filepath);
            string[] levels = JsonHelper.FromJsonArray(JsonString);
                foreach (string level in levels)
                {
                    GameObject newButton = Instantiate(StageButton, contentParent);
                    newButton.name = level;
                    
                    // Set the text (Assumes prefab has a TMP_Text component)
                    newButton.GetComponentInChildren<TMP_Text>().text = level.Substring(5);

                    // Add a click listener
                    newButton.GetComponent<Button>().onClick.AddListener(() => {
                        Stage.level = level;
                        SceneManager.LoadScene("Stage0");
                    });
                }
            }
        }
    }

    public void Update()
    {
        if (Data.stagenum != CurrentStageNum)
        {
            manObj = GameObject.Find("SaveLoadManager");
            SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
            string streamingpath = Application.streamingAssetsPath;
            string[] filePaths = Directory.GetFiles($"{streamingpath}/levellist/", "*.json", SearchOption.AllDirectories);
            StageText.text = $"Stage {Data.stagenum}";
            CurrentStageNum = Data.stagenum;
            SaveLoad.SaveGame("CurrentStage", CurrentStageNum);
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }
            foreach (string filepath in filePaths)
            {
            string fileName = Path.GetFileName(filepath);
            
            int index = fileName.IndexOf(".json");
            if (index >= 0)
                {
                fileName = fileName.Substring(0, index);
                }
            Data.maxstagenum = int.Parse(fileName.Substring(5));
            if (fileName == $"level{Data.stagenum}")
                {
                string JsonString = File.ReadAllText(filepath);
                string[] levels = JsonHelper.FromJsonArray(JsonString);
                    foreach (string level in levels)
                    {
                        GameObject newButton = Instantiate(StageButton, contentParent);
                        newButton.name = level;
                        
                        // Set the text (Assumes prefab has a TMP_Text component)
                        newButton.GetComponentInChildren<TMP_Text>().text = level.Substring(5);

                        // Add a click listener
                        newButton.GetComponent<Button>().onClick.AddListener(() => {
                            Stage.level = level;
                            SceneManager.LoadScene("Stage0");
                        });
                    }
                }
            }
        }
    }

    public void ShopOpen()
    {
        SceneManager.LoadScene("Shop");
    }
        
    public void StartMenu()
    {
        SceneManager.LoadScene("startmenu");
    }
}
