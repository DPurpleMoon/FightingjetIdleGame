using UnityEngine;

[CreateAssetMenu(fileName = "IdleData", menuName = "Idle/New Idle Data")]
public class IdleData : ScriptableObject
{
    public float baseIdleRate = 1f;        // Base currency per second
    public int upgradeCost = 100;

    // Idle rate increases by 10% for each stage level
    public float GetCurrentIdleRate(int currentStage)
    {
        return baseIdleRate * (1f + currentStage * 0.10f);
    }
}
