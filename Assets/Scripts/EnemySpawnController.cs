using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EnemySpawnController : MonoBehaviour {
    
    private float[] EnemyRoute;
    private int[] EnemyPattern;
    public static List<GameObject> DupeEnemyList = new List<GameObject>();
    
    public EnemyData Data; 

    public void EnemySpawn(GameObject EnemyType, int count, float distance) {
        GameObject DupeEnemy = Instantiate(EnemyType, new Vector3(0, 0, 100), Quaternion.identity);
        DupeEnemy.name = $"{EnemyType.name}{count + 1}";
        DupeEnemyList.Add(DupeEnemy);
        
        if (DupeEnemy != null)
            {
                List<float[]> Waypoints = PathFind(Data.MovementType, Data.StartPoint, Data.EndPoint, Data.MidPoint, 0.05f);
                StartCoroutine(WaitPath(DupeEnemy, Waypoints, Data.Speed));
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
                float[] constant = LineFormula(startpoint, endpoint);

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
                float[] constant = CircleFormula(startpoint, endpoint, midpoint);
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

    public float[] LineFormula(Vector2 StartPoint, Vector2 EndPoint)
    {
        float StartPointX = StartPoint.x;
        float StartPointY = StartPoint.y;
        float EndPointX = EndPoint.x;
        float EndPointY = EndPoint.y;
        if (StartPointX - EndPointX != 0)
        {
            float Gradient = (StartPointY - EndPointY) / (StartPointX - EndPointX);
            float HShift = StartPointY - (Gradient * StartPointX);
            // {Gradient, HorizontalShift}
            float[] Constant = new float[] {Gradient, HShift};
            return Constant;
        }
        else
        {
            return null;
        }
    }
    
    public float[] CircleFormula(Vector2 StartPoint, Vector2 EndPoint, Vector2 MidPoint)
    {
        float StartPointX = StartPoint.x;
        float StartPointY = StartPoint.y;
        float EndPointX = EndPoint.x;
        float EndPointY = EndPoint.y;
        float MidPointX = MidPoint.x;
        float MidPointY = MidPoint.y;
        float Radius;
        float StartRadian = 0;
        float EndRadian = 0;
        if (Math.Pow(StartPointX - MidPointX, 2) + Math.Pow(StartPointY - MidPointY, 2) != Math.Pow(EndPointX - MidPointX, 2) + Math.Pow(EndPointY - MidPointY, 2))
        {
            throw new ArgumentException("Length of StartPoint to MidPoint and EndPoint to MidPoint is not the same.");
        }
        else
        {
            Radius = (float)Math.Sqrt(Math.Pow(StartPointX - MidPointX, 2) + Math.Pow(StartPointY - MidPointY, 2));
            
            // Find StartRadian
            // Calculate distance between starting point and 0 degree 
            float LineBetween = (float)Math.Sqrt(Math.Pow(StartPointX - MidPointX, 2) + Math.Pow(StartPointY - MidPointY + Radius, 2));
            StartRadian = (float)Math.Acos(((2 * Math.Pow(Radius, 2)) - Math.Pow(LineBetween, 2)) / (2 * Math.Pow(Radius, 2)));
            // Correct quardrant
            if (StartPointX > MidPointX)
            {
                StartRadian = 2*Mathf.PI - StartRadian;
            }
            

            // Find EndRadian
            float LineBetweenEnd = (float)Math.Sqrt(Math.Pow(EndPointX - MidPointX, 2) + Math.Pow(EndPointY - MidPointY + Radius, 2));
            EndRadian = (float)Math.Acos(((2 * Math.Pow(Radius, 2)) - Math.Pow(LineBetweenEnd, 2)) / (2 * Math.Pow(Radius, 2)));
            // Correct quardrant 
            if (EndPointX > MidPointX)
            {
                EndRadian = 2*Mathf.PI - EndRadian;
            }
            // Increase radian by 2π(360°) if EndRadian is smaller than StartRadian to ensure that the path will always move towards counter clockwise 
            if (StartRadian > EndRadian)
            {
                EndRadian += 2*Mathf.PI;
            }
            
        float[] Constant = new float[] {Radius, StartRadian + Mathf.PI, EndRadian + Mathf.PI};
        return Constant;
        }
    }
}
