using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodClient.Models
{
    public class FoodUser : IdentityUser
    {

        public string FoodUserName { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Founds { get; set; }


       
        public virtual List<FoodRequest> FoodRequests { get; set; }

     
        public virtual List<Offer> Offers { get; set; }

        
        public virtual List<Comment> Comments { get; set; }
    }
}
