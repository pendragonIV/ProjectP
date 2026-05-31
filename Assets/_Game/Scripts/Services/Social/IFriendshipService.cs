using Things.Core.Infrastructure;

namespace Things.Game.Services.Social
{
    public interface IFriendshipService : IService
    {
        void AddFriend(string playerId);
        void SendGift(string playerId, string itemId);
        int GetFriendshipLevel(string playerId);
    }
}
