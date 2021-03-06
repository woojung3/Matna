﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Text;
using System.Net;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Matna.Helpers;
using Matna.Models;
using Plugin.Connectivity;
using Matna.Helpers.GooglePlacesApi;
using Matna.Resources.Localize;

namespace Matna.Utils.Restful
{
    public sealed class Restful
    {
        #region Google API
        public async Task<List<GooglePlaceNearbyItem>> GoogleMapsPlaceNearbySearch(double lat, double lon, double radius, List<string> types, List<string> keywords)
        {
            if (keywords.Any())
                MessagingCenter.Send(this, "DisplayAlert", String.Format(AppResources.UsedKeywords, string.Join(",", keywords)));
            string urlBase = string.Format(Constants.GoogleMapsPlaceNearbySearch, lat, lon, radius, String.Join(",", types), String.Join(",", keywords));
            var uri = new Uri(urlBase);
            var rtn = await GetAsyncWrapper<GooglePlaceNearbys>(uri);

            if (rtn == null || rtn.Status == null)
                return new List<GooglePlaceNearbyItem>();
            if (rtn.Status.Equals("OK", StringComparison.CurrentCultureIgnoreCase))
            {
                if (rtn.Results.Any())
                {
                    var res = rtn.Results;

                    var nextPageToken = rtn.NextPageToken;
                    int i = 1;
                    while (nextPageToken != "" && i < PropertiesDictionary.GoogleRecIdx)
                    {
                        var nextUri = new Uri(urlBase + "&pagetoken=" + nextPageToken);
                        await Task.Delay(2000);
                        var rtnNext = await GetAsyncWrapper<GooglePlaceNearbys>(nextUri);
                        nextPageToken = rtnNext.NextPageToken;

                        if (rtnNext != null && rtnNext.Status != null && rtnNext.Status.Equals("OK", StringComparison.CurrentCultureIgnoreCase) && rtnNext.Results.Any())
                            res = res.Concat(rtnNext.Results).ToList();
                        i++;
                    }
                    return res;
                }
            }
            else
                MessagingCenter.Send(this, "DisplayAlert", AppResources.GoogleAPIError);
            return new List<GooglePlaceNearbyItem>();
        }

        public async Task<List<GoogleAutocompletePrediction>> GoogleMapsPlaceAutocomplete(string input)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var rtn = await GooglePlacesApi.Instance.GetPredictions(input, null);
                var lists = new List<GoogleAutocompletePrediction>();
                try
                {
                    foreach (var i in rtn)
                    {
                        lists.Add(new GoogleAutocompletePrediction()
                        {
                            PlaceId = i.PlaceId,
                            StructuredFormatting = new StructuredFormatting()
                            {
                                MainText = i.Description,
                                SecondaryText = i.Subtitle
                            }
                        });
                    }
                    return lists;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Matna>> Autocompletion Error");
                    System.Diagnostics.Debug.WriteLine(ex);

                    return new List<GoogleAutocompletePrediction>();
                }
            }
            else
            {
                string urlBase = string.Format(Constants.GoogleMapsPlaceAutocomplete, input);
                var uri = new Uri(urlBase);
                var rtn = await GetAsyncWrapper<GoogleMapsPlaceAutocomplete>(uri);

                if (rtn == null || rtn.Status == null)
                    return new List<GoogleAutocompletePrediction>();
                if (rtn.Status.Equals("OK", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (rtn.Predictions.Any())
                        return rtn.Predictions;
                }
                return new List<GoogleAutocompletePrediction>();
            }
        }

        static GooglePlaceNearbyItem NoneItem = new GooglePlaceNearbyItem()
        {
            PlaceId = "",
            Geometry = new Geometry() { Location = new Location() { Lat = 0.0, Lon = 0.0 } },
        };
        public async Task<GooglePlaceNearbyItem> GoogleMapsPlaceNameFromDetails(string placeId)
        {
            // HACK Uncomment these when Google Places Web API call reaches its max
            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    try
            //    {
            //        var rtn = await GooglePlacesApi.Instance.GetDetails(placeId);
            //        var item = new GooglePlaceNearbyItem()
            //        {
            //            PlaceId = rtn.PlaceId,
            //            Types = rtn.Types,
            //            Lat = rtn.Coordinate.Latitude,
            //            Lon = rtn.Coordinate.Longitude,
            //            Vicinity = rtn.FormattedAddress,
            //            Name = rtn.Name,
            //            Rating = rtn.Rating,
            //        };

            //        return item;
            //    }
            //    catch (Exception ex)
            //    {
            //        System.Diagnostics.Debug.WriteLine("Matna>> Autocompletion Error");
            //        System.Diagnostics.Debug.WriteLine(ex);

            //        return NoneItem;
            //    }
            //}
            //else 
            {
                string urlBase = string.Format(Constants.GoogleMapsPlaceDetails, placeId, Keys.GooglePlacesApiKey);
                var uri = new Uri(urlBase);
                var rtn = await GetAsyncWrapper<GooglePlaceDetails>(uri);

                if (rtn == null || rtn.Status == null || rtn.Result == null)
                {
                    return NoneItem;
                }
                if (rtn.Status.Equals("OK", StringComparison.CurrentCultureIgnoreCase))
                {
                    return rtn.Result;
                }
                return NoneItem;
            }
        }
        #endregion Google API



        private static volatile Restful instance;
        public static Restful Inst
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Restful();
                    }
                }

                return instance;
            }
        }
        private static object syncRoot = new object();

        private JsonSerializer _serializer = new JsonSerializer();
        private HttpClient client;
        public HttpClientHandler handler;
        public static class Constants
        {
            public static string MainAddr = "https://maps.googleapis.com/maps/api/";
            public static string GoogleMapsPlaceNearbySearch = MainAddr + "place/nearbysearch/json?location={0},{1}&radius={2}&type={3}&keyword={4}&key=" + Keys.GooglePlacesApiKey;
            public static string GoogleMapsPlaceAutocomplete = MainAddr + "place/autocomplete/json?input={0}&key=" + Keys.GooglePlacesApiKey;
            public static string GoogleMapsPlaceDetails = MainAddr + "place/details/json?placeid={0}&key=" + Keys.GooglePlacesApiKey;
        }

        private Restful()
        {
            CookieContainer cookieContainer = new CookieContainer();
            handler = new HttpClientHandler { CookieContainer = cookieContainer };

            GenClient();
        }

        private void GenClient()
        {
            client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(60),
                // Using Default MaxResponseContentBuffer - 2 gigabytes
                //MaxResponseContentBufferSize = 25600,     // Too small
            };
            client.DefaultRequestHeaders.Add("Accept-Language", System.Globalization.CultureInfo.CurrentCulture.ToString());

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0 MatnaApp (iOS)");
                    break;
                case Device.Android:
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0 MatnaApp (Android)");
                    break;
                default:
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0 MatnaApp (Default)");
                    break;
            }
            client.DefaultRequestHeaders.ExpectContinue = false;
        }

        private async Task<T> GetAsyncWrapper<T>(Uri uri)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    MessagingCenter.Send(this, "NetworkUnavailable");
                    return default(T);
                }
#if DEBUG
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                stopWatch.Stop();
                System.Diagnostics.Debug.WriteLine($"Matna>> {uri}");
                System.Diagnostics.Debug.WriteLine($"Matna>> {String.Concat(Enumerable.Repeat("*", (int)Math.Ceiling(stopWatch.ElapsedMilliseconds / 100.0)))} {(int)stopWatch.ElapsedMilliseconds}");
#else
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
#endif
                return await DeserializeReturn<T>(response);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Matna>> Deserialize Error - Probably the packet size is big");
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return default(T);
        }

        private async Task<T> PostAsyncWrapper<T>(Uri uri, StringContent data)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    MessagingCenter.Send(this, "NetworkUnavailable");
                    return default(T);
                }
#if DEBUG
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                var response = await client.PostAsync(uri, data);
                response.EnsureSuccessStatusCode();
                stopWatch.Stop();
                System.Diagnostics.Debug.WriteLine($"Matna>> {uri}");
                System.Diagnostics.Debug.WriteLine($"Matna>> {String.Concat(Enumerable.Repeat("*", (int)Math.Ceiling(stopWatch.ElapsedMilliseconds / 100.0)))} {(int)stopWatch.ElapsedMilliseconds}");
#else
                var response = await client.PostAsync(uri, data);
                response.EnsureSuccessStatusCode();
#endif
                return await DeserializeReturn<T>(response);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Matna>> Deserialize Error - Probably the packet size is big");
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return default(T);
        }

        private async Task<T> DeserializeReturn<T>(HttpResponseMessage response)
        {
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            using (var json = new JsonTextReader(reader))
            {
                try
                {
                    return _serializer.Deserialize<T>(json);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Matna>> Deserialize Error");
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                return default(T);
            }
        }
    }
}
