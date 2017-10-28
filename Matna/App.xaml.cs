using Matna.Data;
using Matna.Helpers.File;
using Xamarin.Forms;

namespace Matna
{
    public partial class App : Application
    {
        static PlaceItemDatabase myPlacesDatabase;
        public static PlaceItemDatabase MyPlacesDatabase
        {
            get
            {
                if (myPlacesDatabase == null)
                {
                    myPlacesDatabase = new PlaceItemDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("MyPlace.db3"));
                }
                return myPlacesDatabase;
            }
        }

        static PlaceItemDatabase adPlacesDatabase;
        public static PlaceItemDatabase AdPlacesDatabase
        {
            get
            {
                if (adPlacesDatabase == null)
                {
                    adPlacesDatabase = new PlaceItemDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("AdPlace.db3"));
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
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
