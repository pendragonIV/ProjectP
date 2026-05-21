using UnityEngine;

namespace ProjectP
{
    public interface IResourcePicker
    {
        public bool AutoPickResources { get; }
        public Transform SnappingTransform { get; }

        public void OnResourcePickPerformed(ResourceDropBehavior dropBehavior);
    }
}