using UnityEngine;
using Game.Utils;
using System;

namespace Game.Manager
{

    public class ThreatPointManager : MonoBehaviour
    {
        [SerializeField] private int MaxPoint = 5000;
        [SerializeField] private Threshold[] threatThresholds;
        [SerializeField] private BaseObjectiveChannel[] threatPointChannel;
        public event Action<float, float> OnThreatPointIncreased;
        public event Action<int> OnCurrentThreatStateChanged;
        [SerializeField] private ObjectiveScriptableObject Objective;



        private int threatPoints = 0;
        private int currentThreatState = 0;

        public void IncreasePoints(int amount)
        {
            threatPoints += amount;

            int newThreshold = GetCurrentThreshold();

            OnThreatPointIncreased?.Invoke(threatPoints, MaxPoint);

            foreach (BaseObjectiveChannel channel in threatPointChannel)
            {
                channel.Raise(amount);
            }

            if (newThreshold > currentThreatState)
            {
                currentThreatState = newThreshold;

                OnCurrentThreatStateChanged?.Invoke(currentThreatState);
            }

            if (currentThreatState > 0)
            {
                ObjectiveManager.Instance.AddObjective(Objective.Objective);
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