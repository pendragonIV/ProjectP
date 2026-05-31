using UnityEngine;

namespace Things.Game.Configs.EnemyConfig
{
    [CreateAssetMenu(menuName = "Things/Enemy/Definition")]
    public class EnemyDefinition : ScriptableObject
    {
        public string EnemyId;
        public float MaxHealth = 100f;
        public float Damage = 10f;
        public float MoveSpeed = 3f;
        public GameObject Prefab;
    }
}
