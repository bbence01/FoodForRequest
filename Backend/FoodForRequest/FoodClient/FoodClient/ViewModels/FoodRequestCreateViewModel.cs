using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodClient.Models;
using FoodClient.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Maui.Storage;
using System.IO;

namespace FoodClient.ViewModels
{
    public partial class FoodRequestCreateViewModel : ObservableObject
    {
        private readonly RestService _restService;

        public FoodRequestCreateViewModel(RestService restService)
        {
            _restService = restService;
            SaveCommand = new AsyncRelayCommand(SaveAsync);
            AddIngredientCommand = new RelayCommand(AddIngredient);
            RemoveIngredientCommand = new RelayCommand<string>(RemoveIngredient);
            SelectImageCommand = new AsyncRelayCommand(SelectImageAsync);
            Ingredients = new ObservableCollection<string>();
            NewIngredient = string.Empty;
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private int payment;

        [ObservableProperty]
        private string deliveryOptions;

        [ObservableProperty]
        private string pictureURL;

        [ObservableProperty]
        private ObservableCollection<string> ingredients;

        [ObservableProperty]
        private string newIngredient;

        public IAsyncRelayCommand SaveCommand { get; }
        public IRelayCommand AddIngredientCommand { get; }
        public IRelayCommand<string> RemoveIngredientCommand { get; }
        public IAsyncRelayCommand SelectImageCommand { get; }

        private void AddIngredient()
        {
            if (!string.IsNullOrWhiteSpace(NewIngredient) && !Ingredients.Contains(NewIngredient))
            {
                Ingredients.Add(NewIngredient);
                NewIngredient = string.Empty;
            }
        }

        private void RemoveIngredient(string ingredientName)
        {
            if (Ingredients.Contains(ingredientName))
            {
                Ingredients.Remove(ingredientName);
            }
        }

        private async Task SaveAsync()
        {
            var foodRequestCreate = new FoodrequestCreateViewmodel
            {
                Name = Name,
                Description = Description,
                Payment = Payment,
                Deliveryoptions = DeliveryOptions,
                PictureURL = PictureURL,
                Ingredients = Ingredients.ToList()
            };

            try
            {
                await _restService.PostAsync(foodRequestCreate, "foodrequest/CreateFd");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private async Task SelectImageAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select an image"
                });

                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            var imageBytes = memoryStream.ToArray();
                            PictureURL = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
