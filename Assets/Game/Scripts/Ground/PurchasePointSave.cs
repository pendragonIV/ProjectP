using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectP
{
    public class PurchasePointSave : ResourceListSave
    {
        [SerializeField] bool isBought;
        public bool IsBought { get => isBought; set => isBought = value; }
    }
}
