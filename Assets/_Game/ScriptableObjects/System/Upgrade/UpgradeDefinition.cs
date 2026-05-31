using UnityEngine;

namespace Things.Game.Configs.Upgrade
{
    [CreateAssetMenu(menuName = "Things/Upgrade/Definition")]
    public class UpgradeDefinition : ScriptableObject
    {
        public string UpgradeId;
        public string DisplayName;
        public int MaxLevel = 10;
        public float BaseValue;
        public float IncrementPerLevel;
        public string CostCurrencyId;
        public int BaseCost;
        public float CostMultiplierPerLevel;
    }
}
