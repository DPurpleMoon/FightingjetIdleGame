using UnityEngine;
using System.Collections.Generic;

namespace jetfighter.movement
{
    public class gun : MonoBehaviour
    {
        [Header("Default Settings (If Shop is Empty)")]
        [SerializeField] private GameObject defaultBulletPrefab;
        [SerializeField] private float defaultFireForce = 20f;
        [SerializeField] private float defaultFireRate = 0.2f;

        [Header("References")]
        [SerializeField] private Transform firePoint;
        
        public float FireTime;
        private Collider2D playerCollider;
        public WeaponData currentWeapon;

        private void Start()
        {
            // Find Player Collider to ignore self-collision
            playerCollider = GetComponentInParent<Collider2D>();
            if (playerCollider == null)
            {
                playerCollider = GetComponentInChildren<Collider2D>();
            }
            currentWeapon = null;
        }
        private void Update()
        {
            // 1. Get the current weapon from Kuben's Shop
            if (ShopManager.Instance != null)
            {
                currentWeapon = ShopManager.Instance.equippedWeapon;
            }

            // 2. Determine Fire Rate (Use Weapon Data or Default)
            float effectiveFireRate = (currentWeapon != null) ? currentWeapon.fireRate : defaultFireRate;
            if (Input.GetKey(KeyCode.Space) && Time.time >= FireTime && Time.timeScale == 1f)
            {
                Debug.Log(currentWeapon);
                Fire(currentWeapon);
                FireTime = Time.time + effectiveFireRate;
            }
        }

        void Fire(WeaponData weaponData)
        {
            if (defaultBulletPrefab == null || firePoint == null)
            {
                Debug.LogWarning("Gun Error: Bullet Prefab or Fire Point missing!");
                return;
            }

            // A. Instantiate Bullet
            GameObject bulletObj = Instantiate(defaultBulletPrefab, firePoint.position, firePoint.rotation);

            // B. Set Damage (The key integration point!)
            bullet bulletScript = bulletObj.GetComponent<bullet>();
            if (bulletScript != null)
            {
                // Get damage from Shop, or use default 10
                double damageVal = (weaponData != null) ? weaponData.GetDamage() : 10;
                bulletScript.SetDamage((int)damageVal);
            }

            // C. Ignore Collision with Player
            Collider2D bulletCollider = bulletObj.GetComponent<Collider2D>();
            if (bulletCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, playerCollider);
            }

            // D. Apply Physics Force
            Rigidbody2D rbp = bulletObj.GetComponent<Rigidbody2D>();
            if (rbp != null)
            {
                // Use weapon's force if available, otherwise default
                float force = (weaponData != null) ? weaponData.fireForce : defaultFireForce;
                rbp.AddForce(firePoint.up * force, ForceMode2D.Impulse);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPlayerShoot();
            }
        }
    }
}