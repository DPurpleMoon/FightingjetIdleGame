using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EnemySpawnManager : MonoBehaviour {
    public static EnemySpawnManager Instance { get; private set; } 
    public EnemySpawnController Controller;
    public List<GameObject> enemyPrefabs;
    private Dictionary<string, GameObject> _prefabMap = new Dictionary<string, GameObject>();
    public GameObject Test_SpecificEnemyPrefab;
    public EnemyData Data; 
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Destroy this object if another instance already exists
            Destroy(gameObject);
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

    public async Task SpawnEnemy(int enemyamount, float distance)
    {
        // Set EnemyType to selected EnemyName
        GameObject EnemyType;
        if (!_prefabMap.TryGetValue(Data.EnemyName, out EnemyType))
        {
            return; 
        }
        Controller = GetComponent<EnemySpawnController>();
        
        EnemyType = Test_SpecificEnemyPrefab;
        for (int i = 0; i < enemyamount; i++)
        {
            Controller.EnemySpawn(EnemyType, i, distance);
            await Task.Delay((int)(distance * 1000f)); 
        }
    }
}
