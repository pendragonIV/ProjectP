namespace Things.Game.AI.States
{
    public abstract class HelperStateBase
    {
        protected readonly HelperPresenter presenter;

        public HelperStateBase(HelperPresenter presenter)
        {
            this.presenter = presenter;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update(float deltaTime) { }
    }
}
