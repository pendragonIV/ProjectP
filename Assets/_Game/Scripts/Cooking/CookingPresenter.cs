using UnityEngine;
using Things.Core.Infrastructure;
using Things.Game.Services.Cooking;
using Unity.Netcode;

namespace Things.Game.Cooking
{
    public class CookingPresenter : NetworkBehaviour
    {
        [SerializeField] private ParticleSystem fireVfx;
        
        private ICookingService cookingService;

        private void Start()
        {
            ServiceLocator.TryGet(out cookingService);
        }

        public void OnCookRequested(string recipeId)
        {
            if (IsClient)
            {
                CookServerRpc(recipeId);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CookServerRpc(string recipeId)
        {
            if (cookingService != null && cookingService.Cook(recipeId))
            {
                PlayVfxClientRpc();
            }
        }

        [ClientRpc]
        private void PlayVfxClientRpc()
        {
            fireVfx?.Play();
        }
    }
}
