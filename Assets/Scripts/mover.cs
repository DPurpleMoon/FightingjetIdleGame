using UnityEngine;

namespace jetfighter.movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class mover : MonoBehaviour
    {
        [SerializeField] private float movementspeed = 5f;
        private Rigidbody2D body;
        protected Vector3 currentinput;
        
        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            currentinput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );
        }
        
        private void FixedUpdate()
        {
            body.linearVelocity = currentinput * movementspeed;
        }
    }
}