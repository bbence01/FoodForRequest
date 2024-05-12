using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FoodForRequest.Models
{
    public class ProductViewModel
    {
        [StringLength(100)]
        [MinLength(1)]
        public string Name { get; set; }

        [StringLength(1000)]
        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        [DefaultValue(false)]
        public bool IsSold { get; set; }
    }
}
