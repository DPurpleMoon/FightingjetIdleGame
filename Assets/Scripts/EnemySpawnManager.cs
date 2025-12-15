using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UI;

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

    public async Task SpawnEnemy(int enemyamount, float distance, List<List<object>> route)
    {
        // Set EnemyType to selected EnemyName
        GameObject EnemyType;
        if (!_prefabMap.TryGetValue(Data.EnemyName, out EnemyType))
        {
            return; 
        }
        Controller = GetComponent<EnemySpawnController>();
        CancellationToken token = _cancellationTokenSource.Token;
        try
        {
            for (int i = 0; i < enemyamount; i++)
            {
                token.ThrowIfCancellationRequested();
                GameObject DupeEnemy = Instantiate(EnemyType, new Vector3(0, 0, 100), Quaternion.identity);
                GameObject DupeHealthCanvas = Instantiate(HealthBar, HealthCanvas);
                Slider DupeHealth = DupeHealthCanvas.GetComponent<Slider>();
                DupeEnemy.name = $"{EnemyType.name}{i + 1}";
                DupeHealth.name = $"{EnemyType.name}{i + 1}HPBar";
                DupeEnemyList.Add(DupeEnemy);
                DupeEnemyHealthList.Add(DupeHealth);
                List<Vector2> Waypoints = new List<Vector2>{};
                foreach (List<object> path in route)
                {
                    Waypoints.AddRange(Controller.PathFind((string)path[0], (Vector2)path[1], (Vector2)path[2], (Vector2)path[3], 0.05f));
                }
                Controller.SetPath(DupeEnemy, DupeHealth, Waypoints, Data.Speed);
                await Task.Delay((int)(distance * 1000f), token); 
            }
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }
}
