using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodClient.Models
{
    public class Offer
    {
      
        public string Id { get; set; }

     
        public bool Choosen { get; set; }





       
        public string FoodId { get; set; }

        
        public string ContractorId { get; set; }





       

        public virtual FoodRequest Request { get; set; }

        
        public virtual FoodUser User { get; set; }

        public Offer()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
