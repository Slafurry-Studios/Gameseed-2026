using UnityEngine;

namespace Game.AI 
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class NPCMovement : MonoBehaviour
    {
        [SerializeField] private float turnSpeed = 360f;

        private Rigidbody2D rb;
        private Vector2 currentDirection;
        private float currentSpeed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f; 
        }

        public void SetMovement(Vector2 direction, float speed)
        {
            currentDirection = direction;
            currentSpeed = speed;
        }

        private void FixedUpdate()
        {
            rb.velocity = currentDirection * currentSpeed;

            if (currentDirection.sqrMagnitude > 0.01f)
            {
                float targetAngle = Vector2.SignedAngle(Vector2.up, currentDirection);
                float newAngle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, turnSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(newAngle);
            }
        }
    }
}