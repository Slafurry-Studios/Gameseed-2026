using UnityEngine;
using System.Collections.Generic;
using Game.Player;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class CollectibleItem : MonoBehaviour
    {
        [Header("Collectible Settings")]
        [Tooltip("The attributes this item gives when picked up.")]
        public List<AttributeModifier> attributes = new List<AttributeModifier>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (var modifier in attributes)
                {
                    if (modifier.attribute != null)
                    {
                        modifier.attribute.Apply(other.gameObject, modifier.amount);
                    }
                }
                
                Destroy(gameObject);
            }
        }
    }
}
