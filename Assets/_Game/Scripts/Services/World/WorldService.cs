using Things.Game.Configs.World;
using Things.Game.Models;
using Things.Game.Events;
using Things.Core.Infrastructure;
using UnityEngine.SceneManagement;

namespace Things.Game.Services.World
{
    public class WorldService : IWorldService
    {
        private readonly WorldStateModel worldModel;
        private WorldDefinition currentWorldDef;

        public WorldService()
        {
            worldModel = new WorldStateModel();
        }

        public void LoadWorld(WorldDefinition world)
        {
            EventBus.Publish(new WorldLoadStartedEvent { WorldId = world.WorldId });
            currentWorldDef = world;
            worldModel.SetCurrentWorld(world.WorldId);
            
            var asyncOp = SceneManager.LoadSceneAsync(world.SceneName);
            if (asyncOp != null)
            {
                asyncOp.completed += (op) => 
                {
                    EventBus.Publish(new WorldLoadedEvent { WorldId = world.WorldId });
                };
            }
            else
            {
                // Fallback in case scene doesn't exist or Editor issues
                EventBus.Publish(new WorldLoadedEvent { WorldId = world.WorldId });
            }
        }

        public WorldDefinition GetCurrentWorld() => currentWorldDef;
    }
}
