using System;

namespace Things.Game.Models
{
    public class CurrencyModel
    {
        public string CurrencyId { get; private set; }
        public int Amount { get; private set; }

        public event Action<int> OnAmountChanged;

        public CurrencyModel(string currencyId, int initialAmount)
        {
            CurrencyId = currencyId;
            Amount = initialAmount;
        }

        public void Add(int amount)
        {
            Amount += amount;
            OnAmountChanged?.Invoke(Amount);
        }

        public void Subtract(int amount)
        {
            Amount = Math.Max(0, Amount - amount);
            OnAmountChanged?.Invoke(Amount);
        }

        public void Set(int amount)
        {
            Amount = Math.Max(0, amount);
            OnAmountChanged?.Invoke(Amount);
        }
    }
}
