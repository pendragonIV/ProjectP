using UnityEngine;

namespace Things.Game.Player.States
{
    public class PlayerIdleState : PlayerStateBase
    {
        public PlayerIdleState(PlayerPresenter presenter, PlayerStateMachine stateMachine) : base(presenter, stateMachine) { }

        public override void Enter()
        {
            presenter.AnimController.SetSpeed(0f);
        }

        public override void Update(float deltaTime)
        {
            if (presenter.MoveInput.sqrMagnitude > presenter.MovementConfig.MovementInputThreshold)
            {
                stateMachine.ChangeState(presenter.MoveState);
            }
            
            // Note: Thêm logic kiểm tra Attack hoặc Harvest ở đây sau này
        }
    }
}
