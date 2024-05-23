using System.ComponentModel.DataAnnotations.Schema;

namespace FoodForRequest.Models
{
    public class CommentCreatModel
    {
        public string Text { get; set; }



        public string RequestId { get; set; }

        public string ContractorId { get; set; }


    }
}
