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
    public float ShootRate;
    public float bulletspeed;
    public float spawndistance;
    public string AttackType;
    public StageScrollingData Data; 

    void BulletInit(object[] data)
    {
        AttackType = (string)data[0];
        ShootRate = (float)data[1];
        bulletspeed = (float)data[2];
        spawndistance = (float)data[3];
    }
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
            List<object> BulletPattern = Pattern.AttackRead(AttackType);
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
                if (attackpattern[i] is List<float>)
                {
                    foreach (float angle in (List<float>)attackpattern[i])
                    {
                        Shoot(enemy, angle, bulletspeed, spawndistance);
                    }
                }
                timer = 0f;
                i++;
                yield return new WaitUntil(() => Time.timeScale > 0);
            }
            else
            {
                timer += Time.deltaTime;
                yield return new WaitUntil(() => Time.timeScale > 0);
            }
            if (i >= attackpattern.Count)
            {
                i = 0;
            }
        }
    }

    public void Shoot(GameObject enemy, float angle, float bulletspeed, float spawndistance)
    {
        float radians = (angle + 90) * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
        Vector3 SpawnLocation = enemy.transform.position + (direction * spawndistance);
        GameObject bullet = Instantiate(EnemyBulletObject, SpawnLocation, Quaternion.Euler(0f, 0f, angle - 90f));
        if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = direction * bulletspeed * 10f;
        }
    }
}
