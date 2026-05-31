using Things.Game.AI.States;

namespace Things.Game.AI
{
    public class HelperStateMachine
    {
        public HelperStateBase CurrentState { get; private set; }
        private readonly HelperPresenter presenter;

        public HelperStateMachine(HelperPresenter presenter)
        {
            this.presenter = presenter;
        }

        public void ChangeState(HelperStateBase newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
        }

        public void Update(float deltaTime)
        {
            CurrentState?.Update(deltaTime);
        }
    }
}
