using System;

namespace Things.Game.Models
{
    public class UpgradeModel
    {
        public string UpgradeId { get; private set; }
        public int CurrentLevel { get; private set; }

        public event Action<int> OnLevelChanged;

        public UpgradeModel(string upgradeId, int level)
        {
            UpgradeId = upgradeId;
            CurrentLevel = level;
        }

        public void LevelUp()
        {
            CurrentLevel++;
            OnLevelChanged?.Invoke(CurrentLevel);
        }
    }
}
