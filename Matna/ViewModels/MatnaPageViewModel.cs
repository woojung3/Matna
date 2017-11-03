﻿using System;
using System.Windows.Input;
using Xamarin.Forms;
using Matna.Models;
using System.Collections.Generic;
using Matna.Utils.Restful;
using Matna.Helpers;
using System.Linq;
using Matna.Resources.Localize;
using Matna.Views;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Matna.Helpers.Controls;
using Matna.Data.external;
using Matna.Utils;

namespace Matna.ViewModels
{
    public class MatnaPageViewModel : BaseViewModel
    {
        List<GooglePlaceNearbyItem> placesToShow = new List<GooglePlaceNearbyItem>();
        //List<GooglePlaceNearbyItem> placesToShow = ko.SamdaeData; // TODO add external lists
        public List<GooglePlaceNearbyItem> PlacesToShow
        {
            get
            {
                placesToShow.Sort((a, b) => (int)(-1.0 * (a.RatingD.CompareTo(b.RatingD)))); // Desc order
                return placesToShow;
            }
            set
            {
                var list = value;
                if (AppResources.Locale == "ko")
                {
                    List<List<GooglePlaceNearbyItem>> lists = new List<List<GooglePlaceNearbyItem>>();
                    if (true)   // TODO change here to support data filter
                    {
                        lists.Add(ko.SamdaeData);
                        lists.Add(ko.ChakhanData);
                        lists.Add(ko.SuyoData);
                    }

                    foreach (var items in lists)
                    {
                        foreach (GooglePlaceNearbyItem item in items)
                        {
                            var sCoord = new Coordinates(item.Lat, item.Lon);
                            var eCoord = new Coordinates(PropertiesDictionary.Latitude, PropertiesDictionary.Longitude);
                            var dist = sCoord.DistanceFrom(eCoord);
                            if (dist < PropertiesDictionary.Radius)
                                list.Add(item);
                        }
                    }
                }

                var firstItemsInGroup = from item in list group item by item.PlaceId into g select g.First();

                placesToShow = firstItemsInGroup.ToList();
                MessagingCenter.Send(this, "DrawPins", placesToShow);
                OnPropertyChanged("PlacesToShow");
                OnPropertyChanged("IsShowAd");
            }
        }

        static GooglePlaceNearbyItem NoneItem = new GooglePlaceNearbyItem()
        {
            PlaceId = "",
            Geometry = new Geometry() { Location = new Location() { Lat = 0.0, Lon = 0.0 } },
        };
        GooglePlaceNearbyItem selectedItem = new GooglePlaceNearbyItem()
        {
            PlaceId = "",
            Geometry = new Geometry(){ Location = new Location(){ Lat=0.0, Lon=0.0 } },
        };
        public GooglePlaceNearbyItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                MessagingCenter.Send(this, "MoveToLocation", new List<double>() { selectedItem.Lat, selectedItem.Lon, 100 });
                OnPropertyChanged();
                OnPropertyChanged("IsSelectedItemExists");
            }
        }
        public bool IsSelectedItemExists
        {
            get
            {
                if (selectedItem.PlaceId != null && selectedItem.PlaceId != "")
                    return true;
                return false;
            }
        }

        bool isShowSaved = false;
        public bool IsShowSaved
        {
            get
            {
                return isShowSaved;
            }
            set
            {
                isShowSaved = value;
                OnPropertyChanged();
                OnPropertyChanged("IsShowLoadPlacesFromMap");
                OnPropertyChanged("IsShowAd");
            }
        }

        public bool IsShowLoadPlacesFromMap
        {
            get
            {
                if (isShowSaved)
                    return false;
                return true;
            }
        }

        bool isShowAd = false;
        public bool IsShowAd
        {
            get
            {
                if (!PlacesToShow.Any())
                    return false;
                
                return isShowAd;
            }
            set
            {
                isShowAd = value;
                OnPropertyChanged();
            }
        }

        public MatnaPageViewModel()
        {
            // Properties (Commands) initilization
            OnSearchClicked = new Command(() => {
                SelectedItem = NoneItem;
                MessagingCenter.Send(this, "ShowSearch");
            });
            OnItemClicked = new Command((arg) => {
                try
                {
                    SelectedItem = placesToShow.Find(x => x.PlaceId == arg.ToString());
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            });
            OnFilterClicked = new Command(() => {
                MessagingCenter.Send(this, "DisplayAlert", AppResources.ComingSoon);
                //MessagingCenter.Send(this, "ShowFilter");
            });
            OnListClicked = new Command(() => {
                MessagingCenter.Send(this, "ShowList");
            });
            OnLoadSavedClicked = new Command(async () =>
            {
                var items = await App.MyPlacesDatabase.GetItemsAsync();
                Device.BeginInvokeOnMainThread(() => 
                { 
                    PlacesToShow = items;
                    IsShowSaved = true; 
                    SelectedItem = NoneItem;
                });
            });
            OnBackSavedClicked = new Command(() => {
                SelectedItem = NoneItem;
                PlacesToShow = new List<GooglePlaceNearbyItem>();
                IsShowSaved = false;
            });
            OnLoadPlacesFromMapClicked = new Command(() => {
                SelectedItem = NoneItem;
                LoadPlaces();
            });
            OnDeselectItemClicked = new Command(() => {
                SelectedItem = NoneItem;
            });

            OnRouteClicked = new Command(() => {
                Uri uri = new Uri($"https://www.google.com/maps/dir/?api=1&destination={SelectedItem.Name}&destination_place_id={selectedItem.PlaceId}&travelmode=transit");
                MessagingCenter.Send(this, "OpenUri", uri);
            });
            OnDetailClicked = new Command(() => {
                Uri uri = new Uri($"https://www.google.com/maps/search/?api=1&query={SelectedItem.Lat},{SelectedItem.Lon}&query_place_id={SelectedItem.PlaceId}");
                MessagingCenter.Send(this, "OpenUri", uri);
            });
            OnShareClicked = new Command(() => {
                CrossShare.Current.Share(
                    new ShareMessage
                    {
                        Title = SelectedItem.Name,
                        Text = SelectedItem.Vicinity,
                        //Url = urlShareLabel.Text  // Not necessary
                    },
                    new ShareOptions
                    {
                        ChooserTitle = AppResources.Share,
                        ExcludedUIActivityTypes = new[] { ShareUIActivityType.PostToFacebook }
                    }
                );
            });
            OnSaveClicked = new Command(async () => {
                var item = placesToShow.Find(x => x.PlaceId == SelectedItem.PlaceId);
                item.IsSaved = true;
                SelectedItem.IsSaved = true;
                OnPropertyChanged("SelectedItem");
                await App.MyPlacesDatabase.SaveItemAsync(item);
            });
            OnRemoveClicked = new Command(async () => {
                var item = placesToShow.Find(x => x.PlaceId == SelectedItem.PlaceId);
                item.IsSaved = false;
                SelectedItem.IsSaved = false;
                OnPropertyChanged("SelectedItem");
                await App.MyPlacesDatabase.DeleteItemAsync(SelectedItem);
            });

            // Initialize Data
            MessagingCenter.Unsubscribe<MatnaPage>(this, "CameraChanged");
            MessagingCenter.Subscribe<MatnaPage>(this, "CameraChanged", (sender) =>
            {
                // Currently does nothing.                
                // Do not call Places API every time. Cost is way too high.
            });

            MessagingCenter.Unsubscribe<AdViewControl, bool>(this, "ShowAd");
            MessagingCenter.Subscribe<AdViewControl, bool>(this, "ShowAd", (sender, b) =>
            {
                if (!IsShowAd)
                    IsShowAd = b;
            });

            MessagingCenter.Unsubscribe<MatnaPage, GooglePlaceNearbyItem>(this, "PinSelected");
            MessagingCenter.Subscribe<MatnaPage, GooglePlaceNearbyItem>(this, "PinSelected", (sender, item) =>
            {
                SelectedItem = item;
            });

            MessagingCenter.Unsubscribe<SearchPage, string>(this, "OnPredictionSelected");
            MessagingCenter.Subscribe<SearchPage, string>(this, "OnPredictionSelected", async (sender, str) =>
            {
                // 1. Find item detail
                GooglePlaceNearbyItem item = await Restful.Inst.GoogleMapsPlaceNameFromDetails(str);

                // 2. LoadPlaces with defined Radius
                LoadPlaces(item.Lat, item.Lon, 1000, item);    // TODO 1000 to some user-definable value

                // 3. If item type is restaurant, add it to PlacesToShow; else ignore
                if (item.Types.Contains("restaurant"))
                    SelectedItem = item;
                else
                    MessagingCenter.Send(this, "MoveToLocation", new List<double>() { item.Lat, item.Lon, 1000 });
            });
        }

        private async void LoadPlaces(double? Lat = null, double? Lon = null, double? Rad = null, GooglePlaceNearbyItem append = null)
        {
            if (Lat == null)
                Lat = PropertiesDictionary.Latitude;
            if (Lon == null)
                Lon = PropertiesDictionary.Longitude;
            if (Rad == null)
                Rad = PropertiesDictionary.Radius;
            
            MessagingCenter.Send(this, "ShowActIndicator");
            MessagingCenter.Send(this, "CheckMapVisible");
            List<string> types = new List<string>(){
                "restaurant"
            };
            List<GooglePlaceNearbyItem> rtn = await Restful.Inst.GoogleMapsPlaceNearbySearch(
                (double)Lat, 
                (double)Lon, 
                (double)Rad, 
                types, 
                new List<string>(){  }
            );

            if (append == null || !append.Types.Contains("restaurant"))
                PlacesToShow = rtn;
            else
            {
                rtn.Add(append);
                PlacesToShow = rtn;
            }
            MessagingCenter.Send(this, "HideActIndicator");
        }

        public ICommand OnSearchClicked { get; }
        public ICommand OnItemClicked { get; }
        public ICommand OnFilterClicked { get; }
        public ICommand OnListClicked { get; }
        public ICommand OnLoadSavedClicked { get; }
        public ICommand OnBackSavedClicked { get; }
        public ICommand OnLoadPlacesFromMapClicked { get; }
        public ICommand OnDeselectItemClicked { get; }
        public ICommand OnRouteClicked { get; }
        public ICommand OnDetailClicked { get; }
        public ICommand OnShareClicked { get; }
        public ICommand OnSaveClicked { get; }
        public ICommand OnRemoveClicked { get; }
    }
}
