
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FoodClient.Models
{
    public class Ingredient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FoodId { get; set; }
    }
}