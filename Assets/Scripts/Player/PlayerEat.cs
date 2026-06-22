using UnityEngine;

namespace Game.Player
{

    public class PlayerEat : MonoBehaviour
    {
        [SerializeField] private float eatRange = 1.5f;
        [SerializeField] private LayerMask consumableLayer;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                EatConsumable();
            }
        }

        private void EatConsumable()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, eatRange, consumableLayer);
            foreach (var hitCollider in hitColliders)
            {
                Consumable consumable = hitCollider.GetComponent<Consumable>();
                if (consumable != null)
                {
                    Vector2 direction = (transform.position - hitCollider.transform.position).normalized;
                    consumable.Consume(direction);
                    Debug.Log($"[PlayerEat] Consumed: {hitCollider.name}");
                    break;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, eatRange);
        }
    }
}