using UnityEngine;
using System.Collections.Generic;

namespace Things.Game.Configs.Cooking
{
    [CreateAssetMenu(menuName = "Things/Cooking/Recipe")]
    public class RecipeSO : ScriptableObject
    {
        public string RecipeId;
        public string ResultFoodId;
        
        [System.Serializable]
        public struct Ingredient
        {
            public string ItemId;
            public int Amount;
        }
        
        public List<Ingredient> Ingredients;
        public float CookingTime;
    }
}
