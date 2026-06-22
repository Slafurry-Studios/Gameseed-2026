using UnityEngine;
using Game.Player;

namespace Game.Gameplay.Attributes
{
    [CreateAssetMenu(fileName = "NewThreatPointAttribute", menuName = "Game/Dropable/Attributes/Threat Point Attribute")]
    public class ThreatPointAttribute : AttributeData
    {
        public override void Apply(GameObject target, float amount)
        {
            PlayerCollection collection = target.GetComponent<PlayerCollection>();
            if (collection != null)
            {
                collection.AddThreatPoints(Mathf.RoundToInt(amount));
            }
        }
    }
}
