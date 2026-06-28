using UnityEngine;

public abstract class BaseObjectiveChannel : ScriptableObject
{
    public event System.Action<float> OnRaised;

    public void Raise(float amount)
    {
        OnRaised?.Invoke(amount);
    }
}