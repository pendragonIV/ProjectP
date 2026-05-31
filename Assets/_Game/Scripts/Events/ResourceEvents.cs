using Things.Core.Infrastructure;

namespace Things.Game.Events
{
    public struct ResourceHarvestedEvent : IGameEvent
    {
        public string SourceId;
        public string CurrencyId;
    }

    public struct ResourceIncomingEvent : IGameEvent
    {
        public string CurrencyId;
        public int Amount;
    }
}
