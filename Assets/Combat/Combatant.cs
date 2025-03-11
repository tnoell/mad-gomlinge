using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Combat
{
    public class Combatant : DamageSource
    {
        public delegate bool OnDeath(string killedBy);  // return true if cleanup was handled; if none returns true, object gets destroyed
        public OnDeath onDeath;

        public delegate void OnHealthChanged(float change);
        public OnHealthChanged onHealthChanged;

        [SerializeField] private float maxHealth = 100;

        private float currentHealth;

        private Combatant currentTarget;

        public float GetMaxHealth() { return maxHealth; }
        public void SetMaxHealth(float newMaxHealth)
        {
            currentHealth = GetHealthFraction() * newMaxHealth;
            maxHealth = newMaxHealth;
        }
        public float GetHealth() { return currentHealth; }
        public float GetHealthFraction() { return currentHealth / maxHealth; }

        // Start is called before the first frame update
        void Awake()
        {
            currentHealth = maxHealth;
            currentTarget = null;
        }

        public void SetTarget(Combatant target)
        {
            currentTarget = target;
            currentTarget.onDeath += (string killedBy) => {
                currentTarget = null;
                return false;
            };
        }

        public Combatant GetTarget()
        {
            return currentTarget;
        }

        public void Hit(float damageAmount, DamageSource damageSource)
        {
            DealDamage(damageAmount, damageSource.GetName());
        }

        public void DealDamage(float damageAmount, string source)
        {
            if (damageAmount < 0) damageAmount = 0;
            currentHealth -= damageAmount;
            onHealthChanged?.Invoke(-damageAmount);
            if (currentHealth <= 0)
            {
                Die(source);
            }
        }
        
        public bool Heal(float amount)
        {
            if (currentHealth >= maxHealth - 0.01f) return false;
            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            onHealthChanged?.Invoke(amount);
            return true;
        }

        public void SetHealth(float value)
        {
            currentHealth = value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            onHealthChanged?.Invoke(0);
        }

        private void Die(string source)
        {
            var results = onDeath?.GetInvocationList().Select(x => (bool)x.DynamicInvoke(source));

            bool handled = false;
            if (results != null)
            {
                foreach (bool result in results)
                {
                    if (result)
                    {
                        if (handled)
                        {
                            Debug.LogError("Death for Object " + gameObject + " was handled multiple times!");
                        }
                        handled = true;
                    }
                }
            }

            if (!handled)
            {
                Destroy(gameObject);
            }
        }
    }
}
