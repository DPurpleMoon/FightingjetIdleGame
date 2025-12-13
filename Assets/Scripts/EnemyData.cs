using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;
    public string MovementType;
    public float Speed;
    public float maxHealth;
    public float BulletSpeed;
    public float BulletSpawnDistance;
    public float Shootrate;
}