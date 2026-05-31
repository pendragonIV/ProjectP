using System;
using System.Collections.Generic;
using UnityEngine;

namespace Things.Core.Infrastructure
{
    /// <summary>
    /// Typed pub/sub event system. Decoupled communication between systems.
    /// Use EventBusListener component for auto-cleanup on MonoBehaviours.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<object>> bindings = new Dictionary<Type, List<object>>();

        // Reusable buffer to avoid GC allocation during Publish
        private static readonly List<object> publishBuffer = new List<object>(32);

        /// <summary>
        /// Subscribe to events of type T. Returns an EventBinding that must be kept for unsubscription.
        /// </summary>
        public static EventBinding<T> Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var type = typeof(T);
            if (!bindings.ContainsKey(type))
            {
                bindings[type] = new List<object>();
            }

            var binding = new EventBinding<T>(handler);
            bindings[type].Add(binding);
            return binding;
        }

        /// <summary>
        /// Unsubscribe a previously registered binding.
        /// </summary>
        public static void Unsubscribe<T>(EventBinding<T> binding) where T : IGameEvent
        {
            var type = typeof(T);
            if (bindings.TryGetValue(type, out var list))
            {
                list.Remove(binding);
            }
        }

        /// <summary>
        /// Publish an event to all subscribers. Uses an internal buffer to avoid GC allocations.
        /// Safe against concurrent modification (subscriber adding/removing during iteration).
        /// </summary>
        public static void Publish<T>(T gameEvent) where T : IGameEvent
        {
            var type = typeof(T);
            if (!bindings.TryGetValue(type, out var list) || list.Count == 0)
                return;

            // Copy to reusable buffer instead of allocating new list each time
            publishBuffer.Clear();
            publishBuffer.AddRange(list);

            for (int i = 0; i < publishBuffer.Count; i++)
            {
                try
                {
                    (publishBuffer[i] as EventBinding<T>)?.Invoke(gameEvent);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            publishBuffer.Clear();
        }

        /// <summary>
        /// Clear all subscriptions. Call during scene transitions or game shutdown.
        /// </summary>
        public static void Clear()
        {
            bindings.Clear();
        }

        /// <summary>
        /// Returns the number of subscribers for a given event type. Useful for debugging.
        /// </summary>
        public static int GetSubscriberCount<T>() where T : IGameEvent
        {
            var type = typeof(T);
            return bindings.TryGetValue(type, out var list) ? list.Count : 0;
        }
    }
}
