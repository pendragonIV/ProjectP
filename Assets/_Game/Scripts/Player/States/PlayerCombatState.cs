using UnityEngine;

namespace Things.Game.Player.States
{
    public class PlayerCombatState : PlayerStateBase
    {
        private float attackTimer;

        public PlayerCombatState(PlayerPresenter presenter, PlayerStateMachine stateMachine) : base(presenter, stateMachine) { }

        public override void Enter()
        {
            presenter.AnimController.TriggerAttack();
            attackTimer = 0.5f; // Attack Cooldown
        }

        public override void Update(float deltaTime)
        {
            attackTimer -= deltaTime;
            if (attackTimer <= 0)
            {
                stateMachine.ChangeState(presenter.IdleState);
            }
        }
    }
}
