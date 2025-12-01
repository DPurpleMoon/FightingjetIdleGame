using UnityEngine;
using System.Collections.Generic;

public class EnemyHealthController : MonoBehaviour
{
    private Slider healthSlider;
    private float maxDisplayHealth = 100f;
    private float currentDisplayHealth;
    private float MaxHealth;
    public EnemyData Data;
    private float currentHealth;
    public EnemySpawnController Controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        Controller = GetComponent<EnemySpawnController>();
        List<Slider> HealthBarList = Controller.DupeEnemyHealthList;
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

    // health reduce demo
    void OnMouseDown()
    {
        HealthBarChange(5);
    }

    public void HealthBarChange(int hpReduced) {
        currentHealth -= hpReduced;
        currentDisplayHealth = currentHealth * maxDisplayHealth / MaxHealth;
        healthSlider.value = currentDisplayHealth;
        if (currentHealth <= 0)
        {
            if (Controller.DupeEnemyHealthList.Contains(healthSlider))
            {
                Controller.DupeEnemyHealthList.Remove(healthSlider);
            }
            if (Controller.DupeEnemyList.Contains(gameObject))
            {
                Controller.DupeEnemyList.Remove(gameObject);
            }
            Destroy(healthSlider);
            Destroy(gameObject);
        }
    }
}
