using UnityEngine;

namespace jetfighter.movement
{
    public class gun : MonoBehaviour
    {
        [Header("User Weapon System")]
        public WeaponData currentWeapon;   // Drag your Scriptable Object here
        public SpriteRenderer gunRenderer; // Drag the gun's SpriteRenderer here

        [Header("Rushdi's Logic (Auto-filled)")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireForce = 20f;
        [SerializeField] private float fireRate = 0.2f; 
        
        private float nextFireTime = 0f;
        private Collider2D playerCollider;
        private statsUI statsMenu; 

        private void Start()
        {
            // Connect to Stats UI
            statsMenu = FindObjectOfType<statsUI>();

            // Find Player Collider
            playerCollider = GetComponentInParent<Collider2D>();
            if (playerCollider == null)
            {
                playerCollider = GetComponentInChildren<Collider2D>();
            }

            // --- INTEGRATION: Equip the weapon immediately ---
            if (currentWeapon != null)
            {
                EquipWeapon(currentWeapon);
            }
        }

        public void EquipWeapon(WeaponData newWeapon)
        {
            currentWeapon = newWeapon;

            // 1. Update Shooting Mechanics
            if (newWeapon.bulletPrefab != null)
                bulletPrefab = newWeapon.bulletPrefab;

            fireRate = newWeapon.fireRate;
            fireForce = newWeapon.fireForce;

            // 2. Update Visuals
            if (gunRenderer != null && newWeapon.icon != null)
            {
                gunRenderer.sprite = newWeapon.icon;
            }

            // 3. Update Damage in StatsManager
            // We convert 'double' to 'int' because StatsManager uses int
            if (Statsmanger.Instance != null)
            {
                int calculatedDamage = (int)newWeapon.GetDamage();
                Statsmanger.Instance.SetDamage(calculatedDamage);
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime && !IsMenuOpen())
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
            
            Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
            if (bulletCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, playerCollider);
            }
            
            Rigidbody2D rbp = bullet.GetComponent<Rigidbody2D>();
            if (rbp != null)
            {
                rbp.AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            }
        }

        private bool IsMenuOpen()
        {
            if (statsMenu != null)
            {
                return statsMenu.IsMenuOpen();
            }
            return false;
        }
    }
}