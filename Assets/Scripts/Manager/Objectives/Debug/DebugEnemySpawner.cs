using UnityEngine;

public class DebugEnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyDeathChannel deathChannel;
    [SerializeField] private GameObject enemyVisualPrefab; // bisa cube/sphere primitif
    [SerializeField] private int spawnCount = 5;
    [SerializeField] private float spawnRadius = 20f;

    private void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0f,
                Random.Range(-spawnRadius, spawnRadius)
            );

            var go = Instantiate(enemyVisualPrefab, pos, Quaternion.identity);
            go.name = $"DebugEnemy_{i}";

            var target = go.AddComponent<ObjectiveTarget>();
            SetChannelViaReflection(target, deathChannel);

            var debugDeath = go.AddComponent<DebugKillable>();
            debugDeath.Setup(deathChannel);
        }
    }

    // ObjectiveTarget field-nya private [SerializeField], jadi kalau mau set runtime
    // gampangnya bikin public setter di ObjectiveTarget (lihat update di bawah)
    private void SetChannelViaReflection(ObjectiveTarget target, BaseObjectiveChannel channel)
    {
        target.SetChannel(channel);
    }
}