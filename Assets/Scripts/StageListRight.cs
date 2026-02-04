using UnityEngine;

public class StageListRight : MonoBehaviour
{
    public StageListData Data;
    void OnMouseDown()
    {
        Data.stagenum++;
    }
    void Update()
    {
        if (Data.stagenum == Data.maxstagenum)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
