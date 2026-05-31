using UnityEngine;

namespace Things.Game.Configs.Resource
{
    [CreateAssetMenu(menuName = "Things/Resource/Source Config")]
    public class ResourceSourceConfig : ScriptableObject
    {
        public string SourceId;
        public int MaxHits = 3;
        public string DroppedCurrencyId;
        public float RespawnDuration = 10f;
    }
}
