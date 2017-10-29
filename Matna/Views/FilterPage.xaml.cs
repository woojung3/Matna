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

            MessagingCenter.Subscribe<FilterPageViewModel>(this, "HideFilter", (sender) =>
            {
                if (Navigation.ModalStack.Count == 1)
                    Navigation.PopModalAsync();
            });
        }
    }
}
