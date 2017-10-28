using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matna.Helpers;
using Matna.Models;
using Matna.Resources.Localize;
using Matna.ViewModels;
using Matna.Views;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Matna
{
    public partial class MatnaPage : ContentPage
    {
        public MatnaPage()
        {
            InitializeComponent();

            #region Initial Camera Settings
            if (Application.Current.Properties.ContainsKey("Latitude") && Application.Current.Properties.ContainsKey("Longitude") && Application.Current.Properties.ContainsKey("Zoom"))
            {
                double? lat = Application.Current.Properties["Latitude"] as double?;
                double? lon = Application.Current.Properties["Longitude"] as double?;
                double? zoom = Application.Current.Properties["Zoom"] as double?;
                if (lat == null) lat = 37.532600;
                if (lon == null) lon = 127.024612;
                if (zoom == null) zoom = 10.0;

                map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position((double)lat, (double)lon), (double)zoom);
            }
            map.CameraChanged += CameraChanged;
            #endregion Initial Camera Settings

            #region Codes for MessageCenter 
            MessagingCenter.Subscribe<MatnaPageViewModel, string>(this, "DisplayAlert", (sender, str) =>
            {
                DisplayAlert(AppResources.Matna, str, AppResources.OK);
            });

            MessagingCenter.Subscribe<MatnaPageViewModel>(this, "ShowList", async (sender) =>
            {
                if (Navigation.ModalStack.Count == 0)
                {
                    var page = new ListPage();
                    await Navigation.PushModalAsync(page);
                }
            });

            MessagingCenter.Subscribe<MatnaPageViewModel, List<GooglePlaceNearbyItem>>(this, "DrawPins", (sender, items) =>
            {
                DrawFromItems(items);
            });
            #endregion Codes for MessageCenter
        }

        private void DrawFromItems(List<GooglePlaceNearbyItem> items)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    map.Pins.Clear();
                }
                catch (Exception ex)
                {
                    // Binding Pin problem?
                    System.Diagnostics.Debug.WriteLine("Matna>> Unmanaged descriptor could happen on clear / Check Xamarin.Forms.GoogleMaps issue #162");
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
            foreach (GooglePlaceNearbyItem item in items)
            {
                if (item == null)
                    continue;

                AddPin(item);
            }
        }

        void AddPin(GooglePlaceNearbyItem item)
        {
            if (item.PlaceId == null || item.Geometry == null)
                return;

            if (item.Name == null)
                item.Name = AppResources.NoName;

            Device.BeginInvokeOnMainThread(() =>
            {
                Pin pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(item.Geometry.Location.Lat, item.Geometry.Location.Lon),
                    Label = item.Name,
                    Tag = item
                };
                map.Pins.Add(pin);
            });
        }

        public volatile bool isCameraStable = true;
        protected virtual async void CameraChanged(object sender, CameraChangedEventArgs e)
        {
            if (isCameraStable)
            {
                var lat = map.CameraPosition.Target.Latitude;
                var lon = map.CameraPosition.Target.Longitude;
                var zoom = map.CameraPosition.Zoom;

                map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(lat, lon), zoom);

                PropertiesDictionary.Latitude = lat;
                PropertiesDictionary.Longitude = lon;
                PropertiesDictionary.Zoom = zoom;
                PropertiesDictionary.Radius = map.VisibleRegion.Radius.Meters;

                //var placeItems = await App.PlacesDatabase.GetItemsCameraCenterAsync();
                //PlaceItem placeItem;
                //if (placeItems.Any())
                //    placeItem = new PlaceItem() { ID = placeItems[0].ID,  Name = "CameraCenter", Latitude = lat, Longitude = lon, Zoom = zoom };
                //else
                //    placeItem = new PlaceItem() { Name = "CameraCenter", Latitude = lat, Longitude = lon, Zoom = zoom };
                //await App.PlacesDatabase.SaveItemAsync(placeItem);

                MessagingCenter.Send(this, "CameraChanged");

                isCameraStable = false;
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                isCameraStable = true;
            }
        }
    }
}
