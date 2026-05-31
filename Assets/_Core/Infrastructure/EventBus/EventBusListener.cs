using UnityEngine;
using System;
using System.Collections.Generic;

namespace Things.Core.Infrastructure
{
    public class EventBusListener : MonoBehaviour
    {
        private readonly List<Action> unsubscribeActions = new List<Action>();

        public EventBinding<T> Listen<T>(Action<T> handler) where T : IGameEvent
        {
            var binding = EventBus.Subscribe<T>(handler);
            unsubscribeActions.Add(() => EventBus.Unsubscribe(binding));
            return binding;
        }

        protected virtual void OnDestroy()
        {
            foreach (var unsub in unsubscribeActions)
            {
                unsub.Invoke();
            }
            unsubscribeActions.Clear();
        }
    }
}
