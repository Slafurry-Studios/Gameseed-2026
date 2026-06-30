using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    /// <summary>
    /// Core AI State Machine Controller. Evaluates attached state scripts in priority order
    /// every frame and runs the highest-priority state whose conditions are met.
    /// </summary>
    [RequireComponent(typeof(NPCMovement))]
    public class EntityBrain : MonoBehaviour
    {
        [Header("Universal References")]
        public Transform Target;
        public Animator aiAnimation;

        [Header("State Priority List (Top = Highest)")]
        [Tooltip("List of attached EntityState scripts ordered by priority. Every frame, the brain checks from index 0 downward. The FIRST state whose CheckConditions returns true will be activated!")]
        public List<EntityState> stateList = new List<EntityState>();

        public NPCMovement Movement { get; private set; }
        private EntityState currentState;

        private void Awake()
        {
            Movement = GetComponent<NPCMovement>();
            if (aiAnimation == null) aiAnimation = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            EvaluateStates();

            if (currentState != null)
            {
                currentState.UpdateState(this);
            }
        }

        private void Start() // Move logic here
        {
            if (Target == null && PlayerManager.PlayerTransform != null)
            {
                Target = PlayerManager.PlayerTransform;
            }

            // Safety fallback if PlayerManager wasn't ready
            if (Target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) Target = player.transform;
            }
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
            foreach (EntityState state in stateList)
            {
                if (state != null && state.CheckConditions(this))
                {
                    if (currentState != state)
                    {
                        ChangeState(state);
                    }
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