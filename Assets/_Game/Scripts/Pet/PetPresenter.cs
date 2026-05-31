using UnityEngine;
using Things.Core.Infrastructure;

namespace Things.Game.Pet
{
    public class PetPresenter : MonoBehaviour, IUpdatable
    {
        [SerializeField] private ParticleSystem happyVfx;
        [SerializeField] private UnityEngine.AI.NavMeshAgent navMeshAgent;
        [SerializeField] private Transform playerTransform; // Sẽ được inject hoặc tự tìm
        
        public bool IsActive => gameObject.activeInHierarchy;

        private void OnEnable()
        {
            UpdateManager.Register(this, UpdatePhase.Normal);
            if (playerTransform == null)
            {
                var player = FindFirstObjectByType<Things.Game.Player.PlayerPresenter>();
                if (player != null) playerTransform = player.transform;
            }
        }

        private void OnDisable()
        {
            UpdateManager.Unregister(this);
        }

        public void OnUpdate(float deltaTime)
        {
            if (playerTransform != null && navMeshAgent != null)
            {
                navMeshAgent.SetDestination(playerTransform.position);
            }
        }

        public void PlayHappyAnimation()
        {
            happyVfx?.Play();
        }
    }
}
