using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodClient.Models;
using FoodClient.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FoodClient.ViewModels
{
    public partial class FoodRequestUpdateViewModel : ObservableObject
    {
        private readonly RestService _restService;

        public FoodRequestUpdateViewModel(RestService restService)
        {
            _restService = restService;
            SaveCommand = new AsyncRelayCommand(SaveAsync);
            AddIngredientCommand = new RelayCommand(AddIngredient);
            RemoveIngredientCommand = new RelayCommand<string>(RemoveIngredient);
            Ingredients = new ObservableCollection<Ingredient>();
            NewIngredient = string.Empty;
        }

        [ObservableProperty]
        private FoodRequest foodRequest;

        [ObservableProperty]
        private ObservableCollection<Ingredient> ingredients;

        [ObservableProperty]
        private string newIngredient;

        public IAsyncRelayCommand SaveCommand { get; }
        public IRelayCommand AddIngredientCommand { get; }
        public IRelayCommand<string> RemoveIngredientCommand { get; }

        public async Task LoadFoodRequestAsync(string id)
        {
            FoodRequest = await _restService.GetSingleAsync<FoodRequest>($"foodrequest/{id}");
            Ingredients = new ObservableCollection<Ingredient>(await _restService.GetAsync<Ingredient>($"ingridient/GetIngredientsForRequest/{id}"));
        }

        private void AddIngredient()
        {
            var ni = new Ingredient
            {
                Name = NewIngredient,
                Description = NewIngredient
            };

            if (!string.IsNullOrWhiteSpace(NewIngredient) && !Ingredients.Any(i => i.Name == ni.Name))
            {
                Ingredients.Add(ni);
                NewIngredient = string.Empty;
            }
        }

        private void RemoveIngredient(string ingredientName)
        {
            var ingredientToRemove = Ingredients.FirstOrDefault(i => i.Name == ingredientName);
            if (ingredientToRemove != null)
            {
                Ingredients.Remove(ingredientToRemove);
            }
        }

        private async Task SaveAsync()
        {
            var ingredientNames = Ingredients.Select(i => i.Name).ToList();

            try
            {
                var foodRequestUpdate = new FoodrequestCreateViewmodel
                {
                    Name = FoodRequest.Name,
                    Description = FoodRequest.Description,
                    Payment = FoodRequest.Payment,
                    Deliveryoptions = FoodRequest.Deliveryoptions,
                    PictureURL = FoodRequest.PictureURL,
                    Ingredients = ingredientNames
                };

                // await _restService.PutAsync($"foodrequest/{FoodRequest.Id}", foodRequestUpdate);
                await _restService.PutAsync<FoodrequestCreateViewmodel>(foodRequestUpdate, $"foodrequest/{FoodRequest.Id}");

                await Shell.Current.GoToAsync("main");
            }
            catch (Exception ex)
            {
                // Handle error (e.g., show an error message)
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
