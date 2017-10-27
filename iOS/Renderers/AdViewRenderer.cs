using Matna.Helpers.Controls;
using Matna.iOS.Renderers;
using CoreGraphics;
using Facebook.AudienceNetwork;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AdSize = Facebook.AudienceNetwork.AdSize;

[assembly: ExportRenderer(typeof(AdViewControl), typeof(AdViewRenderer))]
namespace Matna.iOS.Renderers
{
    public class AdViewRenderer : ViewRenderer, IAdViewDelegate
    {
        protected AdViewControl AdViewControl => (AdViewControl)Element;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;



            AdSettings.AddTestDevice(AdSettings.TestDeviceHash);

            SetNativeControl();
        }

        private void SetNativeControl()
        {
            var viewcontroller = UIApplication
                    .SharedApplication
                    .Windows[0].RootViewController;
            AdView adView = new AdView(AdViewControl.PlacementId, GetAdSize(AdViewControl.Size), viewcontroller)
            {
                Delegate = this
            };

            adView.LoadAd();
            var viewSize = viewcontroller.View.Bounds.Size;
            var bottomAlignedY = viewSize.Height - AdSizes.BannerHeight50.Size.Height;
            adView.Frame = new CGRect(0, bottomAlignedY, viewSize.Width, AdSizes.BannerHeight50.Size.Height);
            this.AddSubview(adView);


            SetNativeControl(adView);
        }

        #region IAdViewDelegate

        [Export("adViewDidClick:")]
        public void AdViewDidClick(AdView adView)
        {
            // Handle when the banner is clicked
            AdViewControl.OnClick();
        }

        [Export("adViewDidFinishHandlingClick:")]
        public void AdViewDidFinishHandlingClick(AdView adView)
        {
            // Handle when the window that is opened by the click is closed);
        }

        [Export("adViewDidLoad:")]
        public void AdViewDidLoad(AdView adView)
        {
            // Handle when the ad on the banner is loaded
            AdViewControl.OnLoaded();

        }

        [Export("adView:didFailWithError:")]
        public void AdViewDidFail(AdView adView, NSError error)
        {
            // Handle if the ad is not loaded correctly

            AdViewControl.OnError((int)error.Code, error.Description);
        }

        #endregion
        private AdSize GetAdSize(AdViewControl.FacebookAdSize size)
        {
            switch (size)
            {
                case AdViewControl.FacebookAdSize.BannerHeight50:
                    return AdSizes.BannerHeight50;
                case AdViewControl.FacebookAdSize.BannerHeight90:
                    return AdSizes.BannerHeight90;
                case AdViewControl.FacebookAdSize.Interstitial:
                    return AdSizes.Interstitial;
            }
            return AdSizes.RectangleHeight250;
        }
    }
}