using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Things.Game.Configs.Currency
{
    [CreateAssetMenu(menuName = "Things/Currency/Database")]
    public class CurrencyDatabase : ScriptableObject
    {
        public List<CurrencyDefinition> Currencies = new List<CurrencyDefinition>();

        private Dictionary<string, CurrencyDefinition> lookup;

        public IReadOnlyList<CurrencyDefinition> All => Currencies;

        public bool Exists(string currencyId) => lookup != null && lookup.ContainsKey(currencyId);

        public CurrencyDefinition GetById(string currencyId) => lookup != null && lookup.TryGetValue(currencyId, out var def) ? def : null;

        public void Initialize()
        {
            lookup = new Dictionary<string, CurrencyDefinition>();
            foreach (var c in Currencies)
            {
                if (c != null && !string.IsNullOrEmpty(c.CurrencyId))
                {
                    lookup[c.CurrencyId] = c;
                }
            }
        }
    }
}
