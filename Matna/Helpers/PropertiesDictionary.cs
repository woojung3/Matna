using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Matna.Models;
using Newtonsoft.Json;
using PCLStorage;
using Xamarin.Forms;

namespace Matna.Helpers
{
    public class PropertiesDictionary
    {
        public static void Save()
        {
            Application.Current.SavePropertiesAsync();  // To be safe from a crash or being killed by the OS.
        }

        //public static string NotFoundImage = "http://blogfiles1.naver.net/20130625_58/bakain_1372134632375hu8gm_GIF/404-not-found.gif";
        public static string NotFoundImage = "http://icons.iconarchive.com/icons/graphicloads/colorful-long-shadow/256/Restaurant-icon.png";
        public static string IconDefaultImage = "https://maps.gstatic.com/mapfiles/place_api/icons/restaurant-71.png";

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

        private static double zoom = 10.0d;
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

        private static double radius = 500.0;
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

        private static double maxRadKM = 50.0;
        public static double MaxRadKM
        {
            get
            {
                return maxRadKM;
            }
            set
            {
                Application.Current.Properties["MaxRadKM"] = value;
                maxRadKM = value;
                Save();
            }
        }

        private static int googleRecIdx = 1;
        public static int GoogleRecIdx
        {
            get
            {
                return googleRecIdx;
            }
            set
            {
                Application.Current.Properties["GoogleRecIdx"] = value;
                googleRecIdx = value;
                Save();
            }
        }

        private static List<GoogleAutocompletePrediction> searchHist = new List<GoogleAutocompletePrediction>();
        public static List<GoogleAutocompletePrediction> SearchHist
        {
            get
            {
                return searchHist;
            }
            set
            {
                Application.Current.Properties["SearchHist"] = JsonConvert.SerializeObject(value);
                searchHist = value;
                Save();
            }
        }

        private static bool showGoogle = true;
        public static bool ShowGoogle
        {
            get
            {
                return showGoogle;
            }
            set
            {
                Application.Current.Properties["ShowGoogle"] = value;
                showGoogle = value;
                Save();
            }
        }

        private static int googleSort = 0;
        public static int GoogleSort
        {
            get
            {
                return googleSort;
            }
            set
            {
                Application.Current.Properties["GoogleSort"] = value;
                googleSort = value;
                Save();
            }
        }

        private static string keyword = "";
        public static string Keyword
        {
            get
            {
                return keyword;
            }
            set
            {
                Application.Current.Properties["Keyword"] = value;
                keyword = value;
                Save();
            }
        }

        public static List<GooglePlaceNearbyList> SavedLists = new List<GooglePlaceNearbyList>();
    }
}
