using FoodClient.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodForRequestApp.Services
{
    public static class AuthService
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<LoginResponse> LoginAsync(LoginViewModel loginData)
        {
            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5274/Auth", content);

            if (response.IsSuccessStatusCode)
            {

                 var responseContent = await response.Content.ReadAsStringAsync();



                return JsonSerializer.Deserialize<LoginResponse>(responseContent);




            }
            return null;
        }

        public static async Task<bool> RegisterAsync(RegisterViewModel registerData)
        {
            var json = JsonSerializer.Serialize(registerData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://localhost:5274/Auth/register", content);

            return response.IsSuccessStatusCode;
        }

        public static async Task LogoutAsync()
        {
            var client = await GetAuthenticatedClientAsync();
            var rsult = await client.PostAsync("http://localhost:5274/Auth/logout", null);

            

        }


        public static async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            var token = await SecureStorage.GetAsync("AuthToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }


    }
}
