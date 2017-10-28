using Matna.Helpers.Controls;
using Matna.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HScrollView), typeof(HScrollViewRenderer))]

namespace Matna.Droid.Renderers
{
    public class HScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var element = e.NewElement as HScrollView;
            element?.Render();
        }
    }
}