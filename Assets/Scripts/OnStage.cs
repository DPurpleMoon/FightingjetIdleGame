using UnityEngine;
using UnityEngine.SceneManagement;
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
            StageScript = GetComponent<StageScrollingController>();
            Data.EnemyName = "enemydummy";
            Data.MovementType = "Line";
            Data.StartPoint = new Vector2(-100, -100); 
            Data.MidPoint = new Vector2(0, 0);
            Data.EndPoint = new Vector2(100, 100);
            Data.Speed = 10f;
            StageData.ScrollCoordinate = -3000f;
            StageData.AccelerationConstant = 5.0f;
            StageData.MaxVelocity = 100f;
            StageScript.Initiate("stagedummy_0");
            StageScript.Scroll();
            EnemySpawnManager.Instance.SpawnEnemy(2, 20f);
        }
    }
}
