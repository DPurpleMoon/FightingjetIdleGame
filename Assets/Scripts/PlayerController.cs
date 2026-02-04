using UnityEngine;
using UnityEngine.InputSystem;

namespace jetfighter.movement
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float movementSpeed = 20f;
        [SerializeField] private LayerMask wallLayer;

        private Rigidbody2D body;
        private Vector2 moveInput;
        private PlayerInput playerInput;
        
        private float movementSoundTimer = 0f;
        private float movementSoundInterval = 0.1f;
        private bool wasMoving = false; 

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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveInput.normalized, 15f, wallLayer);
        
            if (hit.collider == null)
            {
                body.linearVelocity = moveInput * movementSpeed;
            }    
            else
            {
                body.linearVelocity = Vector2.zero;
            }
            
            bool isMoving = moveInput.magnitude > 0;
            
            if (isMoving)
            {
                movementSoundTimer += Time.fixedDeltaTime;
                
                if (movementSoundTimer >= movementSoundInterval)
                {
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayPlayerMovement();
                        Debug.Log("Playing movement sound!");
                    }
                    else
                    {
                        Debug.LogWarning("AudioManager not found!");
                    }
                    
                    movementSoundTimer = 0f;
                }
                
                if (!wasMoving)
                {
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayPlayerMovement();
                        Debug.Log("Started moving - playing sound!");
                    }
                    movementSoundTimer = 0f;
                }
            }
            else
            {
                movementSoundTimer = 0f;
            }
            
            wasMoving = isMoving;
            
            // Debug.Log($"Velocity: {body.linearVelocity}");
        }
    }
}