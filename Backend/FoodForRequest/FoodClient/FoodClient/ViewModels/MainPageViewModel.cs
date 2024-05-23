using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodClient.Models;
using FoodClient.Services;
using FoodClient.Views;
using FoodForRequestApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FoodClient.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        RestService _restService = new RestService("http://localhost:5274/");
        private CancellationTokenSource _cts;

        public MainPageViewModel()
        {
            FoodRequests = new ObservableCollection<FoodRequest>();
            AllIngredients = new ObservableCollection<string>();
            SelectedIngredients = new ObservableCollection<string>();
            LoadFoodRequestsAsync();
            SearchFoodRequestsCommand = new AsyncRelayCommand<string>(SearchFoodRequestsAsync);

            AddIngredientToFilterCommand = new RelayCommand<string>(AddIngredientToFilter);
            RemoveIngredientFromFilterCommand = new RelayCommand<string>(RemoveIngredientFromFilter);

            LoadAllIngredientsAsync();
                        ErrorMessage = string.Empty;
            IsBusy = false;
        }



        [ObservableProperty]
        private ObservableCollection<FoodRequest> foodRequests;

        [ObservableProperty]
        private FoodRequest selectedFoodRequest;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private string filterQuery;

        [ObservableProperty]
        private ObservableCollection<string> allIngredients;

        [ObservableProperty]
        private ObservableCollection<string> selectedIngredients;

        private List<string> FilterIngredients => filterQuery?.Split(',').Select(i => i.Trim()).ToList() ?? new List<string>();

        public IRelayCommand<string> AddIngredientToFilterCommand { get; }
        public IRelayCommand<string> RemoveIngredientFromFilterCommand { get; }
        public IAsyncRelayCommand<string> SearchFoodRequestsCommand { get; }

        [RelayCommand]
        private async Task Logout()
        {
            try
            {
                await AuthService.LogoutAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during logout: {ex.Message}");
            }
            finally
            {
                SecureStorage.Remove("AuthToken");
                SecureStorage.Remove("UserId");

                await Shell.Current.GoToAsync("LoginPage");
            }
        }

        [RelayCommand]
         public async Task LoadFoodRequestsAsync()
        {
            var client = await AuthService.GetAuthenticatedClientAsync();


            try
            {
                IsBusy = true;
                var items = await _restService.GetAsync<FoodRequest>("foodrequest/GetAll");
                FoodRequests.Clear();
                foreach (var item in items)
                {
                    FoodRequests.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine(ErrorMessage);
            }
            finally
            {
                IsBusy = false;
            }
        }
    

        public void AddIngredientToFilter(string ingredient)
        {
            if (!SelectedIngredients.Contains(ingredient))
            {
                SelectedIngredients.Add(ingredient);
            }
        }

        public void RemoveIngredientFromFilter(string ingredient)
        {
            if (SelectedIngredients.Contains(ingredient))
            {
                SelectedIngredients.Remove(ingredient);
            }
        }
        /*
        private async Task SearchFoodRequestsAsync(string query)
        {
            _cts?.Cancel(); 
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                IsBusy = true;
                var items = await _restService.SearchAsync<FoodRequest>("foodrequest/Search", query, token);
                FoodRequests.Clear();
                foreach (var item in items)
                {
                    FoodRequests.Add(item);
                }
            }
            catch (OperationCanceledException)
            {
                ErrorMessage = "Operation was canceled.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine(ErrorMessage);
            }
            finally
            {
                IsBusy = false;
            }
        }
        */
        private async Task SearchFoodRequestsAsync(string query)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                IsBusy = true;

                var items = new ObservableCollection<FoodRequest>(); ;


                foreach (var item in FoodRequests)
                {
                    

                        items.Add(item);
                    
                }


                FoodRequests.Clear();
                foreach (var item in items)
                {
                    if (item.Description.Contains(query) || item.Name.Contains(query))
                    {

                        FoodRequests.Add(item);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                ErrorMessage = "Operation was canceled.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine(ErrorMessage);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public async Task LoadAllIngredientsAsync()
        {
            _cts?.Cancel(); // Cancel any ongoing task
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                IsBusy = true;
                var ingredients = await _restService.GetAsync<Ingredient>("ingridient/GetDistinct") ;
                AllIngredients.Clear();
                foreach (var ingredient in ingredients)
                {
                    AllIngredients.Add(ingredient.Name);
                }
            }
            catch (OperationCanceledException)
            {
                ErrorMessage = "Operation was canceled.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine(ErrorMessage);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]

        private async Task FilterFoodRequestsAsync()
        {
            _cts?.Cancel(); // Cancel any ongoing task
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                IsBusy = true;
                var ingredientQuery = string.Join(",", SelectedIngredients);
                if (SelectedIngredients.Count > 0)
                {
                    var items = await _restService.FilterByIngredientsAsync<FoodRequest>("foodrequest/FilterByIngredients", ingredientQuery, token);
                    FoodRequests.Clear();
                    foreach (var item in items)
                    {
                        FoodRequests.Add(item);
                    }
                }
                else
                {
                    LoadFoodRequestsAsync();

                }


            }
            catch (OperationCanceledException)
            {
                ErrorMessage = "Operation was canceled.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine(ErrorMessage);
            }
            finally
            {
                IsBusy = false;
            }


        }

        [RelayCommand]
        private async Task NavigateToCreateAsync()
        {
            var createViewModel = new FoodRequestCreateViewModel(_restService);
            var createPage = new FoodRequestCreatePage { BindingContext = createViewModel };
            await Shell.Current.Navigation.PushAsync(createPage);
        }


        [RelayCommand]
        private async Task NavigateToUserRequestsAsync()
        {
            var userId = await SecureStorage.GetAsync("UserId");
            var userRequestsViewModel = new FoodRequestForUserViewModel(userId);
            var userRequestsPage = new FoodRequestForUserPage { BindingContext = userRequestsViewModel };
            await Shell.Current.Navigation.PushAsync(userRequestsPage);


        }

      

    }
}
