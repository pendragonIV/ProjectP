using System;
using UnityEngine;

namespace Things.Game.Models
{
    public class InventoryModel
    {
        public int MaxCapacity { get; private set; }
        public int CurrentCapacity { get; private set; }
        public bool IsFull => CurrentCapacity >= MaxCapacity;
        public int FreeSpace => MaxCapacity - CurrentCapacity;

        public event Action OnCapacityChanged;
        public event Action OnFull;

        public void SetMaxCapacity(int capacity)
        {
            MaxCapacity = capacity;
            OnCapacityChanged?.Invoke();
        }

        public bool CanAdd(int amount) => CurrentCapacity + amount <= MaxCapacity;

        public int ClampToAvailable(int requestedAmount) 
            => Mathf.Min(requestedAmount, FreeSpace);

        public void Add(int amount)
        {
            CurrentCapacity += amount;
            OnCapacityChanged?.Invoke();
            if (IsFull) OnFull?.Invoke();
        }

        public void Remove(int amount)
        {
            CurrentCapacity = Mathf.Max(0, CurrentCapacity - amount);
            OnCapacityChanged?.Invoke();
        }
    }
}
