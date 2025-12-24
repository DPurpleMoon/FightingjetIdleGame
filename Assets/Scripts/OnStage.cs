using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class OnStage : MonoBehaviour
{
    public EnemyData Data; 
    public StageScrollingData StageData;
    public StageScrollingController StageScript;

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
            Data.EnemyName = "enemydummy";

            // rewrote to integrate stage reading from file system later
            string MovementType = "Line";
            // x (-188 > 188), y (-110, 110)
            Vector2 StartPoint = new Vector2(-188, 0); 
            Vector2 MidPoint = new Vector2(0, 0);
            Vector2 EndPoint = new Vector2(0, 0);
            List<object> Paths = new List<object>{MovementType, StartPoint, EndPoint, MidPoint};
            //
            CoordinateList.Add(Paths);
            
            StartPoint = new Vector2(0, 0); 
            MidPoint = new Vector2(0, 0);
            EndPoint = new Vector2(188, 100);
            Paths = new List<object>{MovementType, StartPoint, EndPoint, MidPoint};
            //
            CoordinateList.Add(Paths);


            Data.Speed = 15f;
            StageData.ScrollCoordinate = -3000f;
            StageData.AccelerationConstant = 5.0f;
            StageData.MaxVelocity = 100f;
            StageData.StageName = "forest";
            StageScript.Initiate();
            EnemySpawnManager.Instance.SpawnEnemy(5, 5f, CoordinateList);
        }
    }
}
