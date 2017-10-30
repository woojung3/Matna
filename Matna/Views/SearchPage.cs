using System;
using System.Collections.Generic;
using Matna.Models;
using Matna.Resources.Localize;
using Matna.ViewModels;
using Xamarin.Forms;

namespace Matna.Views
{
    public partial class SearchPage : ContentPage
    {
        void ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            MessagingCenter.Send(this, "OnPredictionSelected", ((GoogleAutocompletePrediction)e.SelectedItem).PlaceId);

            if (Navigation.ModalStack.Count == 1)
                Navigation.PopModalAsync();
        }

        void TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            MessagingCenter.Send(this, "OnSearchClicked");
        }

        public SearchPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<SearchPageViewModel>(this, "HideSearch", (sender) =>
            {
                if (Navigation.ModalStack.Count == 1)
                    Navigation.PopModalAsync();
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            editor.Focus();
        }
    }
}
