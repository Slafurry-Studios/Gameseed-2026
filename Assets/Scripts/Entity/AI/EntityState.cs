using UnityEngine;

namespace Game.AI
{
    public abstract class EntityState : MonoBehaviour
    {
        // The Brain asks the state: "Should you be running right now?"
        public abstract bool CheckConditions(EntityBrain brain);

        public abstract void EnterState(EntityBrain brain);
        public abstract void UpdateState(EntityBrain brain);
        public abstract void ExitState(EntityBrain brain);
    }
}