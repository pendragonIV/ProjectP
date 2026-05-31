using UnityEngine;
using Things.Game.Models;
using Things.Game.Configs.Building;

namespace Things.Game.Building
{
    public class ConstructionPresenter : MonoBehaviour
    {
        private BuildingStateModel model;
        private BuildingDefinition def;

        public void Initialize(BuildingStateModel model, BuildingDefinition def)
        {
            this.model = model;
            this.def = def;
        }

        // Gọi khi Player dùng búa đập vào (sẽ nhận từ HitBox)
        public void OnHit()
        {
            if (model.IsConstructed) return;
            model.ProcessHit();
        }
    }
}
