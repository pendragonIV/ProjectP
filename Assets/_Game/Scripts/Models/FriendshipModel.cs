using System.Collections.Generic;

namespace Things.Game.Models
{
    public class FriendshipModel
    {
        public Dictionary<string, int> FriendLevels = new Dictionary<string, int>();

        public void SetLevel(string playerId, int level)
        {
            FriendLevels[playerId] = level;
        }
    }
}
