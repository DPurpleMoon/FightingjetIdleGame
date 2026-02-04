using UnityEngine;

public class BulletSelfDestruct : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 savedVelocity;
    private float savedAngularVelocity;
    public float destructionYBoundary = -150f;
    void Awake()
    {
        if (gameObject.name != "EnemyBullet"  && gameObject.CompareTag("EnemyBullet"))
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
        rb.linearVelocity = startingVelocity;
    }
}
