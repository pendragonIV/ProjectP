using Things.Game.Models;
using Things.Game.Events;
using Things.Core.Infrastructure;

namespace Things.Game.Services.Energy
{
    public class EnergyService : IEnergyService
    {
        private readonly EnergyModel energyModel;

        public EnergyService(float maxEnergy)
        {
            energyModel = new EnergyModel();
            energyModel.Initialize(maxEnergy);
            energyModel.OnChanged += OnEnergyChanged;
            energyModel.OnDepleted += OnEnergyDepleted;
        }

        private void OnEnergyChanged()
        {
            EventBus.Publish(new EnergyChangedEvent 
            { 
                CurrentEnergy = energyModel.CurrentEnergy, 
                MaxEnergy = energyModel.MaxEnergy 
            });
        }

        private void OnEnergyDepleted()
        {
            EventBus.Publish(new EnergyDepletedEvent());
        }

        public float GetCurrentEnergy() => energyModel.CurrentEnergy;
        public float GetMaxEnergy() => energyModel.MaxEnergy;
        public bool HasEnoughEnergy(float amount) => energyModel.CurrentEnergy >= amount;
        
        public void ConsumeEnergy(float amount)
        {
            energyModel.Consume(amount);
        }

        public void RestoreEnergy(float amount)
        {
            energyModel.Restore(amount);
        }
    }
}
