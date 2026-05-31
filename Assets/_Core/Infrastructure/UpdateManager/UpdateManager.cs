using System.Collections.Generic;
using UnityEngine;

namespace Things.Core.Infrastructure
{
    [DefaultExecutionOrder(-50)]
    [DisallowMultipleComponent]
    public class UpdateManager : MonoBehaviour, IService
    {
        private static UpdateManager instance;

        private readonly Dictionary<UpdatePhase, List<IUpdatable>> updatables = new Dictionary<UpdatePhase, List<IUpdatable>>()
        {
            { UpdatePhase.Early, new List<IUpdatable>() },
            { UpdatePhase.Normal, new List<IUpdatable>() },
            { UpdatePhase.Late, new List<IUpdatable>() },
            { UpdatePhase.Fixed, new List<IUpdatable>() }
        };

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        /// <summary>
        /// Register an IUpdatable to receive managed update ticks.
        /// </summary>
        public static void Register(IUpdatable target, UpdatePhase phase = UpdatePhase.Normal)
        {
            if (instance == null)
            {
                Debug.LogWarning("[UpdateManager] No instance exists. Ensure an UpdateManager is in the scene.");
                return;
            }

            var list = instance.updatables[phase];
            if (!list.Contains(target))
            {
                list.Add(target);
            }
        }

        /// <summary>
        /// Unregister an IUpdatable from managed updates.
        /// </summary>
        public static void Unregister(IUpdatable target)
        {
            if (instance == null) return;

            foreach (var list in instance.updatables.Values)
            {
                list.Remove(target);
            }
        }

        /// <summary>
        /// Clear all registered updatables. Call on scene transitions or cleanup.
        /// </summary>
        public static void ClearAll()
        {
            if (instance == null) return;

            foreach (var list in instance.updatables.Values)
            {
                list.Clear();
            }
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            ProcessPhase(UpdatePhase.Early, dt);
            ProcessPhase(UpdatePhase.Normal, dt);
        }

        private void LateUpdate()
        {
            ProcessPhase(UpdatePhase.Late, Time.deltaTime);
        }

        private void FixedUpdate()
        {
            ProcessPhase(UpdatePhase.Fixed, Time.fixedDeltaTime);
        }

        private void ProcessPhase(UpdatePhase phase, float dt)
        {
            var list = updatables[phase];

            for (int i = list.Count - 1; i >= 0; i--)
            {
                var target = list[i];

                // Handle Unity's fake-null: destroyed MonoBehaviours still hold a C# reference
                if (target is Object unityObj && unityObj == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                // Handle actual null references
                if (target == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                try
                {
                    if (target.IsActive)
                    {
                        target.OnUpdate(dt);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                    // Remove the problematic updatable to prevent repeated exceptions
                    list.RemoveAt(i);
                }
            }
        }
    }
}
