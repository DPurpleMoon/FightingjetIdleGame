using UnityEngine;
public class bullet : MonoBehaviour
{
    public int damage;
    public float destructionYBoundary = 200f;

    void Start()
    {
        destructionYBoundary = 200f;
    }

    void Update()
    {
        if (gameObject.name != "bullet")
        {
            damage = Statsmanger.Instance.GetDamage();
            float currentY = gameObject.transform.position.y;
            if (currentY > destructionYBoundary)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
