using Things.Core.Infrastructure;
using Unity.AI.Navigation;

namespace Things.Game.Services.NavMesh
{
    public class NavMeshService : INavMeshService
    {
        public void RebuildNavMesh()
        {
            var surfaces = UnityEngine.Object.FindObjectsByType<NavMeshSurface>(UnityEngine.FindObjectsSortMode.None);
            foreach (var surface in surfaces)
            {
                surface.BuildNavMesh();
            }
        }
    }
}
