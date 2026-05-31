using System;
using UnityEngine;

namespace Things.Game.Models
{
    public class EnergyModel
    {
        public float MaxEnergy { get; private set; }
        public float CurrentEnergy { get; private set; }
        public bool IsDepleted => CurrentEnergy <= 0f;
        public bool IsFull => CurrentEnergy >= MaxEnergy;

        public float LowEnergySpeedMultiplier { get; set; } = 1f;
        public float LowEnergyHarvestMultiplier { get; set; } = 1f;

        public event Action OnChanged;
        public event Action OnDepleted;

        public void Initialize(float maxEnergy)
        {
            MaxEnergy = maxEnergy;
            CurrentEnergy = maxEnergy;
        }

        public void Restore(float amount)
        {
            CurrentEnergy = Mathf.Min(MaxEnergy, CurrentEnergy + amount);
            OnChanged?.Invoke();
        }

        public void Consume(float amount)
        {
            CurrentEnergy = Mathf.Max(0, CurrentEnergy - amount);
            OnChanged?.Invoke();
            if (IsDepleted) OnDepleted?.Invoke();
        }
    }
}
