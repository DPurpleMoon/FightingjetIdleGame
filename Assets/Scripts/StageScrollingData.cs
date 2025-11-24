using UnityEngine;

[CreateAssetMenu(fileName = "StageScrollingData", menuName = "Stage/StageScrollingData")]
public class StageScrollingData : ScriptableObject
{
    public string StageName;
    public float ScrollCoordinate;
    public bool inStage;
    public float AccelerationConstant;
    public float StartingVelocity;
    public float MaxVelocity;
    
}
