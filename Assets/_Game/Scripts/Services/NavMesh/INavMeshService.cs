using Things.Core.Infrastructure;

namespace Things.Game.Services.NavMesh
{
    public interface INavMeshService : IService
    {
        void RebuildNavMesh();
    }
}
