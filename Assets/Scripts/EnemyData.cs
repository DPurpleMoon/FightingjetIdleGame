using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;
    public string MovementType;
    public float Speed;
    public Vector2 StartPoint;
    public Vector2 MidPoint;
    public Vector2 EndPoint;
    public float maxHealth;
    public float BulletSpeed;
    public float BulletSpawnDistance;
    public float Shootrate;
}