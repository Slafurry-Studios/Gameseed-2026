using UnityEngine;

namespace Game.AI
{
    public abstract class EntityState : MonoBehaviour
    {

        public abstract bool CheckConditions(EntityBrain brain);

        public abstract void EnterState(EntityBrain brain);
        public abstract void UpdateState(EntityBrain brain);
        public abstract void ExitState(EntityBrain brain);
    }
}