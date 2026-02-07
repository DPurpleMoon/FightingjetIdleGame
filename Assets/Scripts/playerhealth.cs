using UnityEngine;
using UnityEngine.SceneManagement;

namespace jetfighter.movement
{ 
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private int maxHealth = 7; 
        
        [Header("Invincibility Settings")]
        [SerializeField] private float invTime = 1.5f;
        
        private int currentHealth;
        private float invFrame;
        private bool isDead = false;  
        public GameObject DeathPanel;
        public GameObject Body;
        
        private void Start()
        {
            currentHealth = maxHealth;
            isDead = false;
            SpriteRenderer bodyRenderer = Body.GetComponent<SpriteRenderer>();
            bodyRenderer.enabled = true;
            Debug.Log("Player Health Initialized: " + currentHealth + "/" + maxHealth);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (isDead) return;
            
            bool isEnemyBullet = collision.gameObject.CompareTag("EnemyBullet");
            bool isEnemy = collision.gameObject.CompareTag("Enemy");
            
            if ((isEnemyBullet || isEnemy) && Time.time >= invFrame)
            {
                Collider2D PlayerCollider = gameObject.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(PlayerCollider, collision);
                
                 if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayPlayerTakeDamage();
                }
                
                TakeDamage(1);
                
                if (isEnemyBullet && collision.gameObject != null)
                {
                    Destroy(collision.gameObject);
                }
                
                 if (!isDead)
                {
                    invFrame = Time.time + invTime;
                }
            }
        }

         
        public void TakeDamage(int damage)
        {
            if (isDead) return;
            
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
            if (isDead) return;
            
            if (currentHealth >= maxHealth)
            {
                Debug.Log("Health already full!");
                return;
            }

            currentHealth += amount;
            
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
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
         
        public void SetMaxHealth(int newMaxHealth)
        {
            maxHealth = newMaxHealth;
            
             if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            
            Debug.Log("Max health set to: " + maxHealth);
        }
 
        private void Die()
        {
            if (isDead) return;
            
            isDead = true;
            
            Debug.Log("Player Died!");
            
             if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPlayerDeath();
            }
            
            
            PlayerController controller = GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.enabled = false;
            }
            
             
            jetfighter.movement.gun gunScript = GetComponentInChildren<jetfighter.movement.gun>();
            if (gunScript != null)
            {
                gunScript.enabled = false;
            }
            

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPlayerDeath();
            }
            Time.timeScale = 0f;
            DeathPanel.SetActive(true);
            SpriteRenderer bodyRenderer = Body.GetComponent<SpriteRenderer>();
            bodyRenderer.enabled = false;
        }
        
         
        public bool IsDead()
        {
            return isDead;
        }
    }
}