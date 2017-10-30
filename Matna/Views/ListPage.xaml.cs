using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Matna.Models;
using Matna.Resources.Localize;
using Matna.ViewModels;
using Xamarin.Forms;

namespace Matna.Views
{
    public partial class ListPage : ContentPage
    {
        public ListPage(List<GooglePlaceNearbyItem> placesToShow)
        {
            InitializeComponent();

            var oc = new ObservableCollection<GooglePlaceNearbyItem>();
            foreach (var item in placesToShow)
                oc.Add(item);
            ((ListPageViewModel)BindingContext).PlacesToShow = oc;
        }

        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event   
            ((ListView)sender).SelectedItem = null; // de-select the row  
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Unsubscribe<ListPageViewModel>(this, "HideList");
            MessagingCenter.Subscribe<ListPageViewModel>(this, "HideList", (sender) =>
            {
                if (Navigation.ModalStack.Count == 1)
                {
                    BindingContext = null;
                    Navigation.PopModalAsync();
                }
            });

            MessagingCenter.Unsubscribe<ListPageViewModel, string>(this, "DisplayAlert");
            MessagingCenter.Subscribe<ListPageViewModel, string>(this, "DisplayAlert", (sender, str) =>
            {
                DisplayAlert(AppResources.Matna, str, AppResources.OK);
            });

            MessagingCenter.Unsubscribe<ListPageViewModel, Uri>(this, "OpenUri");
            MessagingCenter.Subscribe<ListPageViewModel, Uri>(this, "OpenUri", (sender, uri) =>
            {
                Device.OpenUri(uri);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<ListPageViewModel, string>(this, "DisplayAlert");
        }
    }
}
