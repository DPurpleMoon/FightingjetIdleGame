using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    public Slider healthSlider;
    private float maxDisplayHealth = 100f;
    private float currentDisplayHealth;
    public float MaxHealth;
    public string EnemyName;
    public EnemyData Data;
    private float currentHealth;
    public EnemySpawnController Controller;

    [SerializeField] private float invEnemyTime = 1.5f;
    private float invEnemyFrame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(object[] Data)
    {
        EnemyName = (string)Data[0];
        MaxHealth = (float)Data[1];
    }
    public void Start()
    {   
        if (healthSlider == null)
        {
            List<Slider> HealthBarList = EnemySpawnManager.DupeEnemyHealthList;
            foreach (Slider HealthBar in HealthBarList)
            {
                if (HealthBar != null && HealthBar.name == $"{gameObject.name}HPBar")
                    { 
                        healthSlider = HealthBar;
                        break;
                    }
            }
            if (healthSlider != null)
            {
                    healthSlider.maxValue = maxDisplayHealth;
                    currentHealth = MaxHealth;
                    if (MaxHealth > 0)
                    {
                        currentDisplayHealth = (currentHealth * maxDisplayHealth) / MaxHealth;
                    }
                    else
                    {
                        currentDisplayHealth = 0;
                    }
                    healthSlider.value = currentDisplayHealth;
            }
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
                if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayEnemyTakeDamage();
            }
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
        if (healthSlider != null)
        {
        healthSlider.value = currentDisplayHealth;
        }
        if (currentHealth <= 0)
        {
            int EnemyScore = 100;
            if (EnemySpawnManager.DupeEnemyHealthList.Contains(healthSlider))
            {
                StageScore.Instance.AddPoints(EnemyScore);
            }
            // change this to subscribe method later for ease of management
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
