using UnityEngine;
using System;

public class EnemySpawnManager : MonoBehaviour {
    private GameObject EnemyType;
    public static string EnemyName;
    private float[] EnemyRoute;
    private int[] EnemyPattern;
    private void Awake()
    {
        return;
    }
    private void Update()
    {
        // Set EnemyType to selected EnemyName
        EnemyType = GameObject.Find(EnemyName);
    }

    private float[] LineFormula(float[] StartPoint, float[] EndPoint)
    {
        float StartPointX = StartPoint[0];
        float StartPointY = StartPoint[1];
        float EndPointX = EndPoint[0];
        float EndPointY = EndPoint[1];
        float Gradient = (StartPointY - EndPointY) / (StartPointX - EndPointX);
        float HShift = StartPointY - (Gradient * StartPointX);
        // {Gradient, HorizontalShift}
        float[] Constant = new float[] {Gradient, HShift};
        return Constant;
    }
    
    private float[] CircleFormula(float[] StartPoint, float[] EndPoint, float[] MidPoint)
    {
        float StartPointX = StartPoint[0];
        float StartPointY = StartPoint[1];
        float EndPointX = EndPoint[0];
        float EndPointY = EndPoint[1];
        float MidPointX = MidPoint[0];
        float MidPointY = MidPoint[1];
        float Radius;
        float StartRadian;
        float EndRadian;
        if (Math.Pow(StartPointX - MidPointX, 2) + Math.Pow(StartPointY - MidPointY, 2) != Math.Pow(EndPointX - MidPointX, 2) + Math.Pow(EndPointY - MidPointY, 2))
        {
            throw new ArgumentException("Length of StartPoint to MidPoint and EndPoint to MidPoint is not the same.");
        }
        else
        {
            Radius = Math.Sqrt(Math.Pow(StartPointX - MidPointX, 2) + Math.Pow(StartPointY - MidPointY, 2));
            float LineBetween = Math.Sqrt(Math.Pow(StartPointX - MidPointX, 2) + Math.Pow(StartPointY - MidPointY + Radius, 2));
            StartRadian = () * MathF.PI / 180;
        }
        float[] Constant = new float[] {Radius, StartRadian, EndRadian};
        return Constant;
    }
}
