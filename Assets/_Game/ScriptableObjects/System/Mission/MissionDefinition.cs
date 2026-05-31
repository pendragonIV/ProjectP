using UnityEngine;

namespace Things.Game.Configs.Mission
{
    [CreateAssetMenu(menuName = "Things/Mission/Definition")]
    public class MissionDefinition : ScriptableObject
    {
        public string MissionId;
        public string Description;
        public int TargetProgress;
        public string RewardCurrencyId;
        public int RewardAmount;
    }
}
