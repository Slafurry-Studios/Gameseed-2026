// DebugObjectiveTrigger.cs
using UnityEngine;
using Game.UI.HUD;
public class DebugObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private EnemyDeathChannel enemyDeathChannel;
    [SerializeField] private KeyCode addObjectiveKey = KeyCode.Q;
    [SerializeField] private KeyCode manualRaiseKey = KeyCode.W;

    private void Update()
    {
        if (Input.GetKeyDown(addObjectiveKey))
        {
            AddTestObjective();
            Debug.Log("Q");
        }

        if (Input.GetKeyDown(manualRaiseKey))
        {
            enemyDeathChannel.Raise(1f);
            Debug.Log("Manual raise: +1 progress ke EnemyDeathChannel");
        }
    }

    private void AddTestObjective()
    {
        var objective = new Objective
        {
            DisplayName = "Kill 5 Enemies (Debug)",
            Channel = enemyDeathChannel,
            ObjectiveThreshold = 5f,
            ObjectiveType = ObjectiveType.KILL
        };

        ObjectiveManager.Instance.AddObjective(objective);
        // PlayerObjectiveHUD.Instance.AddObjectiveItem(objective);

        Debug.Log($"Objective '{objective.DisplayName}' ditambahkan.");
    }
}