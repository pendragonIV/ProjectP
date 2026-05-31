using UnityEngine;

namespace Things.GlobalUpgrades
{
    [System.Serializable]
    public abstract class GlobalUpgradeStage
    {
        [SerializeField] int price;
        public int Price => price;

        [SerializeField] CurrencyType currencyType;
        public CurrencyType CurrencyType => currencyType;

        [SerializeField] Sprite previewSprite;
        public Sprite PreviewSprite => previewSprite;
    }
}