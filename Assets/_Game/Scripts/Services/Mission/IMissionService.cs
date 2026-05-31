using Things.Core.Infrastructure;
using Things.Game.Configs.Mission;

namespace Things.Game.Services.Mission
{
    public interface IMissionService : IService
    {
        MissionDefinition GetActiveMission();
        void AdvanceProgress(int amount);
        void ClaimReward();
    }
}
