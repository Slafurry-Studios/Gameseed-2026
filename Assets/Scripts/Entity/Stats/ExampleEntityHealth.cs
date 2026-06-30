using UnityEngine;
using Game.Generic;

public class ExampleEntityHealth : Health
{
    protected override void Die()
    {
        base.Die();
        Debug.Log("[ExampleEntityHealth] Entity has died!");
    }
}