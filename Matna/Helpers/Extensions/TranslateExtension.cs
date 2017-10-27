using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Matna
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        protected readonly CultureInfo ci;
        protected const string ResourceId = "Matna.AppResources";

        public TranslateExtension()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                    break;
            }
        }

        public string Text { get; set; }

        public virtual object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager resmgr = new ResourceManager(ResourceId
                                , typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resmgr.GetString(Text, ci);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }

    [ContentProperty("Text")]
    public class TranslateWithSpacingExtension : TranslateExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager resmgr = new ResourceManager(ResourceId
                                , typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resmgr.GetString(Text, ci);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return $"   {translation}   ";
        }
    }
}
