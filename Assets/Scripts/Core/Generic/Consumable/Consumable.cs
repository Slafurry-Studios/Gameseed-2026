using System.Collections;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float duration = 0.5f; 
    [SerializeField] private float pullDistance = 2.0f; 

    private SpriteRenderer spriteRenderer;
    private bool isConsumed = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Consume(Vector2 direction)
    {
        if (isConsumed) return;
        isConsumed = true;

        Debug.Log($"[Consumable] {gameObject.name} has been consumed.");

        StartCoroutine(ConsumeAnimation(direction.normalized));
    }

    private IEnumerator ConsumeAnimation(Vector2 dir)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + (Vector3)(dir * pullDistance);
        Vector3 startScale = transform.localScale;
        Color startColor = spriteRenderer != null ? spriteRenderer.color : Color.white;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 1. Pull Effect (Position)
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // 2. Shrink Effect (Scale)
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            // 3. Fade Out Effect (Alpha)
            if (spriteRenderer != null)
            {
                Color newColor = startColor;
                newColor.a = Mathf.Lerp(startColor.a, 0f, t);
                spriteRenderer.color = newColor;
            }

            yield return null;
        }

        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }
}