using Things.Game.Configs.Currency;
using Things.Game.Models;
using Things.Game.Events;
using Things.Core.Infrastructure;
using System.Collections.Generic;

namespace Things.Game.Services.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly CurrencyDatabase database;
        private readonly Dictionary<string, CurrencyModel> runtimeModels;

        public CurrencyService(CurrencyDatabase db)
        {
            database = db;
            runtimeModels = new Dictionary<string, CurrencyModel>();
            
            // Khởi tạo model dựa trên database
            foreach (var def in database.All)
            {
                var model = new CurrencyModel(def.CurrencyId, 0); // Có thể load từ ISaveService
                model.OnAmountChanged += (amount) => OnModelAmountChanged(def.CurrencyId, amount);
                runtimeModels[def.CurrencyId] = model;
            }
        }

        private void OnModelAmountChanged(string currencyId, int newAmount)
        {
            EventBus.Publish(new CurrencyChangedEvent { CurrencyId = currencyId, NewAmount = newAmount });
        }

        public int GetAmount(string currencyId) => runtimeModels.ContainsKey(currencyId) ? runtimeModels[currencyId].Amount : 0;
        
        public void Add(string currencyId, int amount)
        {
            if (runtimeModels.TryGetValue(currencyId, out var model)) model.Add(amount);
        }

        public void Subtract(string currencyId, int amount)
        {
            if (runtimeModels.TryGetValue(currencyId, out var model)) model.Subtract(amount);
        }

        public void Set(string currencyId, int amount)
        {
            if (runtimeModels.TryGetValue(currencyId, out var model)) model.Set(amount);
        }

        public CurrencyDefinition GetDefinition(string currencyId) => database.Exists(currencyId) ? database.GetById(currencyId) : null;
        public IReadOnlyList<CurrencyDefinition> GetAllDefinitions() => database.All;
        public bool HasEnough(string currencyId, int amount) => GetAmount(currencyId) >= amount;
    }
}
