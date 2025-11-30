using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EnemySpawnController : MonoBehaviour {
    private GameObject EnemyType;
    public EnemySpawnManager Manager;
    public EnemyData Data; 
    private GameObject DupeEnemy;
    public static EnemySpawnController Instance { get; private set; } 
    public List<GameObject> activeEnemies = new List<GameObject>();

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
    }

    void Start()
    {
        if (DupeEnemy != null) 
        {
            List<float[]> Waypoints = PathFind(Data.MovementType, Data.StartPoint, Data.EndPoint, Data.MidPoint, 0.05f);
            StartCoroutine(WaitPath(DupeEnemy, Waypoints, Data.Speed));
        }
    }

    public async Task SpawnEnemy(int enemyamount, float distance)
    {
        // Set EnemyType to selected EnemyName
        EnemyType = GameObject.Find(Data.EnemyName);
        Manager = GetComponent<EnemySpawnManager>();
        for (int i = 0; i < enemyamount; i++)
        {
            DupeEnemy = Instantiate(EnemyType, new Vector3(0, 0, 100), Quaternion.identity);
            activeEnemies.Add(DupeEnemy);
            await Task.Delay((int)(distance * 1000f)); 
        }
    }

    IEnumerator WaitPath(GameObject enemy, List<float[]> waypoints, float speed)
    {
        int i = 0;
        foreach (float[] coordinate in waypoints)
        {
            Vector3 target = new Vector3(coordinate[0], coordinate[1], -0.001f);
            if (i == 0)
            {
                enemy.transform.position = target;
            }
            else 
            {
                while (Vector3.Distance(enemy.transform.position, target) > 0.1f)
                {
                    enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target, speed * Time.deltaTime);
                    yield return null;
                }
            }
            i++;
        }
    }


    public List<float[]> PathFind(string type, Vector2 startpoint, Vector2 endpoint, Vector2 midpoint, float speed)
    {
        if (speed != 0)
        {
            List<float[]> Waypoints = new List<float[]>(); 
            if (type == "Line")
            {
                float[] constant = Manager.LineFormula(startpoint, endpoint);

                if (constant != null)
                {
                    // from left to right
                    if (startpoint.x < endpoint.x)
                    {
                        for (float x = startpoint.x; x < endpoint.x; x += Math.Abs(speed))
                        {
                            float y = constant[0] * x + constant[1];
                            Waypoints.Add(new float[] {x, y});
                        }
                    }
                    
                    // from right to left
                    else if (startpoint.x > endpoint.x)
                    {
                        for (float x = startpoint.x; x > endpoint.x; x -= Math.Abs(speed))
                        {
                            float y = constant[0] * x + constant[1];
                            Waypoints.Add(new float[] {x, y});
                        }
                    }
                } else if (startpoint.y != endpoint.y){
                    // from down to up
                    float x = startpoint.x;
                    if (startpoint.y < endpoint.y)
                    {
                        for (float y = startpoint.y; y < endpoint.y; y += Math.Abs(speed))
                        {
                            Waypoints.Add(new float[] {x, y});
                        }
                    }
                    // from down to up
                    else if (startpoint.y > endpoint.y)
                    {
                        for (float y = startpoint.y; y > endpoint.y; y -= Math.Abs(speed))
                        {
                            Waypoints.Add(new float[] {x, y});
                        }
                    }
                } else {
                    throw new ArgumentException("Cannot set start point and end point as the same coordinate");
                }
            }
            else if (type == "Circle")
            {
                float[] constant = Manager.CircleFormula(startpoint, endpoint, midpoint);
                for (float angle = constant[1]; angle < constant[2]; angle += Math.Abs(speed))
                {
                    float x = midpoint.x + (Mathf.Cos(angle) * constant[0]);
                    float y = midpoint.y + (Mathf.Sin(angle) * constant[0]);
                    Debug.Log(x);
                    Waypoints.Add(new float[] {x, y});
                }
                if (speed < 0)
                {
                    Waypoints.Reverse();
                }
            }
            return Waypoints;
        }
        else 
        {
            return null;
        }
    }
}
