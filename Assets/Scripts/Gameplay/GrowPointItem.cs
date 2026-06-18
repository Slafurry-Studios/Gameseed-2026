using UnityEngine;
using Game.Player;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class GrowPointItem : MonoBehaviour
    {
        [Header("Grow Point Settings")]
        [Tooltip("How many grow points this item gives when collected.")]
        public int growPoints = 5;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerGrowth growth = other.GetComponent<PlayerGrowth>();
                if (growth != null)
                {
                    growth.AddGrowPoints(growPoints);
                    
                    Destroy(gameObject);
                }
            }
        }
    }
}
