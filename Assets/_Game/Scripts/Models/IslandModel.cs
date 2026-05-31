using System.Collections.Generic;

namespace Things.Game.Models
{
    [System.Serializable]
    public class IslandModel
    {
        public string IslandId;
        
        [System.Serializable]
        public struct DecorationData
        {
            public string DecorationId;
            public float X, Y, Z;
        }

        public List<DecorationData> Decorations = new List<DecorationData>();
    }
}
