using UnityEngine;
using UnityEngine.InputSystem;

namespace jetfighter.movement
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float movementSpeed = 20f;

        private Rigidbody2D body;
        private Vector2 moveInput;
        private PlayerInput playerInput;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();
            
            playerInput.actions["Move"].performed += OnMove;
            playerInput.actions["Move"].canceled += OnMove;
        }

        private void OnDestroy()
        {
            playerInput.actions["Move"].performed -= OnMove;
            playerInput.actions["Move"].canceled -= OnMove;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
           // Debug.Log($"Move input: {moveInput}");
        }

        private void FixedUpdate()
        {
            body.linearVelocity = moveInput * movementSpeed;
           // Debug.Log($"Velocity: {body.linearVelocity}");
        }
    }
}