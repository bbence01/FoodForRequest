using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiClient
{
    public static class FoodService
    {
        static HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5274/Foodrequest/") };

        public static async Task<List<FoodRequest>> GetFoodRequestsAsync()
        {
            var response = await client.GetAsync("FoodRequest");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<FoodRequest>>(content);
            }
            return new List<FoodRequest>();
        }

        public static async Task<FoodRequest> GetFoodRequestByIdAsync(string id)
        {
            var response = await client.GetAsync($"FoodRequest/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<FoodRequest>(content);
            }
            return null;
        }

        public static async Task<bool> UpdateFoodRequestAsync(FoodRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"FoodRequest/{request.Id}", content);
            return response.IsSuccessStatusCode;
        }
    }


}
