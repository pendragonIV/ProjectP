using System.Collections.Generic;
using UnityEngine;

namespace Things.Core.Infrastructure
{
    /// <summary>
    /// Generic object pool for Unity Components. Reduces GC pressure by reusing objects.
    /// Usage: var pool = new ObjectPool&lt;Bullet&gt;(prefab, parent, 10);
    ///        var bullet = pool.Get();
    ///        pool.Release(bullet);
    /// </summary>
    public class ObjectPool<T> where T : Component
    {
        private readonly T prefab;
        private readonly Transform parent;
        private readonly Queue<T> available = new Queue<T>();
        private readonly HashSet<T> inUse = new HashSet<T>();

        public int CountAll => available.Count + inUse.Count;
        public int CountAvailable => available.Count;
        public int CountInUse => inUse.Count;

        /// <summary>
        /// Create a new pool for the given prefab.
        /// </summary>
        /// <param name="prefab">Prefab to instantiate.</param>
        /// <param name="parent">Parent transform for pooled objects (null = root).</param>
        /// <param name="preWarmCount">Number of objects to pre-instantiate.</param>
        public ObjectPool(T prefab, Transform parent = null, int preWarmCount = 0)
        {
            this.prefab = prefab;
            this.parent = parent;

            if (preWarmCount > 0)
            {
                PreWarm(preWarmCount);
            }
        }

        /// <summary>
        /// Pre-instantiate objects into the pool.
        /// </summary>
        public void PreWarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = CreateInstance();
                obj.gameObject.SetActive(false);
                available.Enqueue(obj);
            }
        }

        /// <summary>
        /// Get an object from the pool. Creates a new one if pool is empty.
        /// The returned object is active and ready to use.
        /// </summary>
        public T Get()
        {
            T obj;

            if (available.Count > 0)
            {
                obj = available.Dequeue();

                // Handle Unity destroyed objects in pool
                if (obj == null)
                {
                    obj = CreateInstance();
                }
            }
            else
            {
                obj = CreateInstance();
            }

            obj.gameObject.SetActive(true);
            inUse.Add(obj);
            return obj;
        }

        /// <summary>
        /// Get an object and set its position/rotation.
        /// </summary>
        public T Get(Vector3 position, Quaternion rotation)
        {
            var obj = Get();
            obj.transform.SetPositionAndRotation(position, rotation);
            return obj;
        }

        /// <summary>
        /// Return an object to the pool. The object is deactivated.
        /// </summary>
        public void Release(T obj)
        {
            if (obj == null) return;

            if (!inUse.Remove(obj))
            {
                Debug.LogWarning($"[ObjectPool] Trying to release an object that wasn't from this pool: {obj.name}");
                return;
            }

            obj.gameObject.SetActive(false);
            obj.transform.SetParent(parent);
            available.Enqueue(obj);
        }

        /// <summary>
        /// Destroy all pooled objects (both available and in-use).
        /// </summary>
        public void Clear()
        {
            foreach (var obj in available)
            {
                if (obj != null)
                    Object.Destroy(obj.gameObject);
            }

            foreach (var obj in inUse)
            {
                if (obj != null)
                    Object.Destroy(obj.gameObject);
            }

            available.Clear();
            inUse.Clear();
        }

        private T CreateInstance()
        {
            var obj = Object.Instantiate(prefab, parent);
            obj.gameObject.name = $"{prefab.name} [Pool]";
            return obj;
        }
    }
}
