using FoodForRequest.Models;

namespace FoodForRequest.Data
{
    public interface IFoodRequestRepository
    {
        FoodRequest Create(FoodRequest foodrequest);
        void Delete(FoodRequest foodrequest);
        IEnumerable<FoodRequest> GetAll();
        FoodRequest GetOne(string id);
        void Update(FoodRequest foodrequest);


        IEnumerable<FoodRequest> SeeAcceptedOffers(string userId);

        FoodRequest GetOneName(string id);
        FoodRequest GetOneDescription(string id);

    }
}