using UnityEngine;
using System;
using Game.Utils;

namespace Game.Manager
{

    public class SubscriptionPointManager : MonoBehaviour
    {
        [SerializeField] private Threshold[] subsThresholds;
        public event Action<float> OnSubsPointIncreased;
        public event Action<int> OnCurrentSubsStateChanged;
        private int subsPoints = 0;
        private int currentSubsState = 0;

        public void IncreasePoints(int amount)
        {
            subsPoints += amount;

            int newThreshold = GetCurrentThreshold();

            OnSubsPointIncreased?.Invoke(subsPoints);

            if (newThreshold > currentSubsState)
            {
                currentSubsState = newThreshold;

                OnCurrentSubsStateChanged?.Invoke(currentSubsState);
            }
        }

        private int GetCurrentThreshold()
        {
            int level = 0;

            for (int i = 0; i < subsThresholds.Length; i++)
            {
                if (subsPoints >= subsThresholds[i].ThresholdValue)
                {
                    level = i;
                }
            }

            return level;
        }

        public int GetPoint()
        {
            return subsPoints;
        }
    }
}