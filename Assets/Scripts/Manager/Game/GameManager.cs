using UnityEngine;

namespace Game.Manager
{

    public class GameManager : Singleton<GameManager>
    {
        private ThreatPointManager threatManager;
        private SubscriptionPointManager subsManager;
        private UpgradeManager upgradeManager;

        void Start()
        {
            threatManager = GetComponent<ThreatPointManager>();
            subsManager = GetComponent<SubscriptionPointManager>();
            upgradeManager = GetComponent<UpgradeManager>();
        }

        public void AddThreat(int amount)
        {
            threatManager.IncreasePoints(amount);
        }

        public void AddSubs(int amount)
        {
            subsManager.IncreasePoints(amount);
        }
    }
}