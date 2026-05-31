using Things.Game.Player.States;

namespace Things.Game.Player
{
    public class PlayerStateMachine
    {
        public PlayerStateBase CurrentState { get; private set; }

        public void Initialize(PlayerStateBase startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerStateBase newState)
        {
            if (CurrentState == newState) return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
        }

        public void Update(float deltaTime)
        {
            CurrentState?.Update(deltaTime);
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            CurrentState?.FixedUpdate(fixedDeltaTime);
        }
    }
}
