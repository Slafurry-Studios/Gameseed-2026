using Game.AI;
using Game.Core.Effects;
using Game.Generic;
using UnityEngine;

public class EntityHealth : Health
{
    [SerializeField] private BaseObjectiveChannel[] destroyChannel;
    [SerializeField] private ObjectiveScriptableObject objective;

    private EntityBrain entityBrain;
    [SerializeField] private string dieAnim;
    [SerializeField] private string hitAnim;
    [SerializeField] private string hitSound;
    [SerializeField] private string deathSound;
    [SerializeField] private StreamChatType deathChatType = StreamChatType.KILL_HOSTILES;
    private IVisualEffect[] visualEffects;

    protected override void Awake()
    {
        base.Awake();
        entityBrain = GetComponent<EntityBrain>();
        visualEffects = GetComponentsInChildren<IVisualEffect>();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        if (!isDead && entityBrain != null && entityBrain.aiAnimation != null && !string.IsNullOrEmpty(hitAnim))
        {
            entityBrain.aiAnimation.SetTrigger(hitAnim);
            SoundManager.Instance.PlaySound2D(hitSound);

            foreach (var effect in visualEffects)
            {
                effect.PlayEffect();
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySound2D(deathSound);

        Debug.Log("[ExampleEntityHealth] Entity has died!");

        foreach (BaseObjectiveChannel channel in destroyChannel)
        {
            channel.Raise(1);
        }

        ObjectiveManager.Instance.AddObjective(objective.Objective);
        StreamChatManager.Instance.HandleStreamChat(deathChatType, 5);
        if (entityBrain != null)
        {
            if (entityBrain.aiAnimation != null && !string.IsNullOrEmpty(dieAnim))
            {
                entityBrain.aiAnimation.Play(dieAnim, 0, 0f);
            }

            if (entityBrain.Movement != null)
            {
                entityBrain.Movement.SetMovement(Vector2.zero, 0f);
                entityBrain.Movement.enabled = false;
            }


            entityBrain.enabled = false;
            GetComponent<Collider2D>().isTrigger = true;
        }
    }
}