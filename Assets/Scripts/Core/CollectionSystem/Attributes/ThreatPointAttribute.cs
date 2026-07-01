using UnityEngine;
using Game.Manager;
using System;

namespace Game.Gameplay.Attributes
{
    [CreateAssetMenu(fileName = "NewThreatPointAttribute", menuName = "Game/Dropable/Attributes/Threat Point Attribute")]
    public class ThreatPointAttribute : AttributeData
    {
        public override void Apply(GameObject target, float amount)
        {
            GameManager.Instance.AddThreat((int)Math.Round(amount));
            SoundManager.Instance.PlaySound2D("Collection_Point");
        }
    }
}
