using Matna.Helpers.Controls;
using Matna.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Facebook.Ads;
[assembly: ExportRenderer(typeof(AdViewControl), typeof(AdViewRenderer))]
namespace Matna.Droid.Renderers
{
    public class AdViewRenderer : ViewRenderer
    {
        protected AdViewControl AdViewControl => (AdViewControl)Element;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;

            SetNativeControl();
        }

        private void SetNativeControl()
        {
            AdView adView = new AdView(Context, AdViewControl.PlacementId, GetAdSize(AdViewControl.Size))
            {
                LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent)
            };

            adView.AdClicked += (sender, args) => AdViewControl.OnClick();
            adView.AdLoaded += (sender, args) => AdViewControl.OnLoaded();
            adView.LoggingImpression += (sender, args) => AdViewControl.OnImpression();
            adView.Error += (sender, args) => AdViewControl.OnError(args.P1.ErrorCode, args.P1.ErrorMessage);

            adView.LoadAd();
            SetNativeControl(adView);
        }

        private AdSize GetAdSize(AdViewControl.FacebookAdSize size)
        {
            switch (size)
            {
                case AdViewControl.FacebookAdSize.BannerHeight50:
                    return AdSize.BannerHeight50;
                case AdViewControl.FacebookAdSize.BannerHeight90:
                    return AdSize.BannerHeight90;
                case AdViewControl.FacebookAdSize.Interstitial:
                    return AdSize.Interstitial;
            }
            return AdSize.RectangleHeight250;
        }
    }
}