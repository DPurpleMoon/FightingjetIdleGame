using UnityEngine;
using System;

public class StageScrollingController : MonoBehaviour {
    public static Vector2 TargetPosition;
    public static Vector2 ActualLocation = new Vector2(0, 0);
    public static bool StageCloned = true;
    public static GameObject CurrentStage;
    public GameObject DupeStage;
    private float StageMoveVelocity;
    private float Multiplier;
    public static float BGHeight;

    public StageScrollingData Stage; 

    // Default Values
    private void Awake()
    {
        Multiplier = Stage.StartingVelocity;
    }

    private void Update()
    {
        if (Stage.inStage){
            // Check if CurrentStage exists
            if (CurrentStage != null)
            {   
                if (!StageCloned)
                {
                    DupeStage = Instantiate(CurrentStage, new Vector3(0, BGHeight, 0), Quaternion.identity);
                    StageCloned = true;
                }
                // Acceleration method
                // Increase Multiplier if smaller than MaxVelocity
                if ((ActualLocation != TargetPosition) && (Multiplier < Stage.MaxVelocity))
                {
                    Multiplier = Multiplier * Stage.AccelerationConstant; 
                    StageMoveVelocity = Multiplier * Time.deltaTime;
                }
                // Set Multiplier to MaxVelocity if exceed
                else if ((ActualLocation != TargetPosition) && (Multiplier > Stage.MaxVelocity))
                {
                    Multiplier = Stage.MaxVelocity; 
                    StageMoveVelocity = Multiplier * Time.deltaTime;
                }
                // Stop if ActualLocation is equal to TargetPosition
                else if (ActualLocation == TargetPosition)
                {
                    Multiplier = Stage.StartingVelocity;
                }
                // Dynamic deacceleration when ActualLocation is almost equals to TargetPostion
                else if (((ActualLocation.y - TargetPosition.y) < MathF.Log((Stage.MaxVelocity / Stage.StartingVelocity * (1 - Stage.AccelerationConstant)) - 1, 1 - Stage.AccelerationConstant)) && (Multiplier > Stage.StartingVelocity))
                {
                    Multiplier = Multiplier * (1 / Stage.AccelerationConstant); 
                    StageMoveVelocity = Multiplier * Time.deltaTime;
                }

                // Only change TargetPosition in CurrentStage to prevent desync
                if (DupeStage == null)
                {
                    TargetPosition = new Vector2(0, Stage.ScrollCoordinate);
                }

                // Scroll background when inStage is true and TargetPosition is smaller than ActualLocation
                // ActualLocation and TargetPosition is towards negative
                if ((Stage.inStage) && (ActualLocation.y > TargetPosition.y)) {
                    if (DupeStage == null)
                    {
                        CurrentStage.transform.position = new Vector3(0, ((ActualLocation.y - TargetPosition.y) % BGHeight) - BGHeight, 0);
                        ActualLocation = Vector2.MoveTowards(ActualLocation, TargetPosition, StageMoveVelocity);
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
    }

    public void Initiate(string StageName){
        Stage.StageName = StageName;
        CurrentStage = GameObject.Find(Stage.StageName);
        CurrentStage.SetActive(true);
        Renderer renderer = CurrentStage.GetComponent<Renderer>();
        BGHeight = GetComponent<Renderer>().bounds.size.y;
        if (DupeStage != null)
        {
            Destroy(DupeStage);
        }
        StageCloned = false;
        Stage.inStage = true;
        // Clone the background once and put it on top of the original background
    }

    public void LeaveStage(){
        if (DupeStage != null)
        {
            Destroy(DupeStage);
            StageCloned = false;
        }
        Stage.inStage = false;
        CurrentStage.SetActive(false);
    }
}

