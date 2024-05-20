using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodClient.Models


{
    public class Comment
    {

   
        public string Id { get; set; }


        public string Text { get; set; }



        public string RequestId { get; set; }

        public string ContractorId { get; set; }


     
        public virtual FoodRequest Request { get; set; }

     
        public virtual FoodUser User { get; set; }

        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

    }
}
