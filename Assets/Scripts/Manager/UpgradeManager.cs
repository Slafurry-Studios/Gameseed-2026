using System;
using UnityEngine;
public class UpgradeManager : Singleton<UpgradeManager>
{
    private int upgradePoints = 0;
    public event Action<int> OnUpgradePointsChanged;

    // Method to increase upgrade points
    public void IncreaseUpgradePoints(int amount)
    {
        upgradePoints += amount;
        OnUpgradePointsChanged?.Invoke(upgradePoints);
        // Listeners can subscribe to this event to update UI or other systems when upgrade points change.
        // USE: UpgradeManager.Instance.OnUpgradePointsChanged += YourMethod;
        Debug.Log($"[UpgradeManager] Upgrade Points: {upgradePoints}");
    }


    // Call this method when an upgrade is purchased
    public bool SpendUpgradePoints(int amount)
    {
        if (upgradePoints >= amount)
        {
            upgradePoints -= amount;
            OnUpgradePointsChanged?.Invoke(upgradePoints);
            Debug.Log($"[UpgradeManager] Upgrade Points Spent: {amount}. Remaining: {upgradePoints}");
            return true;
        }
        else
        {
            Debug.LogWarning($"[UpgradeManager] Not enough upgrade points. Required: {amount}, Available: {upgradePoints}");
            return false;
        }
    }


    // Idk, maybe need this for UI or other systems to check current upgrade points
    public int GetUpgradePoints()
    {
        return upgradePoints;
    }

}