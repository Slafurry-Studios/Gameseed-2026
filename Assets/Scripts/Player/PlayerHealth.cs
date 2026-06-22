using System;
using UnityEngine;
using Game.Generic;

namespace Game.Player
{
    public class PlayerHealth : Health
    {
        protected override void Die()
        {
            base.Die();
            Debug.Log("[PlayerHealth]Player has died!");
            // Additional logic for player death can be added here, such as triggering a respawn or game over sequence.
        }
    }
}