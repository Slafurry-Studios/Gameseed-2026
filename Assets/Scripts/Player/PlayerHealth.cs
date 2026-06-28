using System;
using UnityEngine;
using Game.Generic;

namespace Game.Player
{
    public class PlayerHealth : Health
    {
        [Header("Death Settings")]
        [SerializeField] private Animator playerDizzyAnimator;
        [SerializeField] private PlayerMovement playerMovement;

        protected override void Awake()
        {
            base.Awake();
            if (playerMovement == null)
            {
                playerMovement = GetComponent<PlayerMovement>();
            }
        }

        protected override void Die()
        {
            currentHealth = 0f;
            base.Die();
            
            if (playerDizzyAnimator != null)
            {
                playerDizzyAnimator.gameObject.SetActive(true);
                playerDizzyAnimator.SetTrigger("Collide");
            }

            // Hentikan movement & set stamina ke 0
            if (playerMovement != null)
            {
                playerMovement.SetStamina(0f);
                playerMovement.enabled = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckBuilding(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckBuilding(other.gameObject);
        }

        private void CheckBuilding(GameObject obj)
        {
            if (isDead)
                return;

            if (obj.CompareTag("Building") || obj.CompareTag("building") || obj.name.ToLower().Contains("building"))
            {
                Die();
            }
        }
        public void ResetPlayer()
        {
            isDead = false; 
            
            currentHealth = 100f; 

            if (playerDizzyAnimator != null)
            {
                playerDizzyAnimator.gameObject.SetActive(false);
            }

            if (playerMovement != null)
            {
                playerMovement.enabled = true;
                playerMovement.SetStamina(100f);
            }

            Debug.Log("[PlayerHealth] Player has been reset and respawned!");
        }
    }
}