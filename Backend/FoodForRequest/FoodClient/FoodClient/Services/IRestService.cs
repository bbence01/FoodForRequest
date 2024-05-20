using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodClient.Services
{
    public interface IRestService
    {
        Task<List<T>> SearchAsync<T>(string endpoint, string query, CancellationToken token = default);
        Task<List<T>> FilterByIngredientsAsync<T>(string endpoint, string ingredients, CancellationToken token = default);
        Task<List<T>> GetAsync<T>(string endpoint);
        List<T> Get<T>(string endpoint);
        Task<T> GetSingleAsync<T>(string endpoint);
        T GetSingle<T>(string endpoint);
        Task<T> GetAsync<T>(int id, string endpoint);
        T Get<T>(int id, string endpoint);
        Task PostAsync<T>(T item, string endpoint);
        void Post<T>(T item, string endpoint);
        Task DeleteAsync(int id, string endpoint);
        void Delete(int id, string endpoint);
        Task PutAsync<T>(T item, string endpoint);
        void Put<T>(T item, string endpoint);
    }
}
