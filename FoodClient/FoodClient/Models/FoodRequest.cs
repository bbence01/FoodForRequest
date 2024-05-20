using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodClient.Models
{
    public class FoodRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Payment { get; set; }
        public bool IsDone { get; set; }
        public bool InProgress { get; set; }
        public string Deliveryoptions { get; set; }
        public string RequestorId { get; set; }
        public string PictureURL { get; set; }

        public FoodRequest()
        {

        }
    }
}
