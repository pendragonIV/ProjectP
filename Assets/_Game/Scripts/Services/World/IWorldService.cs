using Things.Core.Infrastructure;
using Things.Game.Configs.World;

namespace Things.Game.Services.World
{
    public interface IWorldService : IService
    {
        void LoadWorld(WorldDefinition world);
        WorldDefinition GetCurrentWorld();
    }
}
