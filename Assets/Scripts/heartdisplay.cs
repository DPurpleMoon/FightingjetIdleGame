using UnityEngine;
using UnityEngine.UI;

 
public class heartdisplay : MonoBehaviour
{
    [Header("Heart Images - ADD 7 HEART IMAGES IN INSPECTOR")]
    [Tooltip("Drag 7 Image components here from your UI")]
    public Image[] hearts;
    
    [Header("Heart Sprites")]
    [Tooltip("Sprite to show when heart is full")]
    public Sprite fullHeart;
    
    [Tooltip("Sprite to show when heart is empty")]
    public Sprite emptyHeart;
    
    [Header("Player Reference")]
    [Tooltip("Reference to player health script - will auto-find if empty")]
    public jetfighter.movement.PlayerHealth playerHealth;
    
    // Private variables
    private int maxHearts;
    private int currentHearts;
    private int lastKnownHealth = -1;  

    void Start()
    {
         if (playerHealth == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerHealth = playerObj.GetComponent<jetfighter.movement.PlayerHealth>();
            }
        }
        
         if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth not found! Make sure player has 'Player' tag.");
            enabled = false;
            return;
        }
        
         if (hearts == null || hearts.Length == 0)
        {
            Debug.LogError("No heart images assigned! Please assign heart images in inspector.");
            enabled = false;
            return;
        }
        
         maxHearts = playerHealth.GetMaxHealth();
        currentHearts = playerHealth.GetCurrentHealth();
        lastKnownHealth = currentHearts;
        
         if (hearts.Length < maxHearts)
        {
            Debug.LogWarning($"Not enough heart images! Need {maxHearts} but only have {hearts.Length}");
        }
        
         UpdateHearts();
        
        Debug.Log($"Heart Display Initialized. Health: {currentHearts}/{maxHearts}, Heart Images: {hearts.Length}");
    }

    void Update()
    {
        if (playerHealth == null) return;
        
        int newHealth = playerHealth.GetCurrentHealth();
        
        if (newHealth != lastKnownHealth)
        {
            currentHearts = newHealth;
            lastKnownHealth = newHealth;
            UpdateHearts();
            
            Debug.Log($"Health changed! New health: {currentHearts}/{maxHearts}");
        }
    }
   
    void UpdateHearts()
    {
         if (fullHeart == null || emptyHeart == null)
        {
            Debug.LogError("Heart sprites not assigned!");
            return;
        }
        
         for (int i = 0; i < hearts.Length; i++)
        {
             if (hearts[i] == null)
            {
                Debug.LogWarning($"Heart image at index {i} is null!");
                continue;
            }
            
             if (i < maxHearts)
            {
                 hearts[i].enabled = true;
                
                 if (i < currentHearts)
                {
                     hearts[i].sprite = fullHeart;
                }
                else
                {
                     hearts[i].sprite = emptyHeart;
                }
            }
            else
            {
                 hearts[i].enabled = false;
            }
        }
        
         Debug.Log($"Hearts updated: {currentHearts}/{maxHearts} hearts shown");
    }
    
    public void RefreshHearts()
    {
        if (playerHealth != null)
        {
            currentHearts = playerHealth.GetCurrentHealth();
            maxHearts = playerHealth.GetMaxHealth();
            UpdateHearts();
        }
    }
}