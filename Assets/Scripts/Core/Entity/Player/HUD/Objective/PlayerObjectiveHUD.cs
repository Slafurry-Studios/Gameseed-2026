using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.HUD
{
    public class PlayerObjectiveHUD : MonoBehaviour
    {
        [SerializeField] private PlayerObjectiveItem itemPrefab;
        [SerializeField] private Transform itemParent;

        private readonly List<PlayerObjectiveItem> playerObjectiveItems = new();
        private readonly Dictionary<Objective, PlayerObjectiveItem> itemLookup = new();

        private System.Collections.IEnumerator Start()
        {
            yield return new WaitUntil(() => ObjectiveManager.Instance != null);
            ObjectiveManager.Instance.OnObjectiveAdded += AddObjectiveItem;
            ObjectiveManager.Instance.OnObjectiveProgress += HandleProgress;
            ObjectiveManager.Instance.OnObjectiveCompleted += HandleCompleted;
        }

        private void OnDisable()
        {
            ObjectiveManager.Instance.OnObjectiveAdded -= AddObjectiveItem;
            ObjectiveManager.Instance.OnObjectiveProgress -= HandleProgress;
            ObjectiveManager.Instance.OnObjectiveCompleted -= HandleCompleted;
        }

        public void AddObjectiveItem(Objective objective)
        {
            if (itemLookup.ContainsKey(objective)) return;

            var item = Instantiate(itemPrefab, itemParent);
            var progress = new ObjectiveProgress(objective);
            item.Setup(progress);

            playerObjectiveItems.Add(item);
            itemLookup[objective] = item;

            SortItems();
        }

        private void HandleProgress(Objective objective, float currentValue)
        {
            if (!itemLookup.TryGetValue(objective, out var item))
            {
                AddObjectiveItem(objective);
                item = itemLookup[objective];
            }

            item.GetProgress().CurrentValue = currentValue;
            item.RefreshText();
        }

        private void HandleCompleted(Objective objective)
        {
            if (itemLookup.TryGetValue(objective, out var item))
            {
                item.TaskCompleted();
            }
        }

        private void SortItems()
        {
            playerObjectiveItems.Sort((a, b) =>
            {
                bool aMain = a.GetProgress().Data.IsMainMission;
                bool bMain = b.GetProgress().Data.IsMainMission;
                return bMain.CompareTo(aMain);
            });

            for (int i = 0; i < playerObjectiveItems.Count; i++)
            {
                playerObjectiveItems[i].transform.SetSiblingIndex(i);
            }
        }

        private void Update()
        {
            foreach (var item in playerObjectiveItems)
            {
                item.RefreshText();
            }
        }
    }
}