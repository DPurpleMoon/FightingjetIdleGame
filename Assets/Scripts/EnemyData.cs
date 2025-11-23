using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;
    public string MovementType;
    public float Speed;
    public float[] StartPoint;
    public float[] MidPoint;
    public float[] EndPoint;
}