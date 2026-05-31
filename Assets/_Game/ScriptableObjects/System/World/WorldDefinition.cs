using UnityEngine;

namespace Things.Game.Configs.World
{
    [CreateAssetMenu(menuName = "Things/World/Definition")]
    public class WorldDefinition : ScriptableObject
    {
        public string WorldId;
        public string SceneName;
        public string DisplayName;
    }
}
