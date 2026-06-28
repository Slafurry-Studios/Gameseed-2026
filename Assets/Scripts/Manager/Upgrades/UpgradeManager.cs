using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Game.Manager;

namespace Game.Upgrade
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private UpgradeCard[] UpgradeCards;

        public Action<UpgradeCard> SetCard1;
        public Action<UpgradeCard> SetCard2;
        public Action<UpgradeCard> SetCard3;
        public Action<bool> OnUpgrade;

        private UpgradeCard card1;
        private UpgradeCard card2;
        private UpgradeCard card3;

        void Start()
        {
            GameManager.Instance.subsManager.OnCurrentSubsStateChanged += SetNewUpgrades;
        }

        void OnDisable()
        {
            if (GameManager.Instance != null && GameManager.Instance.subsManager != null)
                GameManager.Instance.subsManager.OnCurrentSubsStateChanged -= SetNewUpgrades;
        }

        private void SetNewUpgrades(int threshold)
        {
            List<UpgradeCard> rolled = RollUniqueCards(3, threshold);

            SetCard1.Invoke(rolled[0]);
            SetCard2.Invoke(rolled[1]);
            SetCard3.Invoke(rolled[2]);


            SetCard1?.Invoke(card1);
            SetCard2?.Invoke(card2);
            SetCard3?.Invoke(card3);
            OnUpgrade.Invoke(true);
        }

        private List<UpgradeCard> RollUniqueCards(int count, int threshold)
        {
            List<UpgradeCard> pool = UpgradeCards.ToList();

            if (pool.Count < count)
            {
                Debug.LogWarning("Jumlah UpgradeCardDatas kurang dari jumlah kartu yang diminta!");
                count = pool.Count;
            }

            List<UpgradeCard> result = new List<UpgradeCard>();

            for (int i = 0; i < count; i++)
            {
                UpgradeCard picked = WeightedPick(pool);
                result.Add(picked);
                pool.Remove(picked);
            }

            return result;
        }

        private UpgradeCard WeightedPick(List<UpgradeCard> pool)
        {
            float totalWeight = pool.Sum(c => Mathf.Max(c.UpgradeCardData.Weight, 0f));

            if (totalWeight <= 0f)
            {
                return pool[UnityEngine.Random.Range(0, pool.Count)];
            }

            float roll = UnityEngine.Random.Range(0f, totalWeight);
            float cumulative = 0f;

            foreach (var card in pool)
            {
                cumulative += Mathf.Max(card.UpgradeCardData.Weight, 0f);
                if (roll <= cumulative)
                    return card;
            }

            return pool[pool.Count - 1];
        }
    }
}