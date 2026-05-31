using UnityEngine;
using Things.Game.Models;
using Things.Game.Configs.Building;
using Things.Core.Infrastructure;
using Things.Game.Services.Currency;

namespace Things.Game.Building
{
    public class PurchasePresenter : MonoBehaviour
    {
        private BuildingStateModel model;
        private BuildingDefinition def;
        private ICurrencyService currencyService;

        public void Initialize(BuildingStateModel model, BuildingDefinition def)
        {
            this.model = model;
            this.def = def;
            this.currencyService = ServiceLocator.Get<ICurrencyService>();
        }

        // Gọi khi Player đứng vào trigger (sẽ nhận event từ TriggerBox)
        public void OnPlayerInteraction()
        {
            if (model.IsPurchased) return;

            bool canAfford = true;
            foreach (var cost in def.RequiredResources)
            {
                if (!currencyService.HasEnough(cost.CurrencyId, cost.Amount))
                {
                    canAfford = false;
                    break;
                }
            }

            if (canAfford)
            {
                foreach (var cost in def.RequiredResources)
                {
                    currencyService.Subtract(cost.CurrencyId, cost.Amount);
                }
                model.SetPurchased(def.RequiredHitsToConstruct);
            }
        }
    }
}
