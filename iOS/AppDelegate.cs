using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Matna.Helpers;
using HockeyApp.iOS;
using FFImageLoading.Forms.Touch;

namespace Matna.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();

            #region CONFIG STARTS

            // Code for HockeyApp
            var manager = BITHockeyManager.SharedHockeyManager;
            manager.LogLevel = BITLogLevel.Debug;
            manager.Configure("52a8189f5c134916b91946cf517dcb88");
            manager.StartManager();
            manager.Authenticator.AuthenticateInstallation(); // This line is obsolete in crash only builds

            // Code for starting up the Xamarin Test Cloud Agent
#if DEBUG
            Xamarin.Calabash.Start();
#endif

            // Code for FFImageLoading
            CachedImageRenderer.Init();

            // Code for GoogleMaps
            Xamarin.FormsGoogleMaps.Init(Keys.GoogleMapsApiKeyiOS);

            NativePlacesApi.Init();

            #endregion CONFIG ENDS

            LoadApplication(new App());
            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
