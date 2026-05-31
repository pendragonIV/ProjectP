using UnityEngine;

namespace Things.Game.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem hitParticle;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int DeathHash = Animator.StringToHash("Death");

        public void UpdateMovementAnimation(float normalizedSpeed)
        {
            animator.SetFloat(SpeedHash, normalizedSpeed);
        }

        public void PlayHitEffect()
        {
            if (hitParticle != null) hitParticle.Play();
        }

        public void PlayDeathAnimation()
        {
            animator.SetTrigger(DeathHash);
        }
    }
}
