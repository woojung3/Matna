using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Matna.Helpers;

namespace Matna.Droid
{
    [Activity(Label = "Matna.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
              ,ScreenOrientation = ScreenOrientation.Portrait)] //This is what controls orientation
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            #region CONFIG STARTS

            // Code for GoogleMaps
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);

            #endregion CONFIG ENDS

            LoadApplication(new App());
        }
    }
}
