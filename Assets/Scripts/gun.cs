using UnityEngine;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace jetfighter.movement
{
    public class gun : MonoBehaviour
    {
        [Header("Default Settings (If Shop is Empty)")]
        [SerializeField] private float defaultFireForce = 20f;
        [SerializeField] private float defaultFireRate = 0.2f;

        [Header("References")]
        [SerializeField] public GameObject defaultBulletPrefab;
        [SerializeField] public Transform firePoint;

        [Header("Configuration")]
        public List<WeaponData> availableWeapons;
        
        public float FireTime;
        private Collider2D playerCollider;
        public WeaponData currentWeapon;
        public GameObject manObj;

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
            // 1. Get the current weapon from savefile
            manObj = GameObject.Find("SaveLoadManager");
            SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
            currentWeapon = availableWeapons[(int)SaveLoad.LoadGame("EquipWeapon")];

            // 2. Determine Fire Rate (Use Weapon Data or Default)
            float effectiveFireRate = (currentWeapon != null) ? currentWeapon.fireRate : defaultFireRate;
            if (Input.GetKey(KeyCode.Space) && Time.time >= FireTime && Time.timeScale == 1f)
            {
                Fire(currentWeapon);
                Debug.Log("Shot");
                FireTime = Time.time + effectiveFireRate;
            }
        }

        void Fire(WeaponData weaponData)
        {
            if (defaultBulletPrefab == null)
            {
                Debug.LogWarning("Gun Error: Bullet Prefab missing!");
                defaultBulletPrefab = GameObject.Find("bullet");
                return;
            }
            if (firePoint == null)
            {
                Debug.LogWarning("Gun Error: Fire Point missing!");
                return;
            }

            // A. Instantiate Bullet
            GameObject bulletObj = Instantiate(defaultBulletPrefab, firePoint.position, firePoint.rotation);

            // B. Set Damage (The key integration point!)
            bullet bulletScript = bulletObj.GetComponent<bullet>();
            if (bulletScript != null)
            {
                // Get damage from Shop, or use default 10
                if (manObj == null)
                {
                    manObj = GameObject.Find("SaveLoadManager");
                }
                SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
                string WeaponUnlockedString = (string)SaveLoad.LoadGame("PurchasedWeapons");
                if (string.IsNullOrEmpty(WeaponUnlockedString))
                {
                    WeaponUnlockedString = "{\"weaponList\": [{\"WeaponName\": \"Basic Blaster\", \"WeaponLevel\": 1}]}";
                }
                JsonNode jsonNode = JsonNode.Parse(WeaponUnlockedString);
                JsonArray WeaponDataList = jsonNode?["weaponList"]?.AsArray();
                if (WeaponDataList != null)
                foreach (WeaponData weapon in availableWeapons)
                {
                    foreach (var WeaponDetails in WeaponDataList)
                    {
                        if ((string)WeaponDetails?["WeaponName"] == (string)weapon.name)
                        {
                            weapon.currentLevel = (int)WeaponDetails?["WeaponLevel"] ;
                        }
                    }
                }
                double damageVal = (weaponData != null) ? weaponData.baseDamage + ((weaponData.currentLevel - 1) * weaponData.damageMultiplier) : 10;
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
                rbp.AddForce(firePoint.up * force * 5, ForceMode2D.Impulse);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPlayerShoot();
            }
        }
    }
}