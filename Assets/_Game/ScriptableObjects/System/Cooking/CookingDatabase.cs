using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Things.Game.Configs.Cooking
{
    [CreateAssetMenu(menuName = "Things/Cooking/Database")]
    public class CookingDatabase : ScriptableObject
    {
        public List<RecipeSO> Recipes = new List<RecipeSO>();
        private Dictionary<string, RecipeSO> lookup;

        public void Initialize()
        {
            lookup = new Dictionary<string, RecipeSO>();
            foreach (var r in Recipes)
            {
                if (r != null && !string.IsNullOrEmpty(r.RecipeId))
                {
                    lookup[r.RecipeId] = r;
                }
            }
        }

        public RecipeSO GetById(string id)
        {
            if (lookup == null) Initialize();
            return lookup != null && lookup.TryGetValue(id, out var def) ? def : null;
        }
    }
}
