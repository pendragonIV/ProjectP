using System;

namespace Things.Game.Models
{
    public class WorldStateModel
    {
        public string CurrentWorldId { get; private set; }

        public event Action<string> OnWorldChanged;

        public void SetCurrentWorld(string worldId)
        {
            CurrentWorldId = worldId;
            OnWorldChanged?.Invoke(worldId);
        }
    }
}
