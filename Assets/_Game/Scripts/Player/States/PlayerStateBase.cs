namespace Things.Game.Player.States
{
    public abstract class PlayerStateBase
    {
        protected readonly PlayerPresenter presenter;
        protected readonly PlayerStateMachine stateMachine;

        public PlayerStateBase(PlayerPresenter presenter, PlayerStateMachine stateMachine)
        {
            this.presenter = presenter;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update(float deltaTime) { }
        public virtual void FixedUpdate(float fixedDeltaTime) { }
    }
}
