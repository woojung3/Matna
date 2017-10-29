using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Matna.Models;
using Matna.Resources.Localize;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Xamarin.Forms;

namespace Matna.ViewModels
{
    public class ListPageViewModel : BaseViewModel
    {
        List<GooglePlaceNearbyItem> placesToShow = new List<GooglePlaceNearbyItem>();
        public List<GooglePlaceNearbyItem> PlacesToShow
        {
            get
            {
                return placesToShow;
            }
            set
            {
                placesToShow = value;
                OnPropertyChanged("PlacesToShow");
            }
        }

        public ListPageViewModel()
        {
            // Properties (Commands) initilization
            OnListClicked = new Command(() => {
                MessagingCenter.Send(this, "HideList");
            });

            OnRouteClicked = new Command((arg) => {
                var item = placesToShow.Find(x => x.PlaceId == arg.ToString());
                Uri uri = new Uri($"https://www.google.com/maps/dir/?api=1&destination={item.Name}&destination_place_id={item.PlaceId}&travelmode=transit");
                MessagingCenter.Send(this, "OpenUri", uri);
            });
            OnDetailClicked = new Command((arg) => {
                var item = placesToShow.Find(x => x.PlaceId == arg.ToString());
                Uri uri = new Uri($"https://www.google.com/maps/search/?api=1&query={item.Lat},{item.Lon}&query_place_id={item.PlaceId}");
                MessagingCenter.Send(this, "OpenUri", uri);
            });
            OnShareClicked = new Command((arg) => {
                var item = placesToShow.Find(x => x.PlaceId == arg.ToString());
                CrossShare.Current.Share(
                    new ShareMessage
                    {
                        Title = item.Name,
                        Text = item.Vicinity,
                        //Url = urlShareLabel.Text  // Not necessary
                    },
                    new ShareOptions
                    {
                        ChooserTitle = AppResources.Share,
                        ExcludedUIActivityTypes = new[] { ShareUIActivityType.PostToFacebook }
                    }
                );
            });
            OnSaveClicked = new Command(async (arg) => {
                var item = placesToShow.Find(x => x.PlaceId == arg.ToString());
                item.IsSaved = true;
                MessagingCenter.Send(this, "DisplayAlert", AppResources.Saved);
                await App.MyPlacesDatabase.SaveItemAsync(item);
            });
            OnRemoveClicked = new Command(async (arg) => {
                var item = placesToShow.Find(x => x.PlaceId == arg.ToString());
                item.IsSaved = false;
                MessagingCenter.Send(this, "DisplayAlert", AppResources.Removed);
                await App.MyPlacesDatabase.DeleteItemAsync(item);
            });
        }

        public ICommand OnListClicked { get; }
        public ICommand OnItemClicked { get; }
        public ICommand OnRouteClicked { get; }
        public ICommand OnDetailClicked { get; }
        public ICommand OnShareClicked { get; }
        public ICommand OnSaveClicked { get; }
        public ICommand OnRemoveClicked { get; }
    }
}
