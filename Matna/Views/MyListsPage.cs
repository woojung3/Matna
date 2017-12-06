using System;
using System.Collections.Generic;
using Matna.Helpers;
using Matna.Resources.Localize;
using Matna.ViewModels;
using Xamarin.Forms;

namespace Matna.Views
{
    public partial class MyListsPage : ContentPage
    {
        public MyListsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Unsubscribe<MyListsPageViewModel>(this, "HideMyLists");
            MessagingCenter.Subscribe<MyListsPageViewModel>(this, "HideMyLists", (sender) =>
            {
                if (Navigation.ModalStack.Count == 2)
                    Navigation.PopModalAsync();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<MyListsPageViewModel>(this, "HideMyLists");
        }
    }
}
