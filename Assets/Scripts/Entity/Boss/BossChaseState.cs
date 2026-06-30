using UnityEngine;
using Game.Player;

namespace Game.AI.Boss
{
    public class BossChaseState : EntityState
    {
        [Header("Chase Configuration")]
        [Tooltip("Maximum distance from target (in units) at which the boss detects and starts chasing the player. Default: 25 units.")]
        public float detectionRadius = 25f;

        [Tooltip("Preferred standoff distance (in units) to maintain between the boss and the target. Default: 8 units.")]
        public float maintainDistance = 8f;

        [Tooltip("Speed multiplier relative to the player's current speed. For example, 1.2 moves 20% faster than the player. Default: 1.2x.")]
        public float movementSpeedMultiplier = 1.2f;

        public override bool CheckConditions(EntityBrain brain)
        {
            BossHealth health = brain.GetComponent<BossHealth>();
            if (health != null && health.IsDead) return false;

            if (brain.Target == null) return false;

            float distance = Vector2.Distance(transform.position, brain.Target.position);
            return distance <= detectionRadius;
        }

        public override void EnterState(EntityBrain brain)
        {
        }

        public override void UpdateState(EntityBrain brain)
        {
            if (brain.Target == null || brain.Movement == null) return;

            PlayerMovement player = brain.Target.GetComponent<PlayerMovement>();
            float playerSpeed = (player != null) ? player.CurrentSpeed : 5f;
            float chaseSpeed = playerSpeed * movementSpeedMultiplier;

            float distance = Vector2.Distance(transform.position, brain.Target.position);
            Vector2 directionToTarget = (brain.Target.position - transform.position).normalized;

            if (distance > maintainDistance + 0.5f)
            {
                brain.Movement.SetMovement(directionToTarget, chaseSpeed);
            }
            else if (distance < maintainDistance - 0.5f)
            {
                brain.Movement.SetMovement(-directionToTarget, chaseSpeed);
            }
            else
            {
                brain.Movement.SetMovement(Vector2.zero, 0f);
            }
        }

        public override void ExitState(EntityBrain brain)
        {
            if (brain.Movement != null)
            {
                brain.Movement.SetMovement(Vector2.zero, 0f);
            }
        }
    }
}
