using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Matna.Helpers;

namespace Matna.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();

            #region CONFIG STARTS

            // Code for starting up the Xamarin Test Cloud Agent
#if DEBUG
            Xamarin.Calabash.Start();
#endif
            // Code for GoogleMaps
            Xamarin.FormsGoogleMaps.Init(Keys.GoogleMapsApiKeyiOS);

            #endregion CONFIG ENDS

            LoadApplication(new App());
            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
