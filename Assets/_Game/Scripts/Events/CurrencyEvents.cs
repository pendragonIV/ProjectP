using Things.Core.Infrastructure;

namespace Things.Game.Events
{
    public struct CurrencyChangedEvent : IGameEvent
    {
        public string CurrencyId;
        public int Delta;
        public int NewAmount;
    }
}
