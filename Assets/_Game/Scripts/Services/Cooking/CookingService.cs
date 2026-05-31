using Things.Core.Infrastructure;
using Things.Game.Configs.Cooking;
using Things.Game.Services.Currency;
using UnityEngine;

namespace Things.Game.Services.Cooking
{
    public class CookingService : ICookingService
    {
        private readonly CookingDatabase database;
        private ICurrencyService currencyService;

        public CookingService(CookingDatabase database)
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

        public bool CanCook(string recipeId)
        {
            var recipe = database?.GetById(recipeId);
            if (recipe == null) return false;

            var currSvc = CurrencyService;
            if (currSvc == null) return false;

            foreach (var ingredient in recipe.Ingredients)
            {
                if (!currSvc.HasEnough(ingredient.ItemId, ingredient.Amount))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Cook(string recipeId)
        {
            if (!CanCook(recipeId)) return false;

            var recipe = database.GetById(recipeId);
            var currSvc = CurrencyService;

            // Deduct ingredients
            foreach (var ingredient in recipe.Ingredients)
            {
                currSvc.Subtract(ingredient.ItemId, ingredient.Amount);
            }

            // Add result food item (For MVP, instant cook)
            currSvc.Add(recipe.ResultFoodId, 1);
            
            Debug.Log($"[CookingService] Cooked 1x {recipe.ResultFoodId} from {recipeId}");
            return true;
        }
    }
}
