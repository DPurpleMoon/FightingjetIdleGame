using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public EnemySpawnController Controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Controller = GetComponent<EnemySpawnController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
