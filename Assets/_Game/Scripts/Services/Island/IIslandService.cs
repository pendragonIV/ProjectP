using Things.Core.Infrastructure;

namespace Things.Game.Services.Island
{
    public interface IIslandService : IService
    {
        void LoadIsland(string islandId);
        void PlaceDecoration(string decorationId, UnityEngine.Vector3 position);
    }
}
