using UnityEngine;
using System.Collections.Generic;

namespace Things.Game.Configs.Building
{
    [System.Serializable]
    public struct ResourceCost
    {
        public string CurrencyId;
        public int Amount;
    }

    [CreateAssetMenu(menuName = "Things/Building/Definition")]
    public class BuildingDefinition : ScriptableObject
    {
        public string BuildingId;
        public string DisplayName;
        public int RequiredHitsToConstruct = 3;
        public List<ResourceCost> RequiredResources = new List<ResourceCost>();
        public GameObject ConstructedPrefab;
    }
}
