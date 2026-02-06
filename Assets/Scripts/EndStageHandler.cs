using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class EndStageHandler : MonoBehaviour{
    public ResultScreen Result;
    public IEnumerator EndStageCheck()
    {
        if (EnemySpawnManager.DupeEnemyList.All(item => item == null) != true)
        {
            yield return null;
        }
        else
        {
            Result = GetComponent<ResultScreen>();
            Result.ShowResultScreen();
            yield break;
        }
    }
}