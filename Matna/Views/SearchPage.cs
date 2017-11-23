using System;
using System.Collections.Generic;
using System.Linq;
using Matna.Helpers;
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
            var hist = PropertiesDictionary.SearchHist;
            if (hist.Count() > 4)
                hist.RemoveAt(hist.Count()-1);
            hist.Insert(0, (GoogleAutocompletePrediction)e.SelectedItem);

            MessagingCenter.Send(this, "OnPredictionSelected", ((GoogleAutocompletePrediction)e.SelectedItem).PlaceId);

            if (Navigation.ModalStack.Count == 1)
                Navigation.PopModalAsync();
        }

        void TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (e.NewTextValue.EndsWith(System.Environment.NewLine, StringComparison.CurrentCulture))
            {
                if (((SearchPageViewModel)this.BindingContext).Predictions.Count() > 0)
                {
                    var hist = PropertiesDictionary.SearchHist;
                    if (hist.Count() > 4)
                        hist.RemoveAt(hist.Count() - 1);
                    hist.Insert(0, ((SearchPageViewModel)this.BindingContext).Predictions.FirstOrDefault());

                    MessagingCenter.Send(this, "OnPredictionSelected", ((SearchPageViewModel)this.BindingContext).Predictions.FirstOrDefault().PlaceId);
                }

                if (Navigation.ModalStack.Count == 1)
                    Navigation.PopModalAsync();
            }
            MessagingCenter.Send(this, "OnSearchClicked");
        }

        public SearchPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            editor.Focus();

            MessagingCenter.Unsubscribe<SearchPageViewModel>(this, "HideSearch");
            MessagingCenter.Subscribe<SearchPageViewModel>(this, "HideSearch", (sender) =>
            {
                if (Navigation.ModalStack.Count == 1)
                    Navigation.PopModalAsync();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<SearchPageViewModel>(this, "HideSearch");
        }
    }
}
