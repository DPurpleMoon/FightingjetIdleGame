using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemySpawnManager : MonoBehaviour {
    public static EnemySpawnManager Instance { get; private set; } 
    public EnemySpawnController Controller;
    public List<GameObject> enemyPrefabs;
    private Dictionary<string, GameObject> _prefabMap = new Dictionary<string, GameObject>();
    public EnemyData Data; 
    public GameObject HealthBar;
    public Transform HealthCanvas;
    public static List<GameObject> DupeEnemyList = new List<GameObject>();
    public static List<Slider> DupeEnemyHealthList = new List<Slider>();
    private CancellationTokenSource _cancellationTokenSource;
    public GameObject EnemySpawnManObject;
    void Awake()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        if (Instance != null && Instance != this)
        {
            // Destroy this object if another instance already exists
            Destroy(gameObject);
            return;
        }
        else
        {
            // Set the static reference to this instance
            Instance = this;
            // Often used to keep managers alive across scene loads
            DontDestroyOnLoad(gameObject); 
        }
        // Build the dictionary once at the start of the scene
        foreach (GameObject prefab in enemyPrefabs)
        {
            _prefabMap.Add(prefab.name, prefab);
        }
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    public IEnumerator SpawnEnemy(int enemyamount, float distance, List<List<object>> route, string EnemyName, float Speed, List<object> stats)
    {
        string AttackType = (string)stats[0];
        float Shootrate = (float)stats[1];
        float maxHealth = (float)stats[2];
        float BulletSpeed = (float)stats[3];
        float BulletSpawnDistance = (float)stats[4];
    
        // Set EnemyType to selected EnemyName
        GameObject EnemyType;
        if (!_prefabMap.TryGetValue(EnemyName, out EnemyType))
        {
            yield break; 
        }
        Controller = GetComponent<EnemySpawnController>();
        CancellationToken token = _cancellationTokenSource.Token;
        List<Vector2> Waypoints = new List<Vector2>{};
        foreach (List<object> path in route)
        {
            Waypoints.AddRange(Controller.PathFind((string)path[0], (Vector2)path[1], (Vector2)path[2], (Vector2)path[3], 0.05f));
        }
        for (int i = 0; i < enemyamount; i++)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (token.IsCancellationRequested) yield break;
            if (currentScene.name != "Stage0") yield break;
            GameObject DupeEnemy = Instantiate(EnemyType, new Vector3(0, 0, 100), Quaternion.identity);
            object[] healthstats = new object[] {EnemyName, maxHealth};
            DupeEnemy.SendMessage("Initialize", healthstats, SendMessageOptions.DontRequireReceiver);
            object[] attackstats = new object[] {AttackType, Shootrate, BulletSpeed, BulletSpawnDistance};
            Debug.Log("yes");
            Debug.Log(string.Join(", ", attackstats));
            DupeEnemy.SendMessage("BulletInit", attackstats, SendMessageOptions.DontRequireReceiver);
            GameObject DupeHealthCanvas = Instantiate(HealthBar, HealthCanvas);
            Slider DupeHealth = DupeHealthCanvas.GetComponent<Slider>();
            String enemyId = EnemyQueue();
            DupeEnemy.name = enemyId;
            DupeHealth.name = $"{enemyId}HPBar";
            DupeEnemyList.Add(DupeEnemy);
            DupeEnemyHealthList.Add(DupeHealth);
            Controller.SetPath(DupeEnemy, DupeHealth, Waypoints, Speed);
            yield return new WaitForSeconds(distance);
        }
        gameObject.SendMessage("HandleTaskDone", true, SendMessageOptions.DontRequireReceiver);
    }

    private string EnemyQueue()
    {
        int i = 1;
        List<string> EnemyList = new List<string>();
        foreach (GameObject go in DupeEnemyList)
        {
            EnemyList.Add(go.name);
        }
        while (true)
        {
            if (!EnemyList.Contains($"Enemy{i}"))
            {
                return $"Enemy{i}";
            }
            else
            {
                i++;
            }
        }
    }
}
