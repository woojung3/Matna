using Matna.Helpers.Controls;
using Matna.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HScrollView), typeof(HScrollViewRenderer))]

namespace Matna.iOS.Renderers
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