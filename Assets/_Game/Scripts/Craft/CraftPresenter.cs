using UnityEngine;
using Things.Core.Infrastructure;
using Things.Game.Services.Craft;
using Unity.Netcode;

namespace Things.Game.Craft
{
    public class CraftPresenter : NetworkBehaviour
    {
        [SerializeField] private ParticleSystem craftVfx;
        
        private ICraftService craftService;

        private void Start()
        {
            ServiceLocator.TryGet(out craftService);
        }

        public void OnCraftRequested(string recipeId)
        {
            if (IsClient)
            {
                CraftServerRpc(recipeId);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CraftServerRpc(string recipeId)
        {
            if (craftService != null && craftService.Craft(recipeId))
            {
                PlayVfxClientRpc();
            }
        }

        [ClientRpc]
        private void PlayVfxClientRpc()
        {
            craftVfx?.Play();
        }
    }
}
