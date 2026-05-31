using UnityEngine;

namespace Things.Game.Configs.PlayerConfig
{
    [CreateAssetMenu(menuName = "Things/Player/Harvest Config")]
    public class PlayerHarvestConfig : ScriptableObject
    {
        [Header("Harvest stats")]
        public float HarvestSpeed = 1f;
        public float MaxHarvestDistance = 2f;
    }
}
