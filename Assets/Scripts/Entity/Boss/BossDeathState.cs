using UnityEngine;

namespace Game.AI.Boss
{
    public class BossDeathState : EntityState
    {
        [Header("Death Effects Setup")]
        [Tooltip("Optional particle effect prefab spawned when the boss dies (e.g. big explosion).")]
        public GameObject deathEffectPrefab;

        [Tooltip("Time delay (in seconds) before the boss GameObject is destroyed after dying. Default: 3 seconds.")]
        public float destroyDelay = 3f;

        private bool hasEnteredDeath = false;

        public override bool CheckConditions(EntityBrain brain)
        {
            BossHealth health = brain.GetComponent<BossHealth>();
            return health != null && health.IsDead;
        }

        public override void EnterState(EntityBrain brain)
        {
            if (hasEnteredDeath) return;
            hasEnteredDeath = true;

            Debug.Log($"[BossDeathState] Boss has died! Stopping all movement and combat.");

            if (brain.Movement != null)
            {
                brain.Movement.SetMovement(Vector2.zero, 0f);
            }

            Collider2D[] colliders = brain.GetComponents<Collider2D>();
            foreach (var col in colliders)
            {
                col.enabled = false;
            }

            if (deathEffectPrefab != null)
            {
                GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, destroyDelay + 1f);
            }

            Destroy(brain.gameObject, destroyDelay);
        }

        public override void UpdateState(EntityBrain brain)
        {
            if (brain.Movement != null)
            {
                brain.Movement.SetMovement(Vector2.zero, 0f);
            }
        }

        public override void ExitState(EntityBrain brain)
        {
        }
    }
}
