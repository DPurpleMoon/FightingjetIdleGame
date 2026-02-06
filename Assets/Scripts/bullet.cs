using UnityEngine;

public class bullet : MonoBehaviour
{
    public int damage = 10; // Default
    public float destructionYBoundary;

    void Start()
    {
        destructionYBoundary = 700f;
    }

    // NEW FUNCTION: Called by gun.cs immediately after spawning
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    void Update()
    {
        // Removed the Statsmanger dependency. 
        // Logic handles movement/destruction only.

        if (gameObject.name != "bullet")
        {
            float currentY = gameObject.transform.position.y;
            if (currentY > destructionYBoundary)
            {
                Destroy(gameObject);
            }
        }
    }
}