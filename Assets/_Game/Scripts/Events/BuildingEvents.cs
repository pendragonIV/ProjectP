using Things.Core.Infrastructure;

namespace Things.Game.Events
{
    public struct BuildingPurchasedEvent : IGameEvent
    {
        public string BuildingId;
    }

    public struct BuildingConstructedEvent : IGameEvent
    {
        public string BuildingId;
    }
}
