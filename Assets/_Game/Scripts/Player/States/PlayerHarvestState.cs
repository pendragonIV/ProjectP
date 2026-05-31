using UnityEngine;

namespace Things.Game.Player.States
{
    public class PlayerHarvestState : PlayerStateBase
    {
        private float timer;

        public PlayerHarvestState(PlayerPresenter presenter, PlayerStateMachine stateMachine) : base(presenter, stateMachine) { }

        public override void Enter()
        {
            presenter.AnimController.SetHarvesting(true);
            timer = 0f;
        }

        public override void Exit()
        {
            presenter.AnimController.SetHarvesting(false);
        }

        public override void Update(float deltaTime)
        {
            // Tạm thời giả lập thời gian chặt cây
            timer += deltaTime;
            if (timer >= 1f) // ToolSwingDuration
            {
                stateMachine.ChangeState(presenter.IdleState);
            }
        }
    }
}
