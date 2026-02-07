using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class EndStageHandler : MonoBehaviour{
    public ResultScreen Result;
    public StageScrollingData StageData;

    void Awake()
    {
        StageData.enemySpawnEnd = false;
        StageData.stageReadEnd = false;
        StartCoroutine(EndStageCheck());
    }

    public IEnumerator EndStageCheck()
    {
        yield return new WaitUntil(() => 
                StageData.stageReadEnd && 
                StageData.enemySpawnEnd && 
                EnemySpawnManager.DupeEnemyList.All(enemy => enemy == null)
            );
        Result = GetComponent<ResultScreen>();
        Result.ShowResultScreen();
        yield break;
    }
}