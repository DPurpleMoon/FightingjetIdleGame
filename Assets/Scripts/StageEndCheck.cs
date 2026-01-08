using UnityEngine;
using System;

public class StageEndCheck : MonoBehaviour
{
    public StageScrollingData Data;

    void HandleTaskDone(bool state)
    {
        Data.StageEnd = state;
    }
}