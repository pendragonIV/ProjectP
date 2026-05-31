using Things.Game.Configs.Mission;
using Things.Game.Models;
using Things.Game.Events;
using Things.Core.Infrastructure;
using Things.Game.Services.Currency;

namespace Things.Game.Services.Mission
{
    public class MissionService : IMissionService
    {
        private MissionProgressModel currentMissionModel;
        private MissionDefinition currentMissionDef;

        public void StartMission(MissionDefinition mission)
        {
            currentMissionDef = mission;
            currentMissionModel = new MissionProgressModel();
            currentMissionModel.Initialize(mission.MissionId, mission.TargetProgress);
            
            EventBus.Publish(new MissionStartedEvent { MissionId = mission.MissionId });
        }

        public MissionDefinition GetActiveMission() => currentMissionDef;

        public void AdvanceProgress(int amount)
        {
            if (currentMissionModel != null)
            {
                currentMissionModel.AddProgress(amount);
                if (currentMissionModel.IsCompleted)
                {
                    EventBus.Publish(new MissionCompletedEvent { MissionId = currentMissionDef.MissionId });
                }
            }
        }

        public void ClaimReward()
        {
            if (currentMissionModel != null && currentMissionModel.IsCompleted)
            {
                if (currentMissionDef != null && currentMissionDef.RewardAmount > 0)
                {
                    if (ServiceLocator.TryGet<ICurrencyService>(out var currencySvc))
                    {
                        currencySvc.Add(currentMissionDef.RewardCurrencyId, currentMissionDef.RewardAmount);
                    }
                }

                EventBus.Publish(new MissionRewardClaimedEvent { MissionId = currentMissionDef.MissionId });
                currentMissionModel = null;
                currentMissionDef = null;
            }
        }
    }
}
