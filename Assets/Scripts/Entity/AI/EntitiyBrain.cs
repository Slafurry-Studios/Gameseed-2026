using System.Collections.Generic;
using UnityEngine;

namespace Game.AI 
{
    [RequireComponent(typeof(NPCMovement))]
    public class EntityBrain : MonoBehaviour
    {
        [Header("Universal References")]
        public Transform Target; 
        
        [Header("State Priority List (Top = Highest)")]
        [Tooltip("The brain checks this list from top to bottom. The first state whose conditions are met will run!")]
        public List<EntityState> stateList = new List<EntityState>();

        public NPCMovement Movement { get; private set; }
        private EntityState currentState;

        private void Awake()
        {
            Movement = GetComponent<NPCMovement>();
        }

        private void Update()
        {
            EvaluateStates();
            
            if (currentState != null) 
            {
                currentState.UpdateState(this);
            }
        }

        private void EvaluateStates()
        {
            // 1. Check the list from top to bottom
            foreach (EntityState state in stateList)
            {
                // 2. Ask the state if its conditions are met (e.g., "Is the snek close?")
                if (state.CheckConditions(this))
                {
                    // 3. If it wants to run, and it isn't already running, switch to it!
                    if (currentState != state)
                    {
                        ChangeState(state);
                    }
                    
                    // Stop checking the rest of the list so lower-priority states don't override it
                    return; 
                }
            }
        }

        private void ChangeState(EntityState newState)
        {
            if (currentState != null) currentState.ExitState(this);
            currentState = newState;
            if (currentState != null) currentState.EnterState(this);
        }
    }
}