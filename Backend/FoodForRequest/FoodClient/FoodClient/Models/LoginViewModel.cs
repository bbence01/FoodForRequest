using System.Text.Json.Serialization;


namespace FoodClient.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }


        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
