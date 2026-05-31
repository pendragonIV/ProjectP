using UnityEngine;
using Things.Game.Models;
using Things.Game.Configs.Building;

namespace Things.Game.Building
{
    public class BuildingPresenter : MonoBehaviour
    {
        [SerializeField] private BuildingDefinition definition;
        [SerializeField] private BuildingView view;
        [SerializeField] private PurchasePresenter purchasePresenter;
        [SerializeField] private ConstructionPresenter constructionPresenter;

        private BuildingStateModel model;

        private void Awake()
        {
            model = new BuildingStateModel(definition.BuildingId);
            model.OnPurchased += OnPurchased;
            model.OnConstructed += OnConstructed;

            purchasePresenter.Initialize(model, definition);
            constructionPresenter.Initialize(model, definition);
            
            view.ShowLockedState();
        }

        private void OnPurchased()
        {
            view.ShowConstructionState();
            purchasePresenter.gameObject.SetActive(false);
            constructionPresenter.gameObject.SetActive(true);
        }

        private void OnConstructed()
        {
            view.ShowConstructedState(definition.ConstructedPrefab);
            constructionPresenter.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (model != null)
            {
                model.OnPurchased -= OnPurchased;
                model.OnConstructed -= OnConstructed;
            }
        }
    }
}
