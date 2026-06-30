using UnityEngine;
using Game.Manager;
using System;

namespace Game.Gameplay.Attributes
{
    [CreateAssetMenu(fileName = "NewSubsAttribute", menuName = "Game/Dropable/Attributes/Subs Attribute")]
    public class SubsAttribute : AttributeData
    {
        public override void Apply(GameObject target, float amount)
        {
            GameManager.Instance.AddThreat((int) Math.Round(amount));
        }
    }
}
