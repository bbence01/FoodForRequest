using CommunityToolkit.Mvvm.ComponentModel;
using FoodClient.Models;
using FoodClient.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.IO;

namespace FoodClient.ViewModels
{
    public partial class FoodRequestDetailsViewModel : ObservableObject
    {
        private readonly RestService _restService;

        public FoodRequestDetailsViewModel(RestService restService)
        {
            _restService = restService;
        }

        [ObservableProperty]
        private FoodRequest foodRequest;

        [ObservableProperty]
        private ObservableCollection<Ingredient> ingredients;

        [ObservableProperty]
        private ObservableCollection<Offer> offers;

        [ObservableProperty]
        private ObservableCollection<Comment> comments;

        [ObservableProperty]
        private string pictureSource;

        [ObservableProperty]
        private Image pictureIMG;


        [ObservableProperty]
        private bool base64;

        [ObservableProperty]
        private bool webimg;

        public async Task LoadFoodRequestDetailsAsync(string id)
        {
            FoodRequest = await _restService.GetSingleAsync<FoodRequest>($"foodrequest/{id}");
            Ingredients = new ObservableCollection<Ingredient>(await _restService.GetAsync<Ingredient>($"ingridient/GetIngredientsForRequest/{id}"));
            Offers = new ObservableCollection<Offer>(await _restService.GetAsync<Offer>($"offer/GetOffersForRequest/{id}"));
            Comments = new ObservableCollection<Comment>(await _restService.GetAsync<Comment>($"comment/GetCommentsForRequest/{id}"));


            if (FoodRequest.PictureURL.StartsWith("data:image"))
            {
                  //PictureSource = FoodRequest.PictureURL.Split(',')[1];


                MemoryStream stream = new MemoryStream(Convert.FromBase64String(FoodRequest.PictureURL.Split(',')[1]));
                //ImageSource image = ImageSource.FromStream(() => stream);


                PictureIMG = new Image
                {
                    Source = ImageSource.FromStream(() => stream)
                };

                base64 = true;
                webimg = false;



            }
            else
            {
                PictureSource = FoodRequest.PictureURL.StartsWith("http") ? FoodRequest.PictureURL : $"file://{FoodRequest.PictureURL}";
                base64 = false;
                webimg = true;
            }
        }


    }
}
