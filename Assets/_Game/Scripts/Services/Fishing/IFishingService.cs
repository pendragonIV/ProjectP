using Things.Core.Infrastructure;

namespace Things.Game.Services.Fishing
{
    public interface IFishingService : IService
    {
        bool TryCatchFish(string fishingSpotId);
        void StartFishing(string fishingSpotId);
        void CancelFishing();
    }
}
