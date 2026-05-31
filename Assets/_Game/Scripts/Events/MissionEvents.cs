using Things.Core.Infrastructure;

namespace Things.Game.Events
{
    public struct MissionStartedEvent : IGameEvent
    {
        public string MissionId;
    }

    public struct MissionCompletedEvent : IGameEvent
    {
        public string MissionId;
    }

    public struct MissionRewardClaimedEvent : IGameEvent
    {
        public string MissionId;
    }
}
