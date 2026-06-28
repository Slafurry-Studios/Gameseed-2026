using UnityEngine;
using Game.Utils;
using System;

namespace Game.Manager
{

    public class ThreatPointManager : MonoBehaviour
    {
        [SerializeField] private Threshold[] threatThresholds;
        public event Action<float> OnThreatPointIncreased;

        private int threatPoints = 0;
        private int currentThreatState = 0;

        public void IncreasePoints(int amount)
        {
            threatPoints += amount;

            int newThreshold = GetCurrentThreshold();

            if (newThreshold > currentThreatState)
            {
                currentThreatState = newThreshold;

                OnThreatPointIncreased?.Invoke(threatPoints);

            }

        }

        private int GetCurrentThreshold()
        {
            int level = 0;

            for (int i = 0; i < threatThresholds.Length; i++)
            {
                if (threatPoints >= threatThresholds[i].ThresholdValue)
                {
                    level = i;
                }
            }

            return level;
        }

        public int GetPoint()
        {
            return threatPoints;
        }
    }
}