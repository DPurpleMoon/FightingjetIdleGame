using UnityEngine;

public class BulletSelfDestruct : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 savedVelocity;
    private float savedAngularVelocity;
    public float destructionYBoundary = -150f;
    void Awake()
    {
        if (gameObject.CompareTag("EnemyBullet"))
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    void Update()
    {
        if (gameObject.name != "EnemyBullet"  && gameObject.CompareTag("EnemyBullet"))
        {
            float currentY = gameObject.transform.position.y;
            if (currentY < destructionYBoundary)
            {
                Destroy(gameObject);
                return;
            }
        } 
        else
        {
            return;
        }
    }
    public void BulInitialize(Vector2 startingVelocity, bool startPaused)
    {
        savedVelocity = startingVelocity;
        
        if (startPaused)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = startingVelocity;
        }
    }

    public void PauseBullet()
    {
        if (rb != null)
        {
            if (rb.linearVelocity != Vector2.zero)
            {
                savedVelocity = rb.linearVelocity;
                savedAngularVelocity = rb.angularVelocity;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }

    public void ResumeBullet()
    {
        if (rb != null)
        {
            rb.linearVelocity = savedVelocity;
            rb.angularVelocity = savedAngularVelocity;
            if (rb.linearVelocity == Vector2.zero)
            {
                Destroy(gameObject);
            }
        }
    }
}
