using UnityEngine;

namespace Things.Game.Configs.PlayerConfig
{
    [CreateAssetMenu(menuName = "Things/Player/Combat Config")]
    public class PlayerCombatConfig : ScriptableObject
    {
        [Header("Combat stats")]
        public float BaseDamage = 10f;
        public float BaseAttackSpeed = 1f;
    }
}
