// DebugKillable.cs
using UnityEngine;

public class DebugKillable : MonoBehaviour
{
    private EnemyDeathChannel deathChannel;

    public void Setup(EnemyDeathChannel channel)
    {
        deathChannel = channel;
    }

    private void OnMouseDown()
    {
        Die();
    }

    private void Die()
    {
        deathChannel.Raise(1f);
        Destroy(gameObject);
    }
}