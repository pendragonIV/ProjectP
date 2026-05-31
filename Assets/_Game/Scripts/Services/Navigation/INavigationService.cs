using Things.Core.Infrastructure;
using UnityEngine;

namespace Things.Game.Services.Navigation
{
    public interface INavigationService : IService
    {
        void ShowGuideArrow(Vector3 targetPosition);
        void HideGuideArrow();
    }
}
