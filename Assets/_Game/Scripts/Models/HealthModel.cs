using System;
using UnityEngine;

namespace Things.Game.Models
{
    public class HealthModel
    {
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public bool IsDepleted => CurrentHealth <= 0f;
        public bool IsFull => CurrentHealth >= MaxHealth;
        public float Percentage => MaxHealth > 0 ? CurrentHealth / MaxHealth : 0f;

        public event Action<float> OnChanged;
        public event Action OnDepleted;
        public event Action OnRestored;

        public void Initialize(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public void Subtract(float amount)
        {
            float oldHealth = CurrentHealth;
            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
            OnChanged?.Invoke(CurrentHealth - oldHealth);
            if (CurrentHealth <= 0) OnDepleted?.Invoke();
        }

        public void Add(float amount)
        {
            float oldHealth = CurrentHealth;
            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
            OnChanged?.Invoke(CurrentHealth - oldHealth);
            if (IsFull) OnRestored?.Invoke();
        }

        public void Restore()
        {
            float oldHealth = CurrentHealth;
            CurrentHealth = MaxHealth;
            OnChanged?.Invoke(CurrentHealth - oldHealth);
            OnRestored?.Invoke();
        }
    }
}
