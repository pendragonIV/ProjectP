using UnityEngine;

namespace Things.Game.Building
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private GameObject lockedVisuals;
        [SerializeField] private GameObject constructionVisuals;

        public void ShowLockedState()
        {
            lockedVisuals.SetActive(true);
            constructionVisuals.SetActive(false);
        }

        public void ShowConstructionState()
        {
            lockedVisuals.SetActive(false);
            constructionVisuals.SetActive(true);
        }

        public void ShowConstructedState(GameObject constructedPrefab)
        {
            lockedVisuals.SetActive(false);
            constructionVisuals.SetActive(false);
            
            if (constructedPrefab != null)
            {
                Instantiate(constructedPrefab, transform.position, transform.rotation, transform);
            }
        }
    }
}
