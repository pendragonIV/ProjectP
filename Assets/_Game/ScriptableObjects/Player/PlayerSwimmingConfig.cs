using UnityEngine;

namespace Things.Game.Configs.PlayerConfig
{
    [CreateAssetMenu(menuName = "Things/Player/Swimming Config")]
    public class PlayerSwimmingConfig : ScriptableObject
    {
        [Header("Swimming stats")]
        public float SwimSpeed = 3f;
        public float OxygenDepletionRate = 1f;
    }
}
