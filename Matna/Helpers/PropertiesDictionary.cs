using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Matna.Helpers
{
    public class PropertiesDictionary
    {
        public static void Save()
        {
            Application.Current.SavePropertiesAsync();  // To be safe from a crash or being killed by the OS.
        }

        public static string NotFoundImage = "http://blogfiles1.naver.net/20130625_58/bakain_1372134632375hu8gm_GIF/404-not-found.gif";

        private static double latitude = 37.543821d;
        public static double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                Application.Current.Properties["Latitude"] = value;
                latitude = value;
                Save();
            }
        }

        private static double longitude = 127.083814;
        public static double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                Application.Current.Properties["Longitude"] = value;
                longitude = value;
                Save();
            }
        }

        static double zoom = 10.0d;
        public static double Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                Application.Current.Properties["Zoom"] = value;
                zoom = value;
                Save();
            }
        }

        static double radius = 500.0;
        public static double Radius
        {
            get
            {
                return radius;
            }
            set
            {
                Application.Current.Properties["Radius"] = value;
                radius = value;
                Save();
            }
        }
    }
}
