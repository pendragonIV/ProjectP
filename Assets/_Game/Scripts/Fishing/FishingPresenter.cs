using UnityEngine;
using Things.Core.Infrastructure;
using Things.Game.Services.Fishing;
using Unity.Netcode;

namespace Things.Game.Fishing
{
    public class FishingPresenter : NetworkBehaviour
    {
        [SerializeField] private string spotId;
        [SerializeField] private ParticleSystem waterSplashVfx;
        
        private IFishingService fishingService;

        private void Start()
        {
            ServiceLocator.TryGet(out fishingService);
        }

        public void OnInteract()
        {
            if (IsClient)
            {
                InteractServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractServerRpc()
        {
            fishingService?.StartFishing(spotId);
            PlayVfxClientRpc();
        }

        [ClientRpc]
        private void PlayVfxClientRpc()
        {
            waterSplashVfx?.Play();
        }
    }
}
