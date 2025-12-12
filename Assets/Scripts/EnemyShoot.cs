using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EnemyShoot : MonoBehaviour
{
    public EnemySpawnController Controller;
    public GameObject EnemyBulletObject;
    public GameObject SelectedEnemy;
    public EnemyData Data;

    void Start()
    {
        Controller = GetComponent<EnemySpawnController>();
        List<GameObject> EnemyList = EnemySpawnController.DupeEnemyList;
        foreach (GameObject Enemy in EnemyList)
            {
            if (Enemy != null && Enemy.name == gameObject.name)
                { 
                    SelectedEnemy = gameObject;
                }
            }
        if (SelectedEnemy != null)
        {
            StartCoroutine(ShootContinuous(SelectedEnemy));
        }
    }

    IEnumerator ShootContinuous(GameObject enemy){
        float timer = 0f;
        while (enemy != null)
        {
            if (timer > Data.Shootrate)
            {
                Shoot(enemy);
                timer = 0f;
                yield return null;
            }
            else
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void Shoot(GameObject enemy)
    {
        Vector3 SpawnLocation = enemy.transform.position - new Vector3(0, Data.BulletSpawnDistance, 0);   
        GameObject bullet = Instantiate(EnemyBulletObject, SpawnLocation, Quaternion.Euler(180f, 0f, 0f));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(bullet.transform.up * Data.BulletSpeed, ForceMode2D.Impulse);
        }
    }
}
