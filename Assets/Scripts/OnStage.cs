using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OnStage : MonoBehaviour
{
    public EnemyData Data; 
    public StageScrollingData StageData;
    public StageScrollingController StageScript;
    public StageRead Read;
    public EnemyStatRead StatRead;
    void Awake()
    {
        // Subscribe to the event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    // Unsubscribe when the script is destroyed to prevent memory leaks
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage0")
        {
            StageData.stageReadEnd = false;
            StageData.enemySpawnEnd = false;
            Time.timeScale = 1;
            List<List<object>> CoordinateList = new List<List<object>>{}; 
            StageScript = GetComponent<StageScrollingController>();
            Read = GetComponent<StageRead>();
            StatRead = GetComponent<EnemyStatRead>();
            List<object> LevelData = Read.FileRead(StageData.level);
            Debug.Log(StageData.level);
                        
            StageData.ScrollCoordinate = -999999f;
            StageData.AccelerationConstant = 5.0f;
            StageData.MaxVelocity = 100f;
            StageData.StageName = (string)LevelData[0];
            GameObject parentObj = GameObject.Find("BackgroundList");
            if (parentObj != null)
            {
                foreach (Transform Background in parentObj.transform)
                {
                    Background.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            GameObject CurrentStageImage = GameObject.Find(StageData.StageName);
            CurrentStageImage.GetComponent<SpriteRenderer>().enabled = true;
            StageScript.Initiate();
            float multiplier = float.Parse((string)LevelData[1]);
            StartCoroutine(EnemySpawn(LevelData));
        }
    }


    
    private IEnumerator EnemySpawn(List<object> Level)
    {
            int i = 2;
            bool boss = false;
            while (i < Level.Count)
                {
                    StageData.enemySpawnEnd = false;
                    List<object> EnemyDetails = (List<object>)Level[i];
                    if (-StageScript.ActualLocation.y >= (float)EnemyDetails[0])
                    {
                        List<List<object>> CoordinateList = new List<List<object>>{};
                        string EnemyName = (string)EnemyDetails[1];
                        List<object> Stat = StatRead.EnemyStat(EnemyName);
                        string AttackType = (string)Stat[1];
                        float Speed = (float)EnemyDetails[2];
                        float Shootrate = (float)Stat[0];
                        float maxHealth = (float)Stat[2];
                        float BulletSpeed = (float)Stat[3];
                        float BulletSpawnDistance = (float)Stat[4];
                        List<object> stats = new List<object> {AttackType, Shootrate, maxHealth, BulletSpeed, BulletSpawnDistance};

                        for (int ListCount = 5; ListCount < EnemyDetails.Count; ListCount++)
                        {
                            List<object> Coordinate = (List<object>)EnemyDetails[ListCount];
                            Vector2 StartPoint = Vector2.zero;
                            Vector2 MidPoint = Vector2.zero;
                            Vector2 EndPoint = Vector2.zero;
                            string MovementType = string.Empty;
                            if ((char)Coordinate[0] == 'L')
                            {
                                MovementType = "Line";
                                StartPoint = (Vector2)Coordinate[1];
                                EndPoint = (Vector2)Coordinate[2];
                            }
                            else if ((char)Coordinate[0] == 'C')
                            {
                                MovementType = "Circle";
                                StartPoint = (Vector2)Coordinate[1];
                                MidPoint = (Vector2)Coordinate[2];
                                EndPoint = (Vector2)Coordinate[3];
                            }
                            else if ((char)Coordinate[0] == 'B')
                            {
                                boss = true;
                            }
                            else
                            {
                                yield return new WaitUntil(() => Time.timeScale > 0);
                            }
                            List<object> Paths = new List<object>{MovementType, StartPoint, EndPoint, MidPoint};  
                            CoordinateList.Add(Paths);
                        }
                        // x (-188 > 188), y (-110, 110)
                        StartCoroutine(EnemySpawnManager.Instance.SpawnEnemy((int)EnemyDetails[3], (float)EnemyDetails[4], CoordinateList, EnemyName, Speed, stats, false, boss));
                        i++;
                        continue;
                    }
                yield return new WaitUntil(() => Time.timeScale > 0);
                }
                Debug.Log("yeesyess");
                StageData.stageReadEnd = true;
    }
}
