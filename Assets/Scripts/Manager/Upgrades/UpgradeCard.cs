using UnityEngine;
using UnityEngine.UI;

namespace Game.Upgrade
{

    [CreateAssetMenu(fileName = "NewUpgradeCard", menuName = "Game/Upgrade/Card")]
    public class UpgradeCard : ScriptableObject
    {
        public UpgradeCardData UpgradeCardData;
        public UpgradeCardEffect effect;
        public void OnSelected()
        {
            effect.Apply();
        }
    }
    [System.Serializable]
    public struct UpgradeCardData
    {
        public string UpgradeName;
        public string[] UpgradeDesc;
        public UpgradeType upgradeType;
        public Sprite UpgradeBg;
        public Sprite UpgradeIcon;

        [Tooltip("Drop Weight")]
        public float Weight;
    }
}