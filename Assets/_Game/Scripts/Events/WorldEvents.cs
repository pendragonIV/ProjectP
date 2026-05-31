using Things.Core.Infrastructure;

namespace Things.Game.Events
{
    public struct WorldLoadStartedEvent : IGameEvent
    {
        public string WorldId;
    }

    public struct WorldLoadProgressEvent : IGameEvent
    {
        public float Progress;
    }

    public struct WorldLoadedEvent : IGameEvent
    {
        public string WorldId;
    }

    public struct WorldUnloadedEvent : IGameEvent
    {
        public string WorldId;
    }
}
