using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodForRequest.Models
{
    public class Ingredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }


        public string Name { get; set; }



        public string Description { get; set; }



        [ForeignKey(nameof(Models.FoodRequest))]
        public string? FoodId { get; set; }


        [NotMapped]
        [JsonIgnore]
        [ValidateNever]
      
        public virtual FoodRequest Requests { get; set; }


        public Ingredient()
        {
            this.Id = Guid.NewGuid().ToString();
        }



    }
}
