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
    public class ListPageViewModel : BaseViewModel, IDisposable
    {
        ObservableCollection<GooglePlaceNearbyItem> placesToShow = new ObservableCollection<GooglePlaceNearbyItem>();
        public ObservableCollection<GooglePlaceNearbyItem> PlacesToShow
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
                var item = placesToShow.ToList().Find(x => x.PlaceId == arg.ToString());
                Uri uri = new Uri($"https://www.google.com/maps/dir/?api=1&destination={item.Name}&destination_place_id={item.PlaceId}&travelmode=transit");
                MessagingCenter.Send(this, "OpenUri", uri);
            });
            OnDetailClicked = new Command((arg) => {
                var item = placesToShow.ToList().Find(x => x.PlaceId == arg.ToString());
                Uri uri = new Uri($"https://www.google.com/maps/search/?api=1&query={item.Lat},{item.Lon}&query_place_id={item.PlaceId}");
                MessagingCenter.Send(this, "OpenUri", uri);
            });
            OnShareClicked = new Command((arg) => {
                var item = placesToShow.ToList().Find(x => x.PlaceId == arg.ToString());
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

            bool isSaveRemoveGoingOn = false;
            OnSaveClicked = new Command(async (arg) => {
                if (isSaveRemoveGoingOn)
                    return;

                isSaveRemoveGoingOn = true;

                var item = placesToShow.ToList().FirstOrDefault(x => x.PlaceId == arg.ToString());

                if (item == null)
                    return;
                
                var idx = placesToShow.IndexOf(item);
                item.IsSaved = true;
                if (idx != -1)
                    placesToShow[idx] = item;

                await App.MyPlacesDatabase.SaveItemAsync(item);
                isSaveRemoveGoingOn = false;
            });
            OnRemoveClicked = new Command(async (arg) => {
                if (isSaveRemoveGoingOn)
                    return;

                isSaveRemoveGoingOn = true;

                var item = placesToShow.ToList().FirstOrDefault(x => x.PlaceId == arg.ToString());

                if (item == null)
                    return;
                
                var idx = placesToShow.IndexOf(item);
                item.IsSaved = false;
                if (idx != -1)
                    placesToShow[idx] = item;

                await App.MyPlacesDatabase.DeleteItemAsync(item);
                isSaveRemoveGoingOn = false;
            });
        }

        public ICommand OnListClicked { get; }
        public ICommand OnItemClicked { get; }
        public ICommand OnRouteClicked { get; }
        public ICommand OnDetailClicked { get; }
        public ICommand OnShareClicked { get; }
        public ICommand OnSaveClicked { get; }
        public ICommand OnRemoveClicked { get; }

        public void Dispose() {}
    }
}
