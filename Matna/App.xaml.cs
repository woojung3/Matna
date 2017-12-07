using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matna.Data;
using Matna.Data.external;
using Matna.Helpers;
using Matna.Helpers.File;
using Matna.Helpers.GooglePlacesApi;
using Matna.Models;
using Matna.Resources.Localize;
using Newtonsoft.Json;
using PCLStorage;
using Xamarin.Forms;

namespace Matna
{
    public partial class App : Application
    {
        static GooglePlaceNearbyItemDatabase myPlacesDatabase;
        public static GooglePlaceNearbyItemDatabase MyPlacesDatabase
        {
            get
            {
                if (myPlacesDatabase == null)
                {
                    myPlacesDatabase = new GooglePlaceNearbyItemDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("MyPlace.db3"));
                }
                return myPlacesDatabase;
            }
        }

        static GooglePlaceNearbyItemDatabase adPlacesDatabase;
        public static GooglePlaceNearbyItemDatabase AdPlacesDatabase
        {
            get
            {
                if (adPlacesDatabase == null)
                {
                    adPlacesDatabase = new GooglePlaceNearbyItemDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("AdPlace.db3"));
                }
                return adPlacesDatabase;
            }
        }

        public async void CreateRealFile()
        {
            try
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("Check", CreationCollisionOption.OpenIfExists);
                IFile file = await folder.CreateFileAsync("1.0.18", CreationCollisionOption.FailIfExists);  // Don't change filename unless new check is needed

                // This try-catch is to check whether this is initial launch or not.
                // Body
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
                    PropertiesDictionary.SavedLists.Add(l);
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("Lists", CreationCollisionOption.OpenIfExists);

                var files = await folder.GetFilesAsync();
                foreach (var f in files)
                {
                    var data = await f.ReadAllTextAsync();
                    var list = (GooglePlaceNearbyList)JsonConvert.DeserializeObject<GooglePlaceNearbyList>(data);
                    PropertiesDictionary.SavedLists.Add(list);
                }
            }
        }

        public App()
        {
            InitializeComponent();

            CreateRealFile();

            MainPage = new MatnaPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            GooglePlacesApi.Instance.DisconnectAndRelease();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
