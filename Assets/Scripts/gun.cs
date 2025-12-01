using UnityEngine;

namespace jetfighter.movement
{
    public class gun : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireForce = 20f;
        [SerializeField] private float fireRate = 0.2f; 
        
        private float nextFireTime = 0f;

         private Collider2D playerCollider;
         private void Start()
        
        {
            playerCollider = GetComponentInParent<Collider2D>();
            if (playerCollider == null)
            {
                playerCollider = GetComponentInChildren<Collider2D>();
            }
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }

        void Fire()
        {
            if (bulletPrefab == null || firePoint == null)
            {
                Debug.LogWarning("Bullet Prefab or Fire Point not assigned!");
                return;
            }

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            }
            Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
            if (bulletCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, playerCollider);
            }
        }

    }

    
}
