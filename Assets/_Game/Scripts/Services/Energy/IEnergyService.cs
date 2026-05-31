using Things.Core.Infrastructure;

namespace Things.Game.Services.Energy
{
    public interface IEnergyService : IService
    {
        float GetCurrentEnergy();
        float GetMaxEnergy();
        bool HasEnoughEnergy(float amount);
        void ConsumeEnergy(float amount);
        void RestoreEnergy(float amount);
    }
}
