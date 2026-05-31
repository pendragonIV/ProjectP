using UnityEngine;

namespace Things
{
    public class IAPSettings : ScriptableObject
    {
        [SerializeField, Hide] IAPItem[] storeItems;
        public IAPItem[] StoreItems => storeItems;
    }
}