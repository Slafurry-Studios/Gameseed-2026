using UnityEngine;

[System.Serializable]
public struct Objective
{
    public string DisplayName;
    public BaseObjectiveChannel Channel;
    public float ObjectiveThreshold;
    public ObjectiveType ObjectiveType;
    public bool IsMainMission;

    public override bool Equals(object obj)
    {
        if (obj is not Objective other) return false;
        return DisplayName == other.DisplayName && Channel == other.Channel;
    }

    public override int GetHashCode()
    {
        return System.HashCode.Combine(DisplayName, Channel);
    }
}