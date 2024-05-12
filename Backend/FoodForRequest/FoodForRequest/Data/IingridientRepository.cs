using FoodForRequest.Models;

namespace FoodForRequest.Data
{
    public interface IingridientRepository
    {

        void Create(Ingredient ingredient);
        void Delete(Ingredient ingredient);
        void Delete(string id);
        IEnumerable<Ingredient> GetAll();
        Ingredient GetOne(string id);
        List<Ingredient> GetIngredientsForRequest(string id);
    }
}
