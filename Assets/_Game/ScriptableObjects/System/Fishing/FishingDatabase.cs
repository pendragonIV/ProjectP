using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Things.Game.Configs.Fishing
{
    [CreateAssetMenu(menuName = "Things/Fishing/Database")]
    public class FishingDatabase : ScriptableObject
    {
        public List<FishDataSO> Fishes = new List<FishDataSO>();
        private Dictionary<string, FishDataSO> lookup;

        public void Initialize()
        {
            lookup = new Dictionary<string, FishDataSO>();
            foreach (var f in Fishes)
            {
                if (f != null && !string.IsNullOrEmpty(f.FishId))
                {
                    lookup[f.FishId] = f;
                }
            }
        }

        public FishDataSO GetById(string id)
        {
            if (lookup == null) Initialize();
            return lookup != null && lookup.TryGetValue(id, out var def) ? def : null;
        }

        public FishDataSO GetRandomFish()
        {
            if (Fishes == null || Fishes.Count == 0) return null;
            
            // Simple RNG logic: sum rarities and pick one
            int totalRarity = Fishes.Sum(f => f.Rarity);
            int randomValue = Random.Range(0, totalRarity);
            
            int currentSum = 0;
            foreach (var f in Fishes)
            {
                currentSum += f.Rarity;
                if (randomValue <= currentSum)
                {
                    return f;
                }
            }
            return Fishes.Last();
        }
    }
}
