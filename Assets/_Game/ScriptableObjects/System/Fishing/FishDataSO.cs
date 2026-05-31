using UnityEngine;

namespace Things.Game.Configs.Fishing
{
    [CreateAssetMenu(menuName = "Things/Fishing/Fish Data")]
    public class FishDataSO : ScriptableObject
    {
        public string FishId;
        public string DisplayName;
        public int Rarity;
        public float CatchDifficulty;
        public string RewardCurrencyId;
    }
}
