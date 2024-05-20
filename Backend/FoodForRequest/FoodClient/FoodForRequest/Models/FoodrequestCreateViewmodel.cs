using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FoodForRequest.Models
{
    public class FoodrequestCreateViewmodel
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        [MinLength(5)]
        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public int Payment { get; set; }



        [DefaultValue("No")]
        public string Deliveryoptions { get; set; }


        public string PictureURL { get; set; }

        public  List<string> Ingredients { get; set; }

    }
}
