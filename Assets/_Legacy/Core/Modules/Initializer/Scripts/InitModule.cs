using UnityEngine;

namespace Things
{
    public abstract class InitModule : ScriptableObject
    {
        public abstract string ModuleName { get; }

        public abstract void CreateComponent();
    }
}