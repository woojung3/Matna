using System;
using System.Collections.Generic;
using Matna.Resources.Localize;
using Matna.ViewModels;
using Xamarin.Forms;

namespace Matna.Views
{
    public partial class FilterPage : ContentPage
    {
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
