using System;
using System.Collections.Generic;
using UnityEngine;

namespace Things.Core.Infrastructure
{
    /// <summary>
    /// Central service registry. Replaces static singletons with a managed lifecycle.
    /// Thread-safe for reads after initialization.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, IService> services = new Dictionary<Type, IService>();

        /// <summary>
        /// Register a service by its interface type. Logs a warning on duplicate registration.
        /// </summary>
        public static void Register<T>(T service) where T : class, IService
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            var type = typeof(T);
            if (services.ContainsKey(type))
            {
                Debug.LogWarning($"[ServiceLocator] Service {type.Name} already registered. Skipping duplicate.");
                return;
            }
            services.Add(type, service);
        }

        /// <summary>
        /// Resolve a service. Throws InvalidOperationException if not found.
        /// </summary>
        public static T Get<T>() where T : class, IService
        {
            var type = typeof(T);
            if (services.TryGetValue(type, out var service))
            {
                return service as T;
            }
            throw new InvalidOperationException(
                $"[ServiceLocator] Service {type.Name} not found. " +
                "Ensure it is registered in GameBootstrapper before use.");
        }

        /// <summary>
        /// Try to resolve a service without throwing. Returns false if not found.
        /// </summary>
        public static bool TryGet<T>(out T service) where T : class, IService
        {
            var type = typeof(T);
            if (services.TryGetValue(type, out var svc))
            {
                service = svc as T;
                return true;
            }
            service = null;
            return false;
        }

        /// <summary>
        /// Unregister a specific service type.
        /// </summary>
        public static void Unregister<T>() where T : class, IService
        {
            var type = typeof(T);
            if (services.TryGetValue(type, out var service))
            {
                if (service is IDisposable disposable)
                {
                    try { disposable.Dispose(); }
                    catch (Exception e) { Debug.LogException(e); }
                }
                services.Remove(type);
            }
        }

        /// <summary>
        /// Dispose all services that implement IDisposable, then clear the registry.
        /// Exception-safe: continues disposing remaining services if one throws.
        /// </summary>
        public static void DisposeAll()
        {
            foreach (var kvp in services)
            {
                if (kvp.Value is IDisposable disposable)
                {
                    try
                    {
                        disposable.Dispose();
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"[ServiceLocator] Error disposing {kvp.Key.Name}: {e.Message}");
                    }
                }
            }
            services.Clear();
        }

        /// <summary>
        /// Clear all services without disposing. For unit test teardown.
        /// </summary>
        public static void Reset()
        {
            services.Clear();
        }
    }
}
