using UnityEngine;

namespace Things.Game.Configs.Pet
{
    [CreateAssetMenu(menuName = "Things/Pet/Data")]
    public class PetDataSO : ScriptableObject
    {
        public string PetId;
        public string DisplayName;
        public float MoveSpeed = 3f;
        public GameObject PetPrefab;
    }
}
