using System;

namespace Things.Core.Infrastructure
{
    public class EventBinding<T> where T : IGameEvent
    {
        private readonly Action<T> action;

        public EventBinding(Action<T> action)
        {
            this.action = action;
        }

        public void Invoke(T gameEvent)
        {
            action?.Invoke(gameEvent);
        }
    }
}
