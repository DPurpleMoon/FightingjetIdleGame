using UnityEngine;

namespace jetfighter.movement
{
    public class PlayerHealth : MonoBehaviour
    {

        [SerializeField] private float invTime = 1.5f;
        private int currentHealth;
        private float invFrame;

        private void Start()
        {
            currentHealth = Statsmanger.Instance.GetMaxHealth();
            Debug.Log("Player Health: " + currentHealth + "/" + Statsmanger.Instance.GetMaxHealth());
        }

        void OnTriggerEnter2D(Collider2D collision)
            {
                Collider2D PlayerCollider = gameObject.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(PlayerCollider, collision);
                if ((collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("Enemy")) && Time.time >= invFrame)
                {
                    TakeDamage(1);
                    if (collision.gameObject.name == "EnemyBullet(Clone)")
                    {
                        Destroy(collision.gameObject);
                    }
                    invFrame = Time.time + invTime;
                }
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
    }
}