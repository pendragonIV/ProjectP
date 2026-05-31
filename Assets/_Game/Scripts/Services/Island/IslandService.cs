using Things.Core.Infrastructure;
using Things.Game.Models;
using UnityEngine;

namespace Things.Game.Services.Island
{
    public class IslandService : IIslandService
    {
        private const string SAVE_KEY_PREFIX = "IslandData_";
        private IslandModel currentIsland;
        private ISaveService saveService;

        private ISaveService SaveService
        {
            get
            {
                if (saveService == null)
                    ServiceLocator.TryGet(out saveService);
                return saveService;
            }
        }

        public void LoadIsland(string islandId)
        {
            string saveKey = SAVE_KEY_PREFIX + islandId;
            currentIsland = SaveService?.Load<IslandModel>(saveKey);
            if (currentIsland == null)
            {
                currentIsland = new IslandModel { IslandId = islandId };
            }
            
            Debug.Log($"[IslandService] Loaded island {islandId} with {currentIsland.Decorations.Count} decorations.");
        }

        public void PlaceDecoration(string decorationId, Vector3 position)
        {
            if (currentIsland == null)
            {
                Debug.LogWarning("[IslandService] Cannot place decoration, no island loaded.");
                return;
            }

            currentIsland.Decorations.Add(new IslandModel.DecorationData
            {
                DecorationId = decorationId,
                X = position.x,
                Y = position.y,
                Z = position.z
            });

            // Save immediately for MVP
            string saveKey = SAVE_KEY_PREFIX + currentIsland.IslandId;
            SaveService?.Save(saveKey, currentIsland);
            
            Debug.Log($"[IslandService] Placed {decorationId} at {position} and saved.");
        }
    }
}
