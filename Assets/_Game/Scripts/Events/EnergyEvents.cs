using Things.Core.Infrastructure;

namespace Things.Game.Events
{
    public struct EnergyChangedEvent : IGameEvent
    {
        public float CurrentEnergy;
        public float MaxEnergy;
    }

    public struct EnergyDepletedEvent : IGameEvent { }

    public struct FoodConsumedEvent : IGameEvent
    {
        public string FoodId;
        public float EnergyRestored;
    }
}
