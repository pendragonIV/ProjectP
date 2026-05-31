using Things.Core.Infrastructure;
using Things.Game.Events;
using Things.Game.Models;

namespace Things.Game.Services.Social
{
    public class FriendshipService : IFriendshipService
    {
        private const string SAVE_KEY = "FriendshipData";
        private FriendshipModel model;
        private ISaveService saveService;

        private ISaveService SaveService
        {
            get
            {
                if (saveService == null)
                    ServiceLocator.TryGet(out saveService);
                return saveService;
            }
        }

        public FriendshipService()
        {
            // Try to load model immediately, but SaveService might not be ready.
            // We'll load lazily.
        }

        private void EnsureModelLoaded()
        {
            if (model != null) return;
            model = SaveService?.Load<FriendshipModel>(SAVE_KEY) ?? new FriendshipModel();
        }

        private void SaveModel()
        {
            SaveService?.Save(SAVE_KEY, model);
        }

        public void AddFriend(string playerId)
        {
            EnsureModelLoaded();
            if (!model.FriendLevels.ContainsKey(playerId))
            {
                model.SetLevel(playerId, 1);
                SaveModel();
                EventBus.Publish(new FriendshipLevelChangedEvent { PlayerId = playerId, NewLevel = 1 });
                UnityEngine.Debug.Log($"[FriendshipService] Added friend {playerId}");
            }
        }

        public void SendGift(string playerId, string itemId)
        {
            EnsureModelLoaded();
            if (model.FriendLevels.TryGetValue(playerId, out int currentLevel))
            {
                int newLevel = currentLevel + 1;
                model.SetLevel(playerId, newLevel);
                SaveModel();
                EventBus.Publish(new FriendshipLevelChangedEvent { PlayerId = playerId, NewLevel = newLevel });
                UnityEngine.Debug.Log($"[FriendshipService] Sent gift {itemId} to {playerId}. New level: {newLevel}");
            }
        }

        public int GetFriendshipLevel(string playerId)
        {
            EnsureModelLoaded();
            return model.FriendLevels.TryGetValue(playerId, out int level) ? level : 0;
        }
    }
}
