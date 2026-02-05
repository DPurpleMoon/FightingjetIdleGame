using UnityEngine;
using TMPro;

public class ResultScreen : MonoBehaviour
{
    public GameObject manager;
    public GameObject ResultPanel;
    public GameObject DeathPanel;
    public TMP_Text ScoreText;
    public void ShowResultScreen()
    {
        ResultPanel.SetActive(true);
        ScoreText.text = $"Score: {StageScore.Instance.CurrentScore}";
    }
    public void returnStageList()
    {
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
