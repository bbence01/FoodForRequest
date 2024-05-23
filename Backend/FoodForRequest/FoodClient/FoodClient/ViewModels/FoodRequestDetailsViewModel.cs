using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            ChooseOfferCommand = new AsyncRelayCommand<string>(ChooseOfferAsync);
        }

        [ObservableProperty]
        private string _currentUserId;

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

        [ObservableProperty]
        private bool isRequestor;

        [ObservableProperty]
        private bool canCompleteRequest;

        [ObservableProperty]
        private bool canCancelRequest;

        [ObservableProperty]
        private string newCommentText; 

        public IAsyncRelayCommand<string> ChooseOfferCommand { get; }

        public async Task LoadFoodRequestDetailsAsync(string id)
        {
            _currentUserId = await SecureStorage.GetAsync("UserId");

            FoodRequest = await _restService.GetSingleAsync<FoodRequest>($"foodrequest/{id}");
            Ingredients = new ObservableCollection<Ingredient>(await _restService.GetAsync<Ingredient>($"ingridient/GetIngredientsForRequest/{id}"));
            Offers = new ObservableCollection<Offer>(await _restService.GetAsync<Offer>($"offer/GetOffersForRequest/{id}"));
            Comments = new ObservableCollection<Comment>(await _restService.GetAsync<Comment>($"comment/GetCommentsForRequest/{id}"));

            var reversedOffersList = new ObservableCollection<Comment>(Comments.Reverse());

            Comments.Clear();
            foreach (var coment in reversedOffersList)
            {
                Comments.Add(coment);
            }


            IsRequestor = IsRequestorCheck();
            CanCompleteRequest = CanCompleteRequestCheck();
            CanCancelRequest = CanCancelRequestCheck();

            if (FoodRequest.PictureURL.StartsWith("data:image"))
            {
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(FoodRequest.PictureURL.Split(',')[1]));
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

        private async Task ChooseOfferAsync(string offerId)
        {
            try
            {
                await _restService.PutAsync($"foodrequest/ChooseOffer?foodId={FoodRequest.Id}&offerId={offerId}", new { });
                await LoadFoodRequestDetailsAsync(FoodRequest.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private bool IsRequestorCheck()
        {
            return FoodRequest?.RequestorId == _currentUserId;
        }

        private bool CanCompleteRequestCheck()
        {
            var chosenOffer = Offers.FirstOrDefault(o => o.Choosen);
            return chosenOffer != null && chosenOffer.ContractorId == _currentUserId;
        }

        private bool CanCancelRequestCheck()
        {
            return FoodRequest?.RequestorId == _currentUserId;
        }

        [RelayCommand]
        private async Task NavigateToUpdateAsync()
        {
            var updateViewModel = new FoodRequestUpdateViewModel(_restService);
            await updateViewModel.LoadFoodRequestAsync(FoodRequest.Id);
            var updatePage = new Views.FoodRequestUpdatePage { BindingContext = updateViewModel };
            await Shell.Current.Navigation.PushAsync(updatePage);
        }

        [RelayCommand]

        private async Task CompleteRequestAsync()
        {
            try
            {
                await _restService.PutAsync($"foodrequest/CompleteRequest?id={FoodRequest.Id}", new { });
                await LoadFoodRequestDetailsAsync(FoodRequest.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        [RelayCommand]

        private async Task CancelRequestAsync()
        {
            try
            {
                await _restService.PutAsync($"foodrequest/CancelRequest?id={FoodRequest.Id}", new { });
                await LoadFoodRequestDetailsAsync(FoodRequest.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        [RelayCommand]

        private async Task ResubmitRequestAsync()
        {
            try
            {
                await _restService.PutAsync($"foodrequest/Resubmit?id={FoodRequest.Id}", new { });
                await LoadFoodRequestDetailsAsync(FoodRequest.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        [RelayCommand]

        private async Task AddOfferAsync()
        {
            try
            {
                await _restService.PostIdAsync($"offer/AddOffer?fid={FoodRequest.Id}&cid={_currentUserId}", new { });
                await LoadFoodRequestDetailsAsync(FoodRequest.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        [RelayCommand]

        private async Task PostCommentAsync()
        {
            try
            {
                var comment = new CommentCreatModel
                {
                    RequestId = FoodRequest.Id,
                    ContractorId = _currentUserId,
                    Text = NewCommentText
                };

                await _restService.PostAsync( comment, "comment");
                NewCommentText = string.Empty;
                await LoadFoodRequestDetailsAsync(FoodRequest.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
