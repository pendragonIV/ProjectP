using UnityEngine;

namespace Things.Game.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int IsHarvestingHash = Animator.StringToHash("IsHarvesting");
        private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
        private static readonly int IsSwimmingHash = Animator.StringToHash("IsSwimming");

        public void SetSpeed(float speed)
        {
            animator.SetFloat(SpeedHash, speed);
        }

        public void SetHarvesting(bool isHarvesting)
        {
            animator.SetBool(IsHarvestingHash, isHarvesting);
        }

        public void TriggerAttack()
        {
            animator.SetTrigger(AttackTriggerHash);
        }

        public void SetSwimming(bool isSwimming)
        {
            animator.SetBool(IsSwimmingHash, isSwimming);
        }
    }
}
