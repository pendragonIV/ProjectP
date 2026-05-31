namespace Things.Core.Infrastructure
{
    public interface IUpdatable
    {
        bool IsActive { get; }
        void OnUpdate(float deltaTime);
    }
}
