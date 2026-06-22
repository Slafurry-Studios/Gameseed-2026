using UnityEngine;
using Game.Player;

namespace Game.Gameplay.Attributes
{
    [CreateAssetMenu(fileName = "NewTechPointAttribute", menuName = "Game/Dropable/Attributes/Tech Point Attribute")]
    public class TechPointAttribute : AttributeData
    {
        public override void Apply(GameObject target, float amount)
        {
            PlayerCollection collection = target.GetComponent<PlayerCollection>();
            if (collection != null)
            {
                collection.AddTechPoints(Mathf.RoundToInt(amount));
            }
        }
    }
}
