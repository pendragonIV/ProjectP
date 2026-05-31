using UnityEngine;

namespace Things.Game.Configs.Currency
{
    [CreateAssetMenu(menuName = "Things/Currency/Definition")]
    public class CurrencyDefinition : ScriptableObject
    {
        public string CurrencyId;
        public Sprite Icon;
        public string DisplayName;
    }
}
