using UnityEngine;
using System.Collections.Generic;

namespace Things.Game.Configs.Craft
{
    [CreateAssetMenu(menuName = "Things/Craft/Recipe")]
    public class CraftRecipeSO : ScriptableObject
    {
        public string RecipeId;
        public string ResultCurrencyId;
        public int ResultAmount;
        
        [System.Serializable]
        public struct Ingredient
        {
            public string CurrencyId;
            public int Amount;
        }
        
        public List<Ingredient> Ingredients;
    }
}
