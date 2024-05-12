using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodForRequest.Models
{
    public class OfferViewModel
    {

        public string Id { get; set; }


        public bool Choosen { get; set; }

        [ForeignKey(nameof(Models.FoodRequest))]
        public string FoodId { get; set; }


       
        public string ContractorId { get; set; }

    }
}
