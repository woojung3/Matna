using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matna.Helpers;
using Matna.Models;
using Matna.Resources.Localize;
using Matna.Utils.Restful;
using Matna.ViewModels;
using Matna.Views;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Matna
{
    public partial class MatnaPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();

            #region Codes for MessageCenter 
            MessagingCenter.Unsubscribe<MatnaPageViewModel, string>(this, "DisplayAlert");
            MessagingCenter.Subscribe<MatnaPageViewModel, string>(this, "DisplayAlert", (sender, str) =>
            {
                DisplayAlert(AppResources.Matna, str, AppResources.OK);
            });

            MessagingCenter.Unsubscribe<MatnaPageViewModel, Uri>(this, "OpenUri");
            MessagingCenter.Subscribe<MatnaPageViewModel, Uri>(this, "OpenUri", (sender, uri) =>
            {
                Device.OpenUri(uri);
            });

            MessagingCenter.Unsubscribe<MatnaPageViewModel, List<double>>(this, "MoveToLocation");
            MessagingCenter.Subscribe<MatnaPageViewModel, List<double>>(this, "MoveToLocation", (sender, locRad) =>
            {
                double EPSILON = 0.0001;
                if (Math.Abs(locRad[0]) < EPSILON && Math.Abs(locRad[1]) < EPSILON)
                {
                    MessagingCenter.Send(this, "IsMapIdled");
                    return;
                }

                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(locRad[0], locRad[1]), Distance.FromMeters(locRad[2])));
            });

            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "ShowActIndicator");
            MessagingCenter.Subscribe<MatnaPageViewModel>(this, "ShowActIndicator", (sender) =>
            {
                actIndicator.IsVisible = true;
            });

            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "HideActIndicator");
            MessagingCenter.Subscribe<MatnaPageViewModel>(this, "HideActIndicator", (sender) =>
            {
                actIndicator.IsVisible = false;
            });

            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "CheckMapVisible");
            MessagingCenter.Subscribe<MatnaPageViewModel>(this, "CheckMapVisible", (sender) =>
            {
                map.Circles.Clear();
                if (map.VisibleRegion == null)
                    DisplayAlert(AppResources.Matna, AppResources.MoveMapPlease, AppResources.OK);
                else if (Math.Abs(PropertiesDictionary.Radius - PropertiesDictionary.MaxRadKM * 1000) < 1)
                {
                    DisplayAlert(AppResources.Matna, String.Format(AppResources.MaxRadReached, PropertiesDictionary.MaxRadKM), AppResources.OK);
                    var circle = new Circle
                    {
                        Center = new Position(PropertiesDictionary.Latitude, PropertiesDictionary.Longitude),
                        Radius = Distance.FromMeters(PropertiesDictionary.Radius),
                        StrokeColor = Color.Magenta,
                        StrokeWidth = 3f,
                        FillColor = Color.Transparent,
                    };
                    map.Circles.Add(circle);
                }
            });

            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "ShowFilter");
            MessagingCenter.Subscribe<MatnaPageViewModel>(this, "ShowFilter", async (sender) =>
            {
                if (Navigation.ModalStack.Count == 0)
                {
                    var page = new FilterPage();
                    await Navigation.PushModalAsync(page);
                }
            });

            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "ShowList");
            MessagingCenter.Subscribe<MatnaPageViewModel>(this, "ShowList", async (sender) =>
            {
                if (Navigation.ModalStack.Count == 0)
                {
                    BPH.CenterIcon = Helpers.Controls.Icon.Search;
                    var page = new ListPage(((MatnaPageViewModel)BindingContext).PlacesToShow);
                    await Navigation.PushModalAsync(page);
                    BPH.CenterIcon = Helpers.Controls.Icon.AngleDoubleUp;
                }
            });

            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "ShowSearch");
            MessagingCenter.Subscribe<MatnaPageViewModel>(this, "ShowSearch", async (sender) =>
            {
                if (Navigation.ModalStack.Count == 0)
                {
                    var page = new SearchPage();
                    await Navigation.PushModalAsync(page);
                }
            });

            MessagingCenter.Unsubscribe<MatnaPageViewModel, List<GooglePlaceNearbyItem>>(this, "DrawPins");
            MessagingCenter.Subscribe<MatnaPageViewModel, List<GooglePlaceNearbyItem>>(this, "DrawPins", (sender, items) =>
            {
                DrawFromItems(items);
            });

            MessagingCenter.Unsubscribe<Restful>(this, "NetworkUnavailable");
            MessagingCenter.Subscribe<Restful>(this, "NetworkUnavailable", (sender) =>
            {
                DisplayAlert(AppResources.Matna, AppResources.NetworkUnavailable, AppResources.OK);
            });

            MessagingCenter.Unsubscribe<Restful>(this, "GoogleAPIError");
            MessagingCenter.Subscribe<Restful>(this, "GoogleAPIError", (sender) =>
            {
                DisplayAlert(AppResources.Matna, AppResources.GoogleAPIError, AppResources.OK);
            });
            #endregion Codes for MessageCenter

            #region Initial Camera Settings
            map.MyLocationEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
            #endregion Initial Camera Settings
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<MatnaPageViewModel, string>(this, "DisplayAlert");
            MessagingCenter.Unsubscribe<MatnaPageViewModel, Uri>(this, "OpenUri");
            MessagingCenter.Unsubscribe<MatnaPageViewModel, List<double>>(this, "MoveToLocation");
            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "ShowActIndicator");
            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "HideActIndicator");
            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "CheckMapVisible");
            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "ShowFilter");
            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "ShowList");
            MessagingCenter.Unsubscribe<MatnaPageViewModel>(this, "ShowSearch");
            MessagingCenter.Unsubscribe<MatnaPageViewModel, List<GooglePlaceNearbyItem>>(this, "DrawPins");
            MessagingCenter.Unsubscribe<Restful>(this, "NetworkUnavailable"); 
            MessagingCenter.Unsubscribe<Restful>(this, "GoogleAPIError");
        }

        private bool canClose = true;
        protected override bool OnBackButtonPressed()
        {
            if (canClose)
                ShowExitDialog();
            return canClose;
        }
        private async void ShowExitDialog()
        {
            var answer = await DisplayAlert(AppResources.Matna, AppResources.ReallyQuit, AppResources.OK, AppResources.Cancel);
            if (answer)
                canClose = false;
        }

        public MatnaPage()
        {
            InitializeComponent();

            if (AppResources.Locale == "ko")
                map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(37.532600, 127.024612), 10.0);   // Seoul

            // Load saved data to PropertiesDictionary
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
            if (Application.Current.Properties.ContainsKey("MaxRadKM"))
                PropertiesDictionary.MaxRadKM = (double)Application.Current.Properties["MaxRadKM"];
            if (Application.Current.Properties.ContainsKey("SearchHist"))
                PropertiesDictionary.SearchHist = JsonConvert.DeserializeObject<List<GoogleAutocompletePrediction>>((string)Application.Current.Properties["SearchHist"]);
            if (Application.Current.Properties.ContainsKey("ShowGoogle"))
                PropertiesDictionary.ShowGoogle = (bool)Application.Current.Properties["ShowGoogle"];
            if (Application.Current.Properties.ContainsKey("ShowKRSamdae"))
                PropertiesDictionary.ShowKRSamdae = (bool)Application.Current.Properties["ShowKRSamdae"];
            if (Application.Current.Properties.ContainsKey("ShowKRChakhan"))
                PropertiesDictionary.ShowKRChakhan = (bool)Application.Current.Properties["ShowKRChakhan"];
            if (Application.Current.Properties.ContainsKey("ShowKRSuyo"))
                PropertiesDictionary.ShowKRSuyo = (bool)Application.Current.Properties["ShowKRSuyo"];
            if (Application.Current.Properties.ContainsKey("GoogleSort"))
                PropertiesDictionary.GoogleSort = (int)Application.Current.Properties["GoogleSort"];
            if (Application.Current.Properties.ContainsKey("Keyword"))
                PropertiesDictionary.Keyword = (string)Application.Current.Properties["Keyword"];

            map.CameraIdled += CameraChanged;

            map.PinClicked += (object sender, PinClickedEventArgs e) =>
            {
                if (e.Pin != null && e.Pin.Tag != null)
                {
                    try
                    {
                        string pid = ((GooglePlaceNearbyItem)e.Pin.Tag).PlaceId;
                        MessagingCenter.Send(this, "PinSelected", ((GooglePlaceNearbyItem)e.Pin.Tag));
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                }
            };
        }

        private void DrawFromItems(List<GooglePlaceNearbyItem> items)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    map.Pins.Clear();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Matna>> Unmanaged descriptor could happen on clear / Check Xamarin.Forms.GoogleMaps issue #162");
                    System.Diagnostics.Debug.WriteLine(e);
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
            if (item.PlaceId == null || Math.Abs(item.Lat) < 0.0001 || Math.Abs(item.Lon) < 0.0001)
                return;

            if (item.Name == null)
                item.Name = AppResources.NoName;

            string rating = $"{String.Format("{0:0.0}", item.RatingD).Replace(".", string.Empty)}.png";
            Device.BeginInvokeOnMainThread(() =>
            {
                Pin pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(item.Lat, item.Lon),
                    Label = item.Name,
                    Icon = BitmapDescriptorFactory.FromBundle(rating),
                    Tag = item
                };
                map.Pins.Add(pin);
            });
        }

        public volatile bool isCameraStable = true;
        protected virtual async void CameraChanged(object sender, CameraIdledEventArgs e)
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

                MessagingCenter.Send(this, "CameraChanged");

                isCameraStable = false;
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                isCameraStable = true;
                MessagingCenter.Send(this, "IsMapIdled");
            }
        }
    }
}
