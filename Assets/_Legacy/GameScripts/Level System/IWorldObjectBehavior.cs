namespace Things
{
    public interface IWorldObjectBehavior
    {
        public void Initialise(WorldBehavior worldBehavior);
        public void Unload();
    }
}