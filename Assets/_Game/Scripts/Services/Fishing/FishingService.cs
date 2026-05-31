using Things.Core.Infrastructure;
using Things.Game.Services.Currency;

namespace Things.Game.Services.Fishing
{
    public class FishingService : IFishingService
    {
        private readonly Things.Game.Configs.Fishing.FishingDatabase database;
        private ICurrencyService currencyService;

        public FishingService(Things.Game.Configs.Fishing.FishingDatabase database)
        {
            this.database = database;
        }

        private ICurrencyService CurrencyService
        {
            get
            {
                if (currencyService == null)
                    ServiceLocator.TryGet(out currencyService);
                return currencyService;
            }
        }

        public bool TryCatchFish(string fishingSpotId)
        {
            var fish = database?.GetRandomFish();
            if (fish == null) return false;

            // Simple difficulty check (RNG vs CatchDifficulty)
            float roll = UnityEngine.Random.value; // 0.0 to 1.0
            if (roll <= fish.CatchDifficulty)
            {
                // Caught!
                CurrencyService?.Add(fish.RewardCurrencyId, 1);
                UnityEngine.Debug.Log($"[FishingService] Caught {fish.DisplayName}!");
                return true;
            }

            UnityEngine.Debug.Log("[FishingService] The fish got away...");
            return false;
        }

        public void StartFishing(string fishingSpotId)
        {
            // Lock player state, prepare for mini-game
            UnityEngine.Debug.Log($"[FishingService] Started fishing at {fishingSpotId}");
        }

        public void CancelFishing()
        {
            // Unlock player state
            UnityEngine.Debug.Log("[FishingService] Cancelled fishing.");
        }
    }
}
