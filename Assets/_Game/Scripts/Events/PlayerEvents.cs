using Things.Core.Infrastructure;

namespace Things.Game.Events
{
    public struct PlayerSpawnedEvent : IGameEvent { }
    
    public struct PlayerDiedEvent : IGameEvent { }
    
    public struct PlayerDamagedEvent : IGameEvent
    {
        public float DamageAmount;
    }
}
