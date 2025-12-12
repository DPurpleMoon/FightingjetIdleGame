using UnityEngine;
public class bullet : MonoBehaviour
{
    private int damage;

    private void Start()
    {
        damage = Statsmanger.Instance.GetDamage();
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
           Enemy enemy = collision.gameObject.GetComponent<Enemy>();
           if (enemy != null)
            {
               enemy.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}