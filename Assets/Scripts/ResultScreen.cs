using UnityEngine;
using TMPro;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using System.Linq;

public class ResultScreen : MonoBehaviour
{
    public GameObject manager;
    public GameObject ResultPanel;
    public GameObject DeathPanel;
    public TMP_Text ScoreText;
    public StageScrollingData StageData;
    public void ShowResultScreen()
    {
        ResultPanel.SetActive(true);
        ScoreText.text = $"Score: {StageScore.Instance.CurrentScore}";
    }
    public void returnStageList()
    {
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        var StageScoreParentList = new JsonObject();
        var StageScoreList = new JsonArray();
        
        string StageScoreSave = (string)SaveLoad.LoadGame("StageScore");
        if (string.IsNullOrEmpty(StageScoreSave))
        {
            WeaponUnlockedString = "{\"StageScore\": [{\"Name\": \"NotStage\", \"Score\": 0}]}";
        }
        JsonNode jsonNode = JsonNode.Parse(WeaponUnlockedString);
        JsonArray StageScoreListJson = jsonNode?["StageScore"]?.AsArray();
        List<string> StageList = new List<string>{};
        foreach (var item in StageScoreListJson)
        {
            string StageName = (string)item?["Name"];
            StageList.Add(StageName);
        }
        if StageList.Contains(StageData.level);
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

        StageScoreParentList["StageScore"] = StageScoreList;
        string StageScoreParentListString = JsonSerializer.Serialize(StageScoreParentList);
        SaveLoad.SaveGame("LevelScore", StageScoreParentListString);
        ResultPanel.SetActive(false);
        StageScrollingController Stage = manager.GetComponent<StageScrollingController>();
        Stage.LeaveStage();
    }
    public void returnStageListDeath()
    {
        DeathPanel.SetActive(false);
        StageScrollingController Stage = manager.GetComponent<StageScrollingController>();
        Stage.LeaveStage();
    }
}
