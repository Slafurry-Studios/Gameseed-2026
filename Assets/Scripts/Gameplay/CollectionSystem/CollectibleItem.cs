using UnityEngine;
using Game.Player;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class CollectibleItem : MonoBehaviour
    {
        [Header("Collectible Settings")]
        [Tooltip("The type of collectible this item represents.")]
        public CollectibleType type;
        
        [Tooltip("How much of this collectible to give when picked up.")]
        public int amount = 1;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerCollection collection = other.GetComponent<PlayerCollection>();
                if (collection != null)
                {
                    collection.Collect(type, amount);
                    
                    Destroy(gameObject);
                }
            }
        }
    }
}
