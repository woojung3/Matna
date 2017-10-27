using System;
using Xamarin.Forms;

namespace Matna.Helpers.Controls
{
    public class AdViewControl : ContentView
    {
        public static BindableProperty PlacementIdProperty = BindableProperty.Create(nameof(PlacementId), typeof(string),
            typeof(AdViewControl), null);
        public string PlacementId
        {
            get { return (string)GetValue(PlacementIdProperty); }
            set { SetValue(PlacementIdProperty, value); }
        }

        public static BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(FacebookAdSize),
            typeof(AdViewControl), FacebookAdSize.BannerHeight50);


        public FacebookAdSize Size
        {
            get { return (FacebookAdSize)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }


        public event EventHandler Loaded;
        public event EventHandler Click;
        public event EventHandler<ErrorEventArgs> Error;
        public event EventHandler Impression;

        public virtual void OnLoaded()
        {
            Loaded?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnImpression()
        {
            Impression?.Invoke(this, EventArgs.Empty);
        }

        public void OnError(int errorCode, string errorMessage)
        {
            Error?.Invoke(this, new ErrorEventArgs(errorCode, errorMessage));
        }

        public enum FacebookAdSize
        {
            BannerHeight50,
            BannerHeight90,
            Interstitial,
            RectangleHeight250
        }

        public class ErrorEventArgs : EventArgs
        {
            public ErrorEventArgs(int errorCode, string errorMessage)
            {
                ErrorCode = errorCode;
                ErrorMessage = errorMessage;
            }

            public int ErrorCode { get; }
            public string ErrorMessage { get; }

        }
    }
}