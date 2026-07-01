using UnityEngine;
using UnityEngine.Events;

namespace Game.Player
{
    public class PlayerCollection : MonoBehaviour
    {
        [Header("Collection Stats")]
        [SerializeField] private int orbsCollected = 0;
        [SerializeField] private int threatPoints = 0;
        [SerializeField] private int techPoints = 0;

        [Header("Events")]
        public UnityEvent<int> onOrbCollected;
        public UnityEvent<int> onThreatPointGained;
        public UnityEvent<int> onTechPointCollected;


        public void AddOrbs(int amount)
        {
            orbsCollected += amount;
            onOrbCollected?.Invoke(orbsCollected);
            Debug.Log($"Collected Orb! Total: {orbsCollected}");
        }

        public void AddThreatPoints(int amount)
        {
            threatPoints += amount;
            onThreatPointGained?.Invoke(threatPoints);
            Debug.Log($"Threat Increased! Total Threat: {threatPoints}");
        }

        public void AddTechPoints(int amount)
        {
            techPoints += amount;
            onTechPointCollected?.Invoke(techPoints);
            Debug.Log($"Collected Tech Point! Total: {techPoints}");
        }

        public int GetOrbs() => orbsCollected;
        public int GetThreatPoints() => threatPoints;
        public int GetTechPoints() => techPoints;
    }
}
