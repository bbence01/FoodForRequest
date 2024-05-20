using FoodForRequest.Models;

namespace FoodForRequest.Data
{
    public class FoodrequestRepository : IFoodRequestRepository
    {
        ApplicationDbContext context;

        public FoodrequestRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public FoodRequest Create(FoodRequest product)
        {
            context.Foodrequests.Add(product);
            context.SaveChanges();
            return product;
        }

        public IEnumerable<FoodRequest> GetAll()
        {
            return context.Foodrequests.ToList();
        }

        public FoodRequest GetOne(string id)
        {
            return context.Foodrequests.FirstOrDefault(p => p.Id == id);
        }

        public FoodRequest GetOneName(string id)
        {
            return context.Foodrequests.FirstOrDefault(p => p.Name == id);
        }
        public FoodRequest GetOneDescription(string id)
        {
            return context.Foodrequests.FirstOrDefault(p => p.Description == id);
        }


        public void Update(FoodRequest product)
        {
            FoodRequest old = this.GetOne(product.Id);
            old.IsDone = product.IsDone;
            old.Description = product.Description;
            old.Name = product.Name;
            this.context.SaveChanges();
        }
        /*
        public IEnumerable<FoodRequest> GetPurchasedItems(string userId)
        {
            return this.GetAll().Where(p => p.IsDone && p.HighestBid != null && p.HighestBid.ContractorId == userId);
        }*/

        public void Delete(FoodRequest product)
        {
            this.context.Foodrequests.Remove(product);
            this.context.SaveChanges();
        }

        public IEnumerable<FoodRequest> SeeAcceptedOffers(string userId)
        {
            // return this.GetAll().Where(p => p.IsDone && p.Contractor != null && p.Contractor.Id == userId);

            // return this.GetAll().Where(p => p.Offers.Where(x => x.Choosen == true && x.ContractorId == userId ) );

            return this.GetAll().Where(p => p.Offers.Any(x => x.Choosen == true && x.ContractorId == userId));
        }

    }
}
