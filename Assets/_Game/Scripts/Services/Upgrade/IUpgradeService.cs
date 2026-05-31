using Things.Core.Infrastructure;
using Things.Game.Configs.Upgrade;

namespace Things.Game.Services.Upgrade
{
    public interface IUpgradeService : IService
    {
        int GetLevel(string upgradeId);
        float GetValue(string upgradeId);
        bool TryPurchaseUpgrade(string upgradeId);
        UpgradeDefinition GetDefinition(string upgradeId);
    }
}
