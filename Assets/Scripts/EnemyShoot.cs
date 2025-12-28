using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EnemyShoot : MonoBehaviour
{
    public EnemySpawnController Controller;
    public AttackPattern Pattern;
    public GameObject EnemyBulletObject;
    public GameObject SelectedEnemy;
    public EnemyData Data;

    void Start()
    {
        Controller = GetComponent<EnemySpawnController>();
        Pattern = GetComponent<AttackPattern>();
        List<GameObject> EnemyList = EnemySpawnManager.DupeEnemyList;
        foreach (GameObject Enemy in EnemyList)
            {
            if (Enemy != null && Enemy.name == gameObject.name)
                { 
                    SelectedEnemy = gameObject;
                }
            }
        if (SelectedEnemy != null)
        {
            List<object> BulletPattern = new List<object>{Pattern.AttackRead(Data.AttackType)};
            float ShootRate = Data.Shootrate;
            StartCoroutine(ShootContinuous(SelectedEnemy, BulletPattern, ShootRate));
        }
    }

    IEnumerator ShootContinuous(GameObject enemy, List<object> attackpattern, float shoottime){
        float timer = 0f;
        int i = 0;
        while (enemy != null)
        {
            if (timer > shoottime)
            {
                if ((int)attackpattern[i] != 0)
                {
                    foreach (float angle in (List<float>)attackpattern[i])
                    {
                        Shoot(enemy, angle);
                    }
                }
                timer = 0f;
                i++;
                yield return null;
            }
            else
            {
                timer += Time.deltaTime;
                yield return null;
            }
            if (i >= attackpattern.Count)
            {
                i = 0;
            }
        }
    }

    public void Shoot(GameObject enemy, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
        Vector3 SpawnLocation = enemy.transform.position + (direction * Data.BulletSpawnDistance);
        GameObject bullet = Instantiate(EnemyBulletObject, SpawnLocation, Quaternion.Euler(0f, 0f, angle - 90f));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * Data.BulletSpeed;
        }
    }
}
