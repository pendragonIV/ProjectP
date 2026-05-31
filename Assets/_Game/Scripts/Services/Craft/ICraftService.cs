using Things.Core.Infrastructure;

namespace Things.Game.Services.Craft
{
    public interface ICraftService : IService
    {
        bool CanCraft(string recipeId);
        bool Craft(string recipeId);
    }
}
