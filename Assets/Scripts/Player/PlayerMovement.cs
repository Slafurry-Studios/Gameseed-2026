using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float forwardSpeed = 5f;
        [SerializeField] private float turnSpeed = 360f; // Kecepatan berputar (derajat per detik)
        
        [Header("Input")]
        [SerializeField] private InputActionReference moveAction;

        private Rigidbody2D rb;
        private float targetAngle;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            targetAngle = rb.rotation;
        }

        private void OnEnable()
        {
            if (moveAction != null)
            {
                moveAction.action.Enable();
            }
        }

        private void OnDisable()
        {
            if (moveAction != null)
            {
                moveAction.action.Disable();
            }
        }

        private void Update()
        {
            if (moveAction != null)
            {
                Vector2 input = moveAction.action.ReadValue<Vector2>();
                
                if (input.sqrMagnitude > 0.01f)
                {
                    targetAngle = Vector2.SignedAngle(Vector2.up, input.normalized);
                }
            }
        }

        private void FixedUpdate()
        {
            float newAngle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newAngle);

            rb.velocity = transform.up * forwardSpeed;

            rb.angularVelocity = 0f;
        }
    }
}
