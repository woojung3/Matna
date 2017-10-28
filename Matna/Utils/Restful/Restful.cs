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

namespace Matna.Utils.Restful
{
    public sealed class Restful
    {
        #region Google API
        public async Task<List<GooglePlaceNearbyItem>> GoogleMapsPlaceNearbySearch(double lat, double lon, double radius, List<string> types, List<string> keywords)
        {
            string urlBase = string.Format(Constants.GoogleMapsPlaceNearbysearch, lat, lon, radius, String.Join(",", types), String.Join(",", keywords));
            var uri = new Uri(urlBase);
            var rtn = await GetAsyncWrapper<GooglePlaceNearbys>(uri);

            if (rtn.Status == null)
                return new List<GooglePlaceNearbyItem>();
            if (rtn.Status.Equals("OK", StringComparison.CurrentCultureIgnoreCase))
            {
                if (rtn.Results.Any())
                    return rtn.Results;
            }
            return new List<GooglePlaceNearbyItem>();
        }

        public async Task<string> GoogleMapsPlaceNameFromDetails(string placeId)
        {
            string urlBase = string.Format(Constants.GoogleMapsPlaceDetails, placeId, Keys.GooglePlacesApiKey);
            var uri = new Uri(urlBase);
            var rtn = await GetAsyncWrapper<GooglePlaceDetails>(uri);

            if (rtn.Status == null || rtn.Result == null)
                return "";
            if (rtn.Status.Equals("OK", StringComparison.CurrentCultureIgnoreCase))
            {
                if (rtn.Result.Name == null)
                {
                    if (rtn.Result.AddressComponents.Any() && rtn.Result.AddressComponents.First().LongName != null)
                        return rtn.Result.AddressComponents.First().LongName;
                    else return "";
                }

                if (rtn.Result.AddressComponents != null && rtn.Result.AddressComponents.Any())
                {
                    try
                    {
                        return $"{rtn.Result.Name} {rtn.Result.AddressComponents.Find(x => x.Types.Contains("country")).ShortName}";
                    }
#pragma warning disable CS0168 // Variable is declared but never used
                    catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
                    {
                        return rtn.Result.Name;
                    }
                }
                return rtn.Result.Name;
            }
            return "";
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
            public static string GoogleMapsPlaceNearbysearch = MainAddr + "place/nearbysearch/json?location={0},{1}&radius={2}&type={3}&keyword={4}&key=" + Keys.GooglePlacesApiKey;
            public static string GoogleMapsPlaceDetails = MainAddr + "place/details/json?language=en&placeid={0}&key=" + Keys.GooglePlacesApiKey;
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
            client.DefaultRequestHeaders.Add("Referer", "https://arrangy.com/");
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