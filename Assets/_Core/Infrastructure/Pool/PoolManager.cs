using System;
using System.Collections.Generic;
using UnityEngine;

namespace Things.Core.Infrastructure
{
    /// <summary>
    /// Manages multiple ObjectPools keyed by prefab instance ID.
    /// Implements IPoolManager for ServiceLocator registration.
    /// Implements IDisposable to clean up all pools on shutdown.
    /// </summary>
    public class PoolManager : IPoolManager, IDisposable
    {
        private readonly Dictionary<int, object> pools = new Dictionary<int, object>();
        private readonly Transform poolRoot;

        /// <summary>
        /// Create a PoolManager. Optionally provide a root transform for organization.
        /// </summary>
        /// <param name="poolRoot">Parent transform for all pooled objects. If null, a new "[PoolManager]" GO is created.</param>
        public PoolManager(Transform poolRoot = null)
        {
            if (poolRoot == null)
            {
                var go = new GameObject("[PoolManager]");
                UnityEngine.Object.DontDestroyOnLoad(go);
                this.poolRoot = go.transform;
            }
            else
            {
                this.poolRoot = poolRoot;
            }
        }

        public T Get<T>(T prefab) where T : Component
        {
            var pool = GetOrCreatePool(prefab);
            return pool.Get();
        }

        public T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            var pool = GetOrCreatePool(prefab);
            return pool.Get(position, rotation);
        }

        public void Release<T>(T instance) where T : Component
        {
            if (instance == null) return;

            // Find the pool this instance belongs to by checking all pools
            // In practice, you'd track instanceâ†’pool mapping for O(1), 
            // but for simplicity we iterate (pool count is usually small)
            foreach (var kvp in pools)
            {
                if (kvp.Value is ObjectPool<T> pool)
                {
                    pool.Release(instance);
                    return;
                }
            }

            // Fallback: just destroy if no pool found
            Debug.LogWarning($"[PoolManager] No pool found for {instance.name}. Destroying instead.");
            UnityEngine.Object.Destroy(instance.gameObject);
        }

        public void PreWarm<T>(T prefab, int count) where T : Component
        {
            var pool = GetOrCreatePool(prefab);
            pool.PreWarm(count);
        }

        public void ClearAll()
        {
            foreach (var kvp in pools)
            {
                if (kvp.Value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            pools.Clear();
        }

        public void Dispose()
        {
            ClearAll();
            if (poolRoot != null)
            {
                UnityEngine.Object.Destroy(poolRoot.gameObject);
            }
        }

        private ObjectPool<T> GetOrCreatePool<T>(T prefab) where T : Component
        {
            int key = prefab.GetInstanceID();

            if (pools.TryGetValue(key, out var existing))
            {
                return (ObjectPool<T>)existing;
            }

            // Create a child transform for this pool's objects
            var poolParent = new GameObject($"Pool [{prefab.name}]").transform;
            poolParent.SetParent(poolRoot);

            var pool = new ObjectPool<T>(prefab, poolParent);
            pools[key] = pool;
            return pool;
        }
    }
}
