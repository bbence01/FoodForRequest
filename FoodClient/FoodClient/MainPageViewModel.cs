using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodClient.Models;
using System.Numerics;

namespace FoodClient
{
    public partial class MainPageViewModel:ObservableObject
    {
        RestService _restService = new RestService("http://localhost:5274/api/");

        [ObservableProperty]
         ObservableCollection<FoodRequest> foodRequests;

        [ObservableProperty]
        private FoodRequest selectedFoodRequest;



        [ObservableProperty]
        bool isBusy;

        public MainPageViewModel()
        {
            FoodRequests = new ObservableCollection<FoodRequest>();
            GetActorsAsync();
        }

        async Task GetActorsAsync()
        {
            IsBusy = true;
            FoodRequests.Clear();
            var list = await _restService.GetAsync<FoodRequest>("Foodrequest/GetAll"); ;

            list.ForEach(x => foodRequests.Add(x));
            IsBusy = false;
        }

       
    }
}
