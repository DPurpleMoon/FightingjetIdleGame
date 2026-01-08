using UnityEngine;
using UnityEngine.UI;

public class heartdisplay : MonoBehaviour
{
    [Header("Heart Images")]
    public Image[] hearts;
    
    [Header("Heart Sprites")]
    public Sprite fullHeart;
    public Sprite emptyHeart;
    
    [Header("Player Reference")]
    public jetfighter.movement.PlayerHealth playerHealth;
    
    private int maxHearts;
    private int currentHearts;

    void Start()
    {
        if (playerHealth != null)
        {
            maxHearts = playerHealth.GetMaxHealth();
            currentHearts = playerHealth.GetCurrentHealth();
        }
        
        if (playerHealth == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerHealth = playerObj.GetComponent<jetfighter.movement.PlayerHealth>();
                if (playerHealth != null)
                {
                    maxHearts = playerHealth.GetMaxHealth();
                    currentHearts = playerHealth.GetCurrentHealth();
                }
            }
        }
        
        UpdateHearts();
        Debug.Log("Heart Display Initialized. Health: " + currentHearts + "/" + maxHearts);
    }

    void Update()
    {
        if (playerHealth != null)
        {
            int newHealth = playerHealth.GetCurrentHealth();
            
            if (newHealth != currentHearts)
            {
                currentHearts = newHealth;
                UpdateHearts();
                Debug.Log("Health changed! New health: " + currentHearts);
            }
        }
    }
    
    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHearts)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}