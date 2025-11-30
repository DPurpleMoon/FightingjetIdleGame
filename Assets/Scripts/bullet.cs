using UnityEngine;


    public class bullet : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }
        private void Start()
        {
            Destroy(gameObject, 5f);
        }
    }
