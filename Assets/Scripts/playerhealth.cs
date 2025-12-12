using UnityEngine;

namespace jetfighter.movement
{
    public class PlayerHealth : MonoBehaviour
    {
        
        private int currentHealth;

        private void Start()
        {
            currentHealth = Statsmanger.Instance.GetMaxHealth();
            Debug.Log("Player Health: " + currentHealth + "/" + Statsmanger.Instance.GetMaxHealth());
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            
            if (currentHealth < 0)
                currentHealth = 0;

            Debug.Log("Player took " + damage + " damage! Health: " + currentHealth + "/" + Statsmanger.Instance.GetMaxHealth());

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(int amount)
        {
            int maxHealth = Statsmanger.Instance.GetMaxHealth();
            
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
            return Statsmanger.Instance.GetMaxHealth();
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