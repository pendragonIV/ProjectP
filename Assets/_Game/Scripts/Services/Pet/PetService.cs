using UnityEngine;
using Things.Core.Infrastructure;

namespace Things.Game.Services.Pet
{
    public class PetService : IPetService
    {
        private Things.Game.Configs.Pet.PetDataSO currentPetData;
        private GameObject spawnedPet;

        public void SpawnPet(string petId, Vector3 position)
        {
            // Simple mockup: we need a database ideally, but for now we'll just log
            // Normally: var petData = database.GetById(petId);
            // var pool = ServiceLocator.Get<IPoolManager>();
            // spawnedPet = pool.Get(petData.PetPrefab, position, Quaternion.identity);
            
            Debug.Log($"[PetService] Spawned pet {petId} at {position}");
        }

        public void FeedPet(string petId)
        {
            if (spawnedPet != null)
            {
                var presenter = spawnedPet.GetComponent<Things.Game.Pet.PetPresenter>();
                presenter?.PlayHappyAnimation();
            }
            Debug.Log($"[PetService] Fed pet {petId}");
        }
    }
}
