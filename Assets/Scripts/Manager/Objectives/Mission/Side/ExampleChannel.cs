using UnityEngine;

[CreateAssetMenu(menuName = "Channels/EnemyDeathChannel")]
public class EnemyDeathChannel : BaseObjectiveChannel 
{
    public override void OnCompleted()
    {
        // Implementation for when enemy is killed
    }
}