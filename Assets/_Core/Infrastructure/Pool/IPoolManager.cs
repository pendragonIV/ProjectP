using UnityEngine;

namespace Things.Core.Infrastructure
{
    /// <summary>
    /// Service interface for managing multiple object pools by prefab.
    /// Register in ServiceLocator for global access.
    /// </summary>
    public interface IPoolManager : IService
    {
        /// <summary>
        /// Get a component from the pool for the given prefab.
        /// Creates a new pool automatically if one doesn't exist for this prefab.
        /// </summary>
        T Get<T>(T prefab) where T : Component;

        /// <summary>
        /// Get a component and set its position/rotation.
        /// </summary>
        T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component;

        /// <summary>
        /// Return a component to its pool.
        /// </summary>
        void Release<T>(T instance) where T : Component;

        /// <summary>
        /// Pre-warm a pool for a specific prefab.
        /// </summary>
        void PreWarm<T>(T prefab, int count) where T : Component;

        /// <summary>
        /// Clear all pools, destroying all pooled objects.
        /// </summary>
        void ClearAll();
    }
}
