using UnityEngine;
using System;

public class EnemySpawnManager : MonoBehaviour {
    private float[] EnemyRoute;
    private int[] EnemyPattern;
    private void Awake()
    {
        return;
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
