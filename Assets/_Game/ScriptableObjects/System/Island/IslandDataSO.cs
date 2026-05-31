using UnityEngine;

namespace Things.Game.Configs.Island
{
    [CreateAssetMenu(menuName = "Things/Island/Data")]
    public class IslandDataSO : ScriptableObject
    {
        public string IslandId;
        public string OwnerId;
        public int SizeLevel;
    }
}
