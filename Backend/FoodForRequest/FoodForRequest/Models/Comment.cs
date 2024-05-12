using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodForRequest.Models


{
    public class Comment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }


        public string Text { get; set; }



        [ForeignKey(nameof(Models.FoodRequest))]
        public string RequestId { get; set; }

        [ForeignKey(nameof(Models.FoodUser))]
        public string ContractorId { get; set; }


        [NotMapped]
        [JsonIgnore]
        [ValidateNever]
        public virtual FoodRequest Request { get; set; }

        [NotMapped]
        [JsonIgnore]
        [ValidateNever]
        public virtual FoodUser User { get; set; }

        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

    }
}
