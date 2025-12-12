using UnityEngine;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        if (gameObject.name != Data.EnemyName)
        {
            Controller = GetComponent<EnemySpawnController>();
            List<Slider> HealthBarList = EnemySpawnController.DupeEnemyHealthList;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            PlayerScript playerScript = collision.gameObject.GetComponent<bullet>();
            if (playerScript != null)
            {
                HealthBarChange(playerScript.damage);
                Destroy(collision.gameObject);
            }
        }
    }

    public void HealthBarChange(int hpReduced) {
        currentHealth -= hpReduced;
        currentDisplayHealth = currentHealth * maxDisplayHealth / MaxHealth;
        healthSlider.value = currentDisplayHealth;
        if (currentHealth <= 0)
        {
            if (EnemySpawnController.DupeEnemyHealthList.Contains(healthSlider))
            {
                EnemySpawnController.DupeEnemyHealthList.Remove(healthSlider);
            }
            if (EnemySpawnController.DupeEnemyList.Contains(gameObject))
            {
                EnemySpawnController.DupeEnemyList.Remove(gameObject);
            }
            Destroy(healthSlider.gameObject);
            Destroy(gameObject);
        }
    }
}
