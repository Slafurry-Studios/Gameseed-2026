using UnityEngine;

namespace Game.AI
{
    public class PanicState : EntityState
    {
        [Header("Panic Thresholds")]
        [Tooltip("How close the snek must be to trigger panic")]
        public float panicRadius = 5f; 
        
        [Tooltip("How far away the snek must be before the NPC feels safe again")]
        public float safeRadius = 8f;  
        
        [Header("Movement")]
        public float runSpeed = 5f;

        // We use this to remember what we are currently doing
        private bool isPanicking = false; 

        public override bool CheckConditions(EntityBrain brain)
        {
            if (brain.Target == null) return false;
            
            float distance = Vector2.Distance(transform.position, brain.Target.position);

            // If we are ALREADY running for our lives, use the larger "safe" radius to decide if we should stop
            if (isPanicking)
            {
                return distance <= safeRadius;
            }
            // If we are just wandering normally, use the smaller "panic" radius to detect danger
            else
            {
                return distance <= panicRadius;
            }
        }

        public override void EnterState(EntityBrain brain) 
        { 
            // The moment the brain switches to this state, remember that we are panicking
            isPanicking = true;
        }

        public override void UpdateState(EntityBrain brain)
        {
            Vector2 runDirection = ((Vector2)transform.position - (Vector2)brain.Target.position).normalized;
            brain.Movement.SetMovement(runDirection, runSpeed);
        }

        public override void ExitState(EntityBrain brain)
        {
            // The brain has switched to a different state (like Wander), so we are no longer panicking
            isPanicking = false;
            brain.Movement.SetMovement(Vector2.zero, 0f);
        }
    }
}