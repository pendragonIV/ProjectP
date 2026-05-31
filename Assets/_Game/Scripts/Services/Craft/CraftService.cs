using Things.Core.Infrastructure;
using Things.Game.Configs.Craft;
using Things.Game.Services.Currency;
using UnityEngine;

namespace Things.Game.Services.Craft
{
    public class CraftService : ICraftService
    {
        private readonly CraftDatabase database;
        private ICurrencyService currencyService;

        public CraftService(CraftDatabase database)
        {
            this.database = database;
        }

        private ICurrencyService CurrencyService
        {
            get
            {
                if (currencyService == null)
                    ServiceLocator.TryGet(out currencyService);
                return currencyService;
            }
        }

        public bool CanCraft(string recipeId)
        {
            var recipe = database?.GetById(recipeId);
            if (recipe == null) return false;

            var currSvc = CurrencyService;
            if (currSvc == null) return false;

            foreach (var ingredient in recipe.Ingredients)
            {
                if (!currSvc.HasEnough(ingredient.CurrencyId, ingredient.Amount))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Craft(string recipeId)
        {
            if (!CanCraft(recipeId)) return false;

            var recipe = database.GetById(recipeId);
            var currSvc = CurrencyService;

            // Deduct all ingredients
            foreach (var ingredient in recipe.Ingredients)
            {
                currSvc.Subtract(ingredient.CurrencyId, ingredient.Amount);
            }

            // Add result item
            currSvc.Add(recipe.ResultCurrencyId, recipe.ResultAmount);
            
            Debug.Log($"[CraftService] Crafted {recipe.ResultAmount}x {recipe.ResultCurrencyId} from {recipeId}");
            return true;
        }
    }
}
