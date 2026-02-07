using UnityEngine;
using TMPro;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using System.Linq;
using System;

public class ResultScreen : MonoBehaviour
{
    public GameObject manager;
    public GameObject ResultPanel;
    public GameObject DeathPanel;
    public TMP_Text ScoreText;
    public StageScrollingData StageData;
    public GameObject manObj;
    public void ShowResultScreen()
    {
        Time.timeScale = 0;
        ResultPanel.SetActive(true);
        ScoreText.text = $"Score: {StageScore.Instance.CurrentScore}";
        if (DeathPanel.activeInHierarchy)
        {
            ResultPanel.SetActive(false);
        }
    }
    public void returnStageList()  
    {
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        var StageScoreParentList = new JsonObject();
        var StageScoreList = new JsonArray();
        
        string StageScoreSave = (string)SaveLoad.LoadGame("LevelScore");
        if (string.IsNullOrEmpty(StageScoreSave))
        {
            StageScoreSave = "{\"StageScore\": [{\"Name\": \"NotStage\", \"Score\": 0}]}";
        }
        JsonNode jsonNode = JsonNode.Parse(StageScoreSave);
        JsonArray StageScoreListJson = jsonNode?["StageScore"]?.AsArray();
        List<string> StageList = new List<string>{};
        foreach (var item in StageScoreListJson)
        {
            string StageName = (string)item?["Name"];
            StageList.Add(StageName);
        }
        if (StageList.Contains(StageData.level))
        {        
            foreach (var item in StageScoreListJson)
            {
                if ((string)item?["Name"] == StageData.level)
                {
                    if (StageScore.Instance.CurrentScore > (int)item?["Score"])
                    {
                        StageScoreList.Add(new JsonObject { ["Name"] = (string)item?["Name"], ["Score"] = StageScore.Instance.CurrentScore});
                    }
                    else
                    {
                        StageScoreList.Add(new JsonObject { ["Name"] = (string)item?["Name"], ["Score"] = (int)item?["Score"]});
                    }
                }
                else
                {
                    StageScoreList.Add(new JsonObject { ["Name"] = (string)item?["Name"], ["Score"] = (int)item?["Score"]});
                }
            }
        }
        else
        {
            foreach (var item in StageScoreListJson)
            {
                StageScoreList.Add(new JsonObject { ["Name"] = (string)item?["Name"], ["Score"] = (int)item?["Score"]});
            }
            StageScoreList.Add(new JsonObject { ["Name"] = StageData.level, ["Score"] = StageScore.Instance.CurrentScore}); 
        }   

        StageScoreParentList["StageScore"] = StageScoreList;
        string StageScoreParentListString = JsonSerializer.Serialize(StageScoreParentList);
        SaveLoad.SaveGame("LevelScore", StageScoreParentListString);
        int CurrentStage = (int)SaveLoad.LoadGame("CurrentStage");
        double CurrentCurrency = (double)SaveLoad.LoadGame("Currency");
        double NewCurrency = (Math.Pow(3, CurrentStage - 1) * StageScore.Instance.CurrentScore) + CurrentCurrency;
        SaveLoad.SaveGame("Currency", NewCurrency);
        ResultPanel.SetActive(false);
        StageScrollingController Stage = manager.GetComponent<StageScrollingController>();
        StageData.stageReadEnd = false;
        StageData.enemySpawnEnd = false;
        Stage.LeaveStage();
    }
    public void returnStageListDeath()
    {
        DeathPanel.SetActive(false);
        StageScrollingController Stage = manager.GetComponent<StageScrollingController>();
        StageData.stageReadEnd = false;
        StageData.enemySpawnEnd = false;
        Stage.LeaveStage();
    }
}
