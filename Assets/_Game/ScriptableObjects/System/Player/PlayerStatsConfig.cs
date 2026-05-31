using UnityEngine;

namespace Things.Game.Configs.PlayerConfig
{
    [CreateAssetMenu(menuName = "Things/Player/Stats Config")]
    public class PlayerStatsConfig : ScriptableObject
    {
        public float MaxHealth = 100f;
        public float MaxEnergy = 100f;
        public float BaseDamage = 10f;
    }
}
