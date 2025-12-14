using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    public Slider healthSlider;
    private float maxDisplayHealth = 100f;
    private float currentDisplayHealth;
    private float MaxHealth;
    public EnemyData Data;
    private float currentHealth;
    public EnemySpawnController Controller;

    [SerializeField] private float invEnemyTime = 1.5f;
    private float invEnemyFrame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        if (gameObject.name != Data.EnemyName)
        {
            List<Slider> HealthBarList = EnemySpawnManager.DupeEnemyHealthList;
            foreach (Slider HealthBar in HealthBarList)
            {
                if (HealthBar != null && HealthBar.name == $"{gameObject.name}HPBar")
                    { 
                        healthSlider = HealthBar;
                    }
            }
            MaxHealth = Data.maxHealth;
            healthSlider.maxValue = maxDisplayHealth;
            currentHealth = MaxHealth;
            currentDisplayHealth = currentHealth * maxDisplayHealth / MaxHealth;
            healthSlider.value = currentDisplayHealth;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Collider2D EnemyCollide = gameObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(EnemyCollide, collision);
            bullet playerScript = collision.gameObject.GetComponent<bullet>();
            if (playerScript != null)
            {
                HealthBarChange(playerScript.damage);
                Destroy(collision.gameObject);  
            }
        }
        else if (collision.gameObject.CompareTag("Player") && Time.time >= invEnemyFrame)
        {
            StartCoroutine(IgnoreCollisionTemp(collision, invEnemyTime));
            HealthBarChange(999999);
            invEnemyFrame = Time.time + invEnemyTime;
        }
    }

    private IEnumerator IgnoreCollisionTemp(Collider2D collider, float duration)
    {
        Collider2D EnemyCollide = gameObject.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(EnemyCollide, collider);
        yield return new WaitForSeconds(duration);
        if (collider != null) 
        {
            Physics2D.IgnoreCollision(EnemyCollide, collider, false);
        }
    }

    public void HealthBarChange(int hpReduced) {
        currentHealth -= hpReduced;
        currentDisplayHealth = currentHealth * maxDisplayHealth / MaxHealth;
        healthSlider.value = currentDisplayHealth;
        if (currentHealth <= 0)
        {
            int EnemyScore = 100;
            StageScore.AddPoints(EnemyScore);
            if (EnemySpawnManager.DupeEnemyHealthList.Contains(healthSlider))
            {
                EnemySpawnManager.DupeEnemyHealthList.Remove(healthSlider);
            }
            if (EnemySpawnManager.DupeEnemyList.Contains(gameObject))
            {
                EnemySpawnManager.DupeEnemyList.Remove(gameObject);
            }
            if (healthSlider.gameObject != null) Destroy(healthSlider.gameObject);
            if (gameObject != null) Destroy(gameObject);
        }
    }
}
