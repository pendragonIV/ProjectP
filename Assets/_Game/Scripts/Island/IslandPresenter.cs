using UnityEngine;
using Things.Core.Infrastructure;
using Things.Game.Services.Island;

namespace Things.Game.Island
{
    public class IslandPresenter : MonoBehaviour
    {
        private IIslandService islandService;

        private void Start()
        {
            ServiceLocator.TryGet(out islandService);
        }

        public void OnDecorationPlaced(string decorationId, Vector3 position)
        {
            islandService?.PlaceDecoration(decorationId, position);
        }
    }
}
