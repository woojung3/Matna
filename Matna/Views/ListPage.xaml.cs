using System;
using System.Collections.Generic;
using Matna.Models;
using Matna.Resources.Localize;
using Matna.ViewModels;
using Xamarin.Forms;

namespace Matna.Views
{
    public partial class ListPage : ContentPage
    {
        private List<GooglePlaceNearbyItem> placesToShow;

        public ListPage(List<GooglePlaceNearbyItem> placesToShow)
        {
            InitializeComponent();

            ((ListPageViewModel)BindingContext).PlacesToShow = placesToShow;

            MessagingCenter.Subscribe<ListPageViewModel>(this, "HideList", (sender) =>
            {
                if (Navigation.ModalStack.Count == 1)
                    Navigation.PopModalAsync();
            });

            MessagingCenter.Subscribe<ListPageViewModel, string>(this, "DisplayAlert", (sender, str) =>
            {
                DisplayAlert(AppResources.Matna, str, AppResources.OK);
            });

            MessagingCenter.Subscribe<ListPageViewModel, Uri>(this, "OpenUri", (sender, uri) =>
            {
                Device.OpenUri(uri);
            });
        }

        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event   
            ((ListView)sender).SelectedItem = null; // de-select the row  
        }
    }
}
