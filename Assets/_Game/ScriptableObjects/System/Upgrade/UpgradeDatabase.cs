using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Things.Game.Configs.Upgrade
{
    [CreateAssetMenu(menuName = "Things/Upgrade/Database")]
    public class UpgradeDatabase : ScriptableObject
    {
        public List<UpgradeDefinition> Upgrades = new List<UpgradeDefinition>();
        
        private Dictionary<string, UpgradeDefinition> lookup;

        public void Initialize()
        {
            lookup = new Dictionary<string, UpgradeDefinition>();
            foreach (var u in Upgrades)
            {
                if (u != null && !string.IsNullOrEmpty(u.UpgradeId))
                {
                    lookup[u.UpgradeId] = u;
                }
            }
        }

        public UpgradeDefinition GetById(string id)
        {
            if (lookup == null) Initialize();
            return lookup != null && lookup.TryGetValue(id, out var def) ? def : null;
        }
    }
}
