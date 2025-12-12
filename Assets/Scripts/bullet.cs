using UnityEngine;
public class bullet : MonoBehaviour
{
    public int damage;
    public float destructionYBoundary = -150f;

    private void Start()
    {
        damage = Statsmanger.Instance.GetDamage();
        float currentY = gameObject.transform.position.y;
        if (currentY > destructionYBoundary)
            {
                Destroy(gameObject);
                return;
            }
        else
        {
            Destroy(gameObject, 5f);
        }
    }
}
