using System;
using System.Collections.Generic;
using Matna.Helpers;
using Matna.Resources.Localize;
using Matna.ViewModels;
using Xamarin.Forms;

namespace Matna.Views
{
    public partial class FilterPage : ContentPage
    {
        async void OnManageMyListClicked(object sender, System.EventArgs e)
        {
            if (Navigation.ModalStack.Count == 1)
            {
                var page = new MyListsPage();
                await Navigation.PushModalAsync(page);
            }
        }

        void SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                PropertiesDictionary.GoogleSort = selectedIndex;
            }
        }

        public FilterPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Unsubscribe<FilterPageViewModel>(this, "HideFilter");
            MessagingCenter.Subscribe<FilterPageViewModel>(this, "HideFilter", (sender) =>
            {
                if (Navigation.ModalStack.Count == 1)
                    Navigation.PopModalAsync();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<FilterPageViewModel>(this, "HideFilter");
        }
    }
}
