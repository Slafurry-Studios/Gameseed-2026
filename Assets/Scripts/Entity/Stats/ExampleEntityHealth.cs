using Game.AI;
using Game.Generic;
using UnityEngine;

public class ExampleEntityHealth : Health
{
    private EntityBrain entityBrain;
    [SerializeField] private string dieAnim;

    protected override void Awake()
    {
        base.Awake();
        entityBrain = GetComponent<EntityBrain>();
    }

    protected override void Die()
    {
        base.Die();

        Debug.Log("[ExampleEntityHealth] Entity has died!");

        if (entityBrain != null &&
            entityBrain.aiAnimation != null &&
            !string.IsNullOrEmpty(dieAnim))
        {
            entityBrain.aiAnimation.Play(dieAnim, 0, 0f);
        }
    }
}