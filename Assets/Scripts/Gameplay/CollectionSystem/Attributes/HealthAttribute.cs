using UnityEngine;
using Game.Player;

namespace Game.Gameplay.Attributes
{
    [CreateAssetMenu(fileName = "NewHealthAttribute", menuName = "Game/Dropable/Attributes/Health Attribute")]
    public class HealthAttribute : AttributeData
    {
        public override void Apply(GameObject target, float amount)
        {
            PlayerHealth health = target.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(amount);
            }
        }
    }
}
