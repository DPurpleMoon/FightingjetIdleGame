using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class EndStageHandler : MonoBehaviour{
    public StageScrollingController Stage;
    public IEnumerator EndStageCheck()
    {
        Stage = GetComponent<StageScrollingController>();
        if (EnemySpawnManager.DupeEnemyList.All(item => item == null) != true)
        {
            yield return new WaitUntil(() => Time.timeScale > 0);
        }
        else 
        {
            Stage.LeaveStage();
            yield break;
        }
    }
}