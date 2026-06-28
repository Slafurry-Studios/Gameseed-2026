using UnityEngine;
using Game.Upgrade;

namespace Game.Upgrade.Effect
{
    [CreateAssetMenu(fileName = "NewExampleEffect", menuName = "Game/Upgrade/Effect/Example")]
    public class ThickSkinEffect : UpgradeCardEffect
    {
        public int armorBonus = 5;

        public override void Apply()
        {
            // PlayerStats.Instance.AddArmor(armorBonus);
        }
    }
}