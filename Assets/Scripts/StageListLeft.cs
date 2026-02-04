using UnityEngine;

public class StageListLeft : MonoBehaviour
{
    public StageListData Data;
    void OnMouseDown()
    {
        Data.stagenum--;
    }
    void Update()
    {
        if (Data.stagenum == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
