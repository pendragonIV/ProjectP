using Things.Core.Infrastructure;

namespace Things.Game.Events
{
    public struct UpgradeChangedEvent : IGameEvent
    {
        public string UpgradeId;
        public int NewLevel;
    }
}
