namespace Things.Game.Events
{
    public struct FriendshipLevelChangedEvent : Things.Core.Infrastructure.IGameEvent
    {
        public string PlayerId;
        public int NewLevel;
    }
}
