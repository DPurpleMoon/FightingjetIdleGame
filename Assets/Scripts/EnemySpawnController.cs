using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnController : MonoBehaviour {
    private GameObject EnemyType;
    public EnemySpawnManager Manager;
    public EnemyData Data; 

    //testing purpose
    private void Awake() 
    {   
        // Set EnemyType to selected EnemyName
        EnemyType = GameObject.Find(Data.EnemyName);
        Manager = GetComponent<EnemySpawnManager>();
        List<float[]> Waypoints = PathFind("Line", new float[] {-50f, -50f}, new float[] {50f, 50f}, new float[] {0f, 0f}, 3f);
        StartCoroutine(WaitPath(EnemyType, Waypoints, 3f));
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

    private void Update()
    {
    }

    public List<float[]> PathFind(string type, float[] startpoint, float[] endpoint, float[] midpoint, float speed)
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
                    if (startpoint[0] < endpoint[0])
                    {
                        for (float x = startpoint[0]; x < endpoint[0]; x += Math.Abs(speed))
                        {
                            float y = constant[0] * x + constant[1];
                            Waypoints.Add(new float[] {x, y});
                        }
                    }
                    
                    // from right to left
                    else if (startpoint[0] > endpoint[0])
                    {
                        for (float x = startpoint[0]; x > endpoint[0]; x -= Math.Abs(speed))
                        {
                            float y = constant[0] * x + constant[1];
                            Waypoints.Add(new float[] {x, y});
                        }
                    }
                } else if (startpoint[1] != endpoint[1]){
                    // from down to up
                    float x = startpoint[0];
                    if (startpoint[1] < endpoint[1])
                    {
                        for (float y = startpoint[1]; y < endpoint[1]; y += Math.Abs(speed))
                        {
                            Waypoints.Add(new float[] {x, y});
                        }
                    }
                    // from down to up
                    else if (startpoint[1] > endpoint[1])
                    {
                        for (float y = startpoint[1]; y > endpoint[1]; y -= Math.Abs(speed))
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
                    float x = midpoint[0] + Mathf.Cos(angle) * constant[0];
                    float y = midpoint[1] + Mathf.Sin(angle) * constant[0];
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