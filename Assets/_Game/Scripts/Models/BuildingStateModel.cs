using System;

namespace Things.Game.Models
{
    public class BuildingStateModel
    {
        public string BuildingId { get; private set; }
        public bool IsPurchased { get; private set; }
        public bool IsConstructed { get; private set; }
        public int HitsRemaining { get; private set; }

        public event Action OnPurchased;
        public event Action<int> OnHit;
        public event Action OnConstructed;

        public BuildingStateModel(string id)
        {
            BuildingId = id;
        }

        public void SetPurchased(int requiredHits)
        {
            IsPurchased = true;
            HitsRemaining = requiredHits;
            OnPurchased?.Invoke();
        }

        public void ProcessHit()
        {
            if (!IsPurchased || IsConstructed) return;
            
            HitsRemaining--;
            OnHit?.Invoke(HitsRemaining);
            
            if (HitsRemaining <= 0)
            {
                IsConstructed = true;
                OnConstructed?.Invoke();
            }
        }
    }
}
