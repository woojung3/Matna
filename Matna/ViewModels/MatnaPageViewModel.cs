using System;
using System.Windows.Input;
using Xamarin.Forms;
using Matna.Models;
using System.Collections.Generic;
using Matna.Utils.Restful;
using Matna.Helpers;

namespace Matna.ViewModels
{
    public class MatnaPageViewModel : BaseViewModel
    {
        List<GooglePlaceNearbyItem> placesToShow = new List<GooglePlaceNearbyItem>();
        public List<GooglePlaceNearbyItem> PlacesToShow
        {
            get
            {
                placesToShow.Sort((p1, p2) => (int)(-1.0 * (p1.RatingD - p2.RatingD))); // Desc order
                return placesToShow;
            }
            set
            {
                placesToShow = value;
                OnPropertyChanged("PlacesToShow");
                MessagingCenter.Send(this, "DrawPins", placesToShow);
            }
        }

        public MatnaPageViewModel()
        {
            // Properties (Commands) initilization
            OnSearchClicked = new Command(() => {
                MessagingCenter.Send(this, "DisplayAlert", "Search");
            });
            OnFilterClicked = new Command(() => {
                MessagingCenter.Send(this, "DisplayAlert", "Filter");
            });
            OnListClicked = new Command(() => {
                MessagingCenter.Send(this, "ShowList");
            });
            OnLoadClicked = new Command(() => {
                MessagingCenter.Send(this, "DisplayAlert", "Load");
            });

            // Initialize Data
            MessagingCenter.Subscribe<MatnaPage>(this, "CameraChanged", (sender) =>
            {
                LoadPlaces();
            });
        }

        private async void LoadPlaces()
        {
            List<string> types = new List<string>(){
                "restaurant"
            };
            List<GooglePlaceNearbyItem> rtn = await Restful.Inst.GoogleMapsPlaceNearbySearch(
                PropertiesDictionary.Latitude, 
                PropertiesDictionary.Longitude, 
                PropertiesDictionary.Radius, 
                types, 
                new List<string>(){  }
            );

            PlacesToShow = rtn;
            MessagingCenter.Send(this, "DisplayAlert", rtn.Count);
        }

        public ICommand OnSearchClicked { get; }
        public ICommand OnFilterClicked { get; }
        public ICommand OnListClicked { get; }
        public ICommand OnLoadClicked { get; }
    }
}
