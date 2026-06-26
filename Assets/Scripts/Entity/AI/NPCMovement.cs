using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Steering Settings")]
    public float rayDistance = 1.5f;
    public LayerMask obstacleLayer;
    public int rayCount = 8;
    public float rayAngle = 90f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetMovement(Vector2 desiredDirection, float speed)
    {
        if (desiredDirection == Vector2.zero)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 finalDirection = Steer(desiredDirection);
        rb.velocity = finalDirection * speed;
    }

    private Vector2 Steer(Vector2 desired)
    {
        Vector2 final = desired;
        
        // Cast rays in a fan shape to detect obstacles
        for (int i = 0; i < rayCount; i++)
        {
            float angle = Mathf.Lerp(-rayAngle / 2, rayAngle / 2, (float)i / (rayCount - 1));
            Vector2 dir = Quaternion.Euler(0, 0, angle) * desired;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayDistance, obstacleLayer);
            
            if (hit.collider != null)
            {
                // If we hit a wall, steer away from the normal of the hit
                Vector2 awayFromWall = Vector2.Reflect(desired, hit.normal);
                final += awayFromWall * 0.5f; // Adjust force
            }
        }
        return final.normalized;
    }
}