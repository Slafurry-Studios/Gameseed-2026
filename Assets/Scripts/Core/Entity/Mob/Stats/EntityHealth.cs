using Game.AI;
using Game.Generic;
using UnityEngine;

public class EntityHealth : Health
{
    [SerializeField] private BaseObjectiveChannel[] destroyChannel;

    private EntityBrain entityBrain;
    [SerializeField] private string dieAnim;
    [SerializeField] private string hitAnim;

    protected override void Awake()
    {
        base.Awake();
        entityBrain = GetComponent<EntityBrain>();
    }

    public override void TakeDamage(float amount)
    {
        // Hit bullet selalu dihitung 1 HP
        base.TakeDamage(1f);

        if (!isDead && entityBrain != null && entityBrain.aiAnimation != null && !string.IsNullOrEmpty(hitAnim))
        {
            entityBrain.aiAnimation.SetTrigger(hitAnim);
        }
    }

    protected override void Die()
    {
        base.Die();

        Debug.Log("[ExampleEntityHealth] Entity has died!");

        foreach (BaseObjectiveChannel channel in destroyChannel)
        {
            channel.Raise(1);
        }

        if (entityBrain != null)
        {
            if (entityBrain.aiAnimation != null && !string.IsNullOrEmpty(dieAnim))
            {
                entityBrain.aiAnimation.Play(dieAnim, 0, 0f);
            }

            // Memastikan entitas diam dan tidak bisa menembak saat mati
            if (entityBrain.Movement != null)
            {
                entityBrain.Movement.SetMovement(Vector2.zero, 0f);
                entityBrain.Movement.enabled = false;
            }



            entityBrain.enabled = false;
        }
    }
}