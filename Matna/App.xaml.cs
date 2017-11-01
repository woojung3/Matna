using Matna.Data;
using Matna.Helpers.File;
using Matna.Helpers.GooglePlacesApi;
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

        public App()
        {
            InitializeComponent();

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
