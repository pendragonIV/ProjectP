using Things.Core.Infrastructure;

namespace Things.Game.Services.Cooking
{
    public interface ICookingService : IService
    {
        bool CanCook(string recipeId);
        bool Cook(string recipeId);
    }
}
