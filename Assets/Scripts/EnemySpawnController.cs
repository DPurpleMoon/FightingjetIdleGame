using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemySpawnController : MonoBehaviour {
    
    private float[] EnemyRoute;
    private int[] EnemyPattern;
    public StageScrollingData Data;

    public void SetPath(GameObject enemy, Slider Health, List<Vector2> waypoints, float speed, bool boss){
        StartCoroutine(WaitPath(enemy, Health, waypoints, speed, boss));
    }

    IEnumerator WaitPath(GameObject enemy, Slider Health, List<Vector2> waypoints, float speed, bool boss)
    {
        int i = 0;
        Renderer renderer = enemy.GetComponent<Renderer>();
        if (renderer == null) yield break;
        float enemyheight = renderer.bounds.size.y;
        do {
            int n;
            foreach (Vector2 coordinate in waypoints)
            {   
                n = 0;
                Scene currentScene = SceneManager.GetActiveScene();
                if (enemy == null) yield break;
                Vector3 target = new Vector3(coordinate.x, coordinate.y, -20f);
                // Set health y position to current location of enemy + height of enemy/2 + 5f and x position to current location of enemy + 1.5f
                Vector3 HealthTarget = new Vector3(coordinate.x + 1.5f, coordinate.y + (enemyheight / 2) + 5f, 0f);
                if (i == 0)
                {
                    enemy.transform.position = target;
                    Health.transform.position = HealthTarget;
                }
                else 
                {
                    while (Vector3.Distance(enemy.transform.position, target) > 0.05f)
                    {
                        if (enemy == null) yield break;
                        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target, speed * Time.deltaTime);
                        Health.transform.position = Vector3.MoveTowards(Health.transform.position, HealthTarget, speed * Time.deltaTime);
                        if (Time.timeScale == 0) 
                        {
                            yield return null;
                            n = 5;
                        }
                        if (Time.timeScale == 1 && n != 0) 
                        {
                            yield return null;
                            n--;
                        }
                    }
                }
                if (i % speed == 0)
                {
                    yield return new WaitUntil(() => Time.timeScale > 0);;
                }
                i++;
                if (currentScene.name != "Stage0") yield break;
            }
        } while (boss == true && enemy != null);
        if (EnemySpawnManager.DupeEnemyHealthList.Contains(Health))
        {
            EnemySpawnManager.DupeEnemyHealthList.Remove(Health);
        }
        if (EnemySpawnManager.DupeEnemyList.Contains(enemy))
        {
            EnemySpawnManager.DupeEnemyList.Remove(enemy);
        }
        if (Health.gameObject != null) Destroy(Health.gameObject);
        if (enemy != null) Destroy(enemy);
        
    }


    public List<Vector2> PathFind(string type, Vector2 startpoint, Vector2 endpoint, Vector2 midpoint, float speed)
    {
        if (speed != 0)
        {
            List<Vector2> Waypoints = new List<Vector2>(); 
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
                            Waypoints.Add(new Vector2(x, y));
                        }
                    }
                    
                    // from right to left
                    else if (startpoint.x > endpoint.x)
                    {
                        for (float x = startpoint.x; x > endpoint.x; x -= Math.Abs(speed))
                        {
                            float y = constant[0] * x + constant[1];
                            Waypoints.Add(new Vector2(x, y));
                        }
                    }
                } else if (startpoint.y != endpoint.y){
                    // from down to up
                    float x = startpoint.x;
                    if (startpoint.y < endpoint.y)
                    {
                        for (float y = startpoint.y; y < endpoint.y; y += Math.Abs(speed))
                        {
                            Waypoints.Add(new Vector2(x, y));
                        }
                    }
                    // from down to up
                    else if (startpoint.y > endpoint.y)
                    {
                        for (float y = startpoint.y; y > endpoint.y; y -= Math.Abs(speed))
                        {
                            Waypoints.Add(new Vector2(x, y));
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
                    Waypoints.Add(new Vector2(x, y));
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
        if (!Mathf.Approximately((float)(Math.Pow(StartPointX - MidPointX, 2) + Math.Pow(StartPointY - MidPointY, 2)), (float)(Math.Pow(EndPointX - MidPointX, 2) + Math.Pow(EndPointY - MidPointY, 2))))
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
