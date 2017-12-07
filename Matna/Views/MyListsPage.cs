using System;
using System.Collections.Generic;
using System.Linq;
using Matna.Data.external;
using Matna.Helpers;
using Matna.Models;
using Matna.Resources.Localize;
using Matna.ViewModels;
using Newtonsoft.Json;
using Plugin.Clipboard;
using Plugin.Share;
using Xamarin.Forms;

namespace Matna.Views
{
    public partial class MyListsPage : ContentPage
    {
        async void GetBundleClickedAsync(object sender, System.EventArgs e)
        {
            var ans = await DisplayAlert(AppResources.Warning, AppResources.BundleOverwriteWarning, AppResources.OK, AppResources.Cancel);
            if (ans)
            {
                List<GooglePlaceNearbyList> lists = new List<GooglePlaceNearbyList>()
                {
                    new GooglePlaceNearbyList()
                    {
                        Name = AppResources.Samdae,
                        List = ko.SamdaeData,
                        IsEnabled = true
                    },
                    new GooglePlaceNearbyList()
                    {
                        Name = AppResources.Chakhan,
                        List = ko.ChakhanData,
                        IsEnabled = true
                    },
                    new GooglePlaceNearbyList()
                    {
                        Name = AppResources.Suyo,
                        List = ko.SuyoData,
                        IsEnabled = true
                    }
                };

                foreach (var l in lists)
                {
                    await l.Save2FileAsync();
                    try
                    {
                        PropertiesDictionary.SavedLists.Remove(PropertiesDictionary.SavedLists.Single(s => s.Name == l.Name));
                    }
                    catch (Exception ex) { } // Do nothing. Not exists
                    PropertiesDictionary.SavedLists.Add(l);
                }

                if (Navigation.ModalStack.Count == 2)
                    await Navigation.PopModalAsync();
            }
        }

        void OnItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            GooglePlaceNearbyList item = (Matna.Models.GooglePlaceNearbyList)((ListView)sender).SelectedItem;
            CrossShare.Current.SetClipboardText(item.ToString());
            DisplayAlert(AppResources.Matna, AppResources.ListCopied, AppResources.OK);
        }

        async void OnAddMyListClickedAsync(object sender, System.EventArgs e)
        {
            try
            {
                var str = await CrossClipboard.Current.GetTextAsync();
                var list = (GooglePlaceNearbyList)JsonConvert.DeserializeObject<GooglePlaceNearbyList>(str);

                bool isWriteOk = true;
                if (PropertiesDictionary.SavedLists.Exists(s => s.Name == list.Name))
                {
                    var ans = await DisplayAlert(AppResources.Warning, AppResources.ListOverwriteWarning, AppResources.OK, AppResources.Cancel);
                    if (ans)
                        PropertiesDictionary.SavedLists.Remove(PropertiesDictionary.SavedLists.Single(s => s.Name == list.Name));
                    else
                        isWriteOk = false;
                }

                if (isWriteOk)
                {
                    await list.Save2FileAsync();
                    PropertiesDictionary.SavedLists.Add(list);

                    await DisplayAlert(AppResources.Matna, AppResources.Success, AppResources.OK);
                    if (Navigation.ModalStack.Count == 2)
                        await Navigation.PopModalAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                await DisplayAlert(AppResources.Matna, AppResources.AddListError, AppResources.OK);
            }
        }

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
