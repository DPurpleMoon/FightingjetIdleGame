using UnityEngine;

public class BulletSelfDestruct : MonoBehaviour
{
    public float destructionYBoundary = -150f;
    void Update()
    {
        if (gameObject.name != "EnemyBullet")
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
}
