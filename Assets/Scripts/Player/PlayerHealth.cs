using System;
using UnityEngine;

namespace Game.Player
{
    public class PlayerHealth : MonoBehaviour
    {

        public event Action<float> OnHealthPctChanged;

        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100f;
        
        private float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        private void Start()
        {
           
            OnHealthPctChanged?.Invoke(GetHealthNormalized());
        }

        
        public void TakeDamage(float amount)
        {
            
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            
            OnHealthPctChanged?.Invoke(GetHealthNormalized());

            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        
        public void Heal(float amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            OnHealthPctChanged?.Invoke(GetHealthNormalized());
        }

        
        public float GetHealthNormalized()
        {
            return currentHealth / maxHealth;
        }

        private void Die()
        {
            
            Debug.Log("Player has died!");
        }
    }
}