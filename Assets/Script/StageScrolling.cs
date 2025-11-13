using UnityEngine;
using System;

public class StageScrolling : MonoBehaviour {
    public static float ScrollCoordinate = 0f;
    public static bool inStage = false;
    public static Vector2 TargetPosition;
    public static Vector2 ActualLocation = new Vector2(0, 0);
    public GameObject CurrentStage;
    public GameObject DupeStage;
    public static bool StageCloned = true;
    private float StageMoveVelocity;
    private float Multiplier = 0.6f;
    public string StageBGName;
    public float AccelarationConstant;
    public float StartingVelocity;
    public float MaxVelocity;

    // Testing purpose
    private void Awake()
    {
        StageCloned = false;
        StageBGName = "stagedummy_0";
        inStage = true;
        ScrollCoordinate = -200f;
        MaxVelocity = 10f;
        StartingVelocity = 0.6f;
        AccelarationConstant = 1.1f;
    }

    private void Update()
    {
        // Get stage background name and height of the background
        CurrentStage = GameObject.Find(StageBGName);
        Renderer renderer = CurrentStage.GetComponent<Renderer>();
        float BGHeight = GetComponent<Renderer>().bounds.size.y;

        // Clone the background once and put it on top of the original background
        if (!StageCloned && CurrentStage != null)
        {
            DupeStage = Instantiate(CurrentStage, new Vector3(0, BGHeight, 0), Quaternion.identity);
            StageCloned = true;
        }

        // Accelaration method
        if ((ActualLocation != TargetPosition) && (Multiplier < MaxVelocity))
        {
            Multiplier = Multiplier * AccelarationConstant; 
            StageMoveVelocity = Multiplier * Time.deltaTime;
        }
        else if ((ActualLocation != TargetPosition) && (Multiplier > MaxVelocity))
        {
            Multiplier = MaxVelocity; 
            StageMoveVelocity = Multiplier * Time.deltaTime;

        }
        else if (ActualLocation == TargetPosition)
        {
            Multiplier = StartingVelocity;
        }
        else if (((ActualLocation.y - TargetPosition.y) < MathF.Log((MaxVelocity / StartingVelocity * (1 - AccelarationConstant)) - 1, 1 - AccelarationConstant)) && (Multiplier > StartingVelocity))
        {
            Multiplier = Multiplier * (1 / AccelarationConstant); 
            StageMoveVelocity = Multiplier * Time.deltaTime;
        }

        if (DupeStage == null)
        {
            TargetPosition = new Vector2(0, ScrollCoordinate);
        }

        // Scroll background when inStage is true and TargetPosition is smaller than ActualLocation
        // ActualLocation and TargetPosition is towards negative
        if ((inStage == true) && (ActualLocation.y > TargetPosition.y)) {
            if (DupeStage == null)
            {
                CurrentStage.transform.position = new Vector3(0, ((ActualLocation.y - TargetPosition.y) % BGHeight) - BGHeight, 0);
                ActualLocation = Vector2.MoveTowards(ActualLocation, TargetPosition, StageMoveVelocity);
                Debug.Log(ActualLocation);
            }
            if (DupeStage != null)
            {   
                DupeStage.transform.position = new Vector3(0, (ActualLocation.y - TargetPosition.y) % BGHeight, 0);
            }
        }
        else
        {
            ActualLocation = TargetPosition;
        }
    }
}
