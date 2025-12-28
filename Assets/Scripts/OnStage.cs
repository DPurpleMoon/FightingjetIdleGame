using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class OnStage : MonoBehaviour
{
    public EnemyData Data; 
    public StageScrollingData StageData;
    public StageScrollingController StageScript;
    public StageRead Read;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage0")
        {
            List<List<object>> CoordinateList = new List<List<object>>{}; 
            StageScript = GetComponent<StageScrollingController>();
            Read = GetComponent<StageRead>();
            List<object> LevelData = Read.FileRead("level0-1");
                        
            StageData.ScrollCoordinate = -3000f;
            StageData.AccelerationConstant = 5.0f;
            StageData.MaxVelocity = 100f;
            StageData.StageName = (string)LevelData[0];
            StageScript.Initiate();
            float multiplier = float.Parse((string)LevelData[1]);
            StartCoroutine(EnemySpawn(LevelData));
        }
    }
    
    private IEnumerator EnemySpawn(List<object> Level)
    {
        List<List<object>> CoordinateList = new List<List<object>>{};
        for (int i = 2; i < Level.Count;)
            {
                List<object> EnemyDetails = (List<object>)Level[i];
                if (-StageScript.ActualLocation.y >= (int)EnemyDetails[0])
                {
                    Data.EnemyName = (string)EnemyDetails[1];
                    Data.AttackType = (string)EnemyDetails[2];

                    foreach (List<object> Coordinate in (List<object>)EnemyDetails[3])
                    {
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
                        else
                        {
                            yield return null;
                        }
                        List<object> Paths = new List<object>{MovementType, StartPoint, EndPoint, MidPoint};  
                        CoordinateList.Add(Paths);
                    }
                    // x (-188 > 188), y (-110, 110)
                    Data.Speed = 10f;
                    Data.Shootrate = 1f;
                    EnemySpawnManager.Instance.SpawnEnemy(5, 5f, CoordinateList);
                    i++;
                }
                else
                {
                    yield return null;
                }
            }
    }
}
