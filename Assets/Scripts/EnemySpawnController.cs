using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemySpawnController : MonoBehaviour {
    public Transform CircleCenter;
    private GameObject EnemyType;
    public EnemySpawnManager Manager;
    public EnemyData Data; 

    private void Awake() 
    {
        Manager = GetComponent<EnemySpawnManager>();
    }
    private void Update()
    {
        // Set EnemyType to selected EnemyName
        EnemyType = GameObject.Find(Data.EnemyName);
    }

    public List<float[]> PathFind(string type, float[] startpoint, float[] endpoint, float[] midpoint, float speed)
    {
        if (speed == 0)
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
                        for (float x = startpoint[0]; x < endpoint[0]; x += Math.Abs(speed) * Time.deltaTime)
                        {
                            float y = constant[0] * startpoint[0] + constant[1];
                            Waypoints.Add(new float[] {x, y});
                        }
                    }
                    
                    // from right to left
                    else if (startpoint[0] > endpoint[0])
                    {
                        for (float x = startpoint[0]; x > endpoint[0]; x -= Math.Abs(speed) * Time.deltaTime)
                        {
                            float y = constant[0] * startpoint[0] + constant[1];
                            Waypoints.Add(new float[] {x, y});
                        }
                    }
                } else if (startpoint[1] != endpoint[1]){
                    // from down to up
                    float x = startpoint[0];
                    if (startpoint[1] < endpoint[1])
                    {
                        for (float y = startpoint[1]; y < endpoint[1]; y += Math.Abs(speed) * Time.deltaTime)
                        {
                            Waypoints.Add(new float[] {x, y});
                        }
                    }
                    // from down to up
                    else if (startpoint[1] > endpoint[1])
                    {
                        for (float y = startpoint[1]; y > endpoint[1]; y -= Math.Abs(speed) * Time.deltaTime)
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
                for (float angle = constant[1]; angle < constant[2]; angle += Math.Abs(speed) * Time.deltaTime)
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
    }
}