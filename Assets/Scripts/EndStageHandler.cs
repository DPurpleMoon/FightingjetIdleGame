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
            yield return new WaitUntil(() => Time.timeScale > 0);
        }
        else
        {
            Result = GetComponent<ResultScreen>();
            Result.ShowResultScreen();
            yield break;
        }
    }
}