using UnityEngine;

namespace jetfighter.movement
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private int maxHealth = 4; 
        private int currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
            Debug.Log("Player Health: " + currentHealth + "/" + maxHealth);
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            
            if (currentHealth < 0)
                currentHealth = 0;

            Debug.Log("Player took " + damage + " damage! Health: " + currentHealth + "/" + maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(int amount)
        {
            if (currentHealth >= maxHealth)
            {
                Debug.Log("Health already full!");
                return;
            }

            currentHealth += amount;
            
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            
            Debug.Log("Player healed! Health: " + currentHealth + "/" + maxHealth);
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        private void Die()
        {
            Debug.Log("Player Died!");
         
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                TakeDamage(1); 
            }
        }
    }
}