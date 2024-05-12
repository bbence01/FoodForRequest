using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodForRequest.Models
{
    public class RequestViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        [MinLength(5)]
        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public int Payment { get; set; }



        [DefaultValue(false)]
        public bool IsDone { get; set; }

        [DefaultValue(false)]
        public bool InProgress { get; set; }
        [DefaultValue("No")]
        public string Deliveryoptions { get; set; }


        public string RequestorId { get; set; }

        public string PictureURL { get; set; }

    }
}
