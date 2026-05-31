using UnityEngine;

namespace Things.Game.Player.States
{
    public class PlayerMoveState : PlayerStateBase
    {
        private Vector3 currentVelocity;

        public PlayerMoveState(PlayerPresenter presenter, PlayerStateMachine stateMachine) : base(presenter, stateMachine) { }

        public override void Update(float deltaTime)
        {
            if (presenter.MoveInput.sqrMagnitude <= presenter.MovementConfig.MovementInputThreshold)
            {
                stateMachine.ChangeState(presenter.IdleState);
                return;
            }

            // Xoay character
            if (presenter.MoveInput != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(presenter.MoveInput);
                presenter.transform.rotation = Quaternion.Slerp(
                    presenter.transform.rotation, 
                    targetRotation, 
                    presenter.MovementConfig.RotationLerpSpeed * deltaTime);
            }

            // Di chuyển
            float speed = presenter.MovementConfig.BaseMaxSpeed;
            presenter.Controller.Move(presenter.MoveInput * speed * deltaTime);
            
            presenter.AnimController.SetSpeed(speed);
        }
    }
}
