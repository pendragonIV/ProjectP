using UnityEngine;

namespace Things.Game.Player.States
{
    public class PlayerSwimState : PlayerStateBase
    {
        public PlayerSwimState(PlayerPresenter presenter, PlayerStateMachine stateMachine) : base(presenter, stateMachine) { }

        public override void Enter()
        {
            presenter.AnimController.SetSwimming(true);
        }

        public override void Exit()
        {
            presenter.AnimController.SetSwimming(false);
        }

        public override void Update(float deltaTime)
        {
            // Logic bơi lội tương tự move state nhưng tốc độ chậm hơn
            if (presenter.MoveInput.sqrMagnitude > 0)
            {
                float speed = presenter.MovementConfig.BaseMaxSpeed * presenter.MovementConfig.SwimmingSpeedMultiplier;
                presenter.Controller.Move(presenter.MoveInput * speed * deltaTime);
            }
        }
    }
}
