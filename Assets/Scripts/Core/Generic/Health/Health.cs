using System;
using UnityEngine;
using Game.Core;

namespace Game.Generic
{
    public class Health : MonoBehaviour, IDamageable
    {
        public event Action<float> OnHealthPctChanged;
        public event Action<float> OnDamaged;
        public event Action<float> OnHealed;
        public event Action OnDied;

        [Header("Health Settings")]
        [SerializeField] protected float maxHealth = 100f;

        protected float currentHealth;
        protected bool isDead;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsDead => isDead;

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }

        protected virtual void Start()
        {
            OnHealthPctChanged?.Invoke(GetHealthNormalized());
        }

        public virtual void TakeDamage(float amount)
        {
            if (isDead || amount <= 0f)
                return;

            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            OnDamaged?.Invoke(amount);
            OnHealthPctChanged?.Invoke(GetHealthNormalized());

            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        public virtual void Heal(float amount)
        {
            if (isDead || amount <= 0f)
                return;

            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            OnHealed?.Invoke(amount);
            OnHealthPctChanged?.Invoke(GetHealthNormalized());
        }

        public virtual void SetHealth(float value)
        {
            currentHealth = Mathf.Clamp(value, 0f, maxHealth);

            OnHealthPctChanged?.Invoke(GetHealthNormalized());

            if (currentHealth <= 0f && !isDead)
            {
                Die();
            }
        }

        public float GetHealthNormalized()
        {
            return currentHealth / maxHealth;
        }

        protected virtual void Die()
        {
            if (isDead)
                return;

            isDead = true;

            OnDied?.Invoke();

            Debug.Log($"{gameObject.name} died.");
        }
    }
}