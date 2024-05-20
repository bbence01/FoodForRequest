using System.Numerics;
using FoodForRequest.Models;

namespace FoodForRequest.Data
{
    public interface IFoodUserRepository
    {
        FoodUser CreateFooduser(FoodUser foodUser);
        void DeleteFooduser(string foodUserid);
        IEnumerable<FoodUser> GetAllFoodusers();
        FoodUser GetFooduserById(string foodUserid);
        FoodUser UpdateFooduser(FoodUser foodUser);
        void AddRange(List<FoodUser> foodUsers);
    }
}