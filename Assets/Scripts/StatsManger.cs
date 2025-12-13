using UnityEngine;

public class Statsmanger : MonoBehaviour
{
    public static Statsmanger Instance { get; private set; }

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    
    [Header("damege Settings")]
    [SerializeField] private int damage = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetDamage()
    {
        return damage;
    }
}