using UnityEngine;
using Game.Player;

namespace Game.Gameplay.Attributes
{
    [CreateAssetMenu(fileName = "NewOrbAttribute", menuName = "Game/Dropable/Attributes/Orb Attribute")]
    public class OrbAttribute : AttributeData
    {
        public override void Apply(GameObject target, float amount)
        {
            PlayerCollection collection = target.GetComponent<PlayerCollection>();
            if (collection != null)
            {
                collection.AddOrbs(Mathf.RoundToInt(amount));
            }
        }
    }
}
