using System;
using UnityEngine;

public class WantedManager : MonoBehaviour
{
    public static WantedManager Instance { get; private set; }

    [SerializeField] private WantedLevel[] wantedLevels;

    private int threatPoints = 0;
    private int currentWantedLevel = 0;

    // Other objects can subscribe to this event
    public event Action<WantedLevel> OnWantedLevelIncreased;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseThreatPoints(int amount)
    {
        threatPoints += amount;

        int newWantedLevel = GetCurrentWantedLevel();

        if (newWantedLevel > currentWantedLevel)
        {
            currentWantedLevel = newWantedLevel;

            WantedLevel levelData = wantedLevels[newWantedLevel];

            Debug.Log($"[WantedManager] Wanted Level Increased: {levelData.Name}");

            // Notify all listeners
            OnWantedLevelIncreased?.Invoke(levelData);

            // You can listen using WantedManager.Instance.OnWantedLevelIncreased += YourMethod;
            // Don't forget to unsubscribe when not needed to avoid memory leaks.
            // Example: WantedManager.Instance.OnWantedLevelIncreased -= YourMethod;
        }

        Debug.Log($"[WantedManager] Threat Points: {threatPoints}");
    }

    private int GetCurrentWantedLevel()
    {
        int level = 0;

        for (int i = 0; i < wantedLevels.Length; i++)
        {
            if (threatPoints >= wantedLevels[i].Threshold)
            {
                level = i;
            }
        }

        return level;
    }

    public int GetThreatPoints()
    {
        return threatPoints;
    }
}

[System.Serializable]
public struct WantedLevel
{
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int Threshold { get; private set; }

    public WantedLevel(string name, int level, int threshold)
    {
        Name = name;
        Level = level;
        Threshold = threshold;
    }
}