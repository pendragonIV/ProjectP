using Things.Core.Infrastructure;
using Things.Game.Configs.Currency;
using System.Collections.Generic;

namespace Things.Game.Services.Currency
{
    public interface ICurrencyService : IService
    {
        int GetAmount(string currencyId);
        void Add(string currencyId, int amount);
        void Subtract(string currencyId, int amount);
        void Set(string currencyId, int amount);
        CurrencyDefinition GetDefinition(string currencyId);
        IReadOnlyList<CurrencyDefinition> GetAllDefinitions();
        bool HasEnough(string currencyId, int amount);
    }
}
