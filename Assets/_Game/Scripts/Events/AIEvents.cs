using Things.Core.Infrastructure;

namespace Things.Game.Events
{
    public struct HelperTaskStartedEvent : IGameEvent
    {
        public string TaskId;
    }

    public struct HelperTaskCompletedEvent : IGameEvent
    {
        public string TaskId;
    }
}
