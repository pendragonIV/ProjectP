using UnityEngine;

namespace Things
{
    public interface ISubworldEntrance
    {
        public SubworldBehavior SubworldBehavior { get; }
        public Transform SubworldSpawnPoint { get; }

        public Transform ExitSpawnPoint { get; }
    }
}