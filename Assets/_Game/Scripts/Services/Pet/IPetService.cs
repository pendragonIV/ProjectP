using Things.Core.Infrastructure;

namespace Things.Game.Services.Pet
{
    public interface IPetService : IService
    {
        void SpawnPet(string petId, UnityEngine.Vector3 position);
        void FeedPet(string petId);
    }
}
