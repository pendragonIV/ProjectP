using UnityEngine;

namespace ProjectP
{
    public class IAPSettings : ScriptableObject
    {
        [SerializeField, Hide] IAPItem[] storeItems;
        public IAPItem[] StoreItems => storeItems;
    }
}