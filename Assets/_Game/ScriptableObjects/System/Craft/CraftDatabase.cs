using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Things.Game.Configs.Craft
{
    [CreateAssetMenu(menuName = "Things/Craft/Database")]
    public class CraftDatabase : ScriptableObject
    {
        public List<CraftRecipeSO> Recipes = new List<CraftRecipeSO>();
        private Dictionary<string, CraftRecipeSO> lookup;

        public void Initialize()
        {
            lookup = new Dictionary<string, CraftRecipeSO>();
            foreach (var r in Recipes)
            {
                if (r != null && !string.IsNullOrEmpty(r.RecipeId))
                {
                    lookup[r.RecipeId] = r;
                }
            }
        }

        public CraftRecipeSO GetById(string id)
        {
            if (lookup == null) Initialize();
            return lookup != null && lookup.TryGetValue(id, out var def) ? def : null;
        }
    }
}
