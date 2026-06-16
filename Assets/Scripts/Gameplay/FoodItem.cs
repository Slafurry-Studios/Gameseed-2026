using UnityEngine;
using Game.Player;

namespace Game.Gameplay
{
    public class FoodItem : MonoBehaviour
    {
        [Header("Food Properties")]
        [Tooltip("How many tail segments are added when this food is eaten.")]
        public int growthRate = 1;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SnakeTailManager tailManager = other.GetComponent<SnakeTailManager>();
                if (tailManager != null)
                {
                    tailManager.Grow(growthRate);
                    
                    Destroy(gameObject);
                }
            }
        }
    }
}
