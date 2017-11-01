using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using MapKit;
using Matna.Helpers.GooglePlacesApi;
using Matna.iOS;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

[assembly: Dependency(typeof(Matna.iOS.GooglePlacesApi))]

namespace Matna.iOS
{
    /// <summary>
    /// iOS implementation of the <see cref="IGooglePlacesApi"/>
    /// </summary>
    public class GooglePlacesApi : IGooglePlacesApi
    {
        /// <summary>
        /// Just to avoid linking
        /// </summary>
        [Preserve]
        public static void Init() { }
        ///<inheritdoc/>
        public void Connect()
        {
            // Nothing to do on iOS
        }
        ///<inheritdoc/>
        public void DisconnectAndRelease()
        {
            // Nothing to do on iOS
        }
        ///<inheritdoc/>
        public async Task<IEnumerable<IPlaceResult>> GetPredictions(string query, MapSpan bounds)
        {
            throw new NotImplementedException("Do not use Native API on iOS");

            // These uses Apple API. I want to use google places API
            //List<IPlaceResult> result = new List<IPlaceResult>();

            //var region = new MKCoordinateRegion(bounds.Center.ToLocationCoordinate(), new MKCoordinateSpan(0.25, 0.25));

            //var request = new MKLocalSearchRequest
            //{
            //    NaturalLanguageQuery = query,
            //    Region = region
            //};

            //MKLocalSearch search = new MKLocalSearch(request);
            //var nativeResult = await search.StartAsync();

            //if (nativeResult != null && nativeResult.MapItems != null)
            //{
            //    result.AddRange(nativeResult.MapItems.Select(i =>
            //        new NativeiOSPlaceResult
            //        {
            //            Description = string.Format("{0}, {1} {2}", i.Placemark.Title, i.Placemark.AdministrativeArea, i.Placemark.SubAdministrativeArea),
            //            Details = new PlaceDetails
            //            {
            //                Coordinate = i.Placemark.Coordinate.ToPosition(),
            //                FormattedAddress = i.Placemark.Title,
            //                InternationalPhoneNumber = i.PhoneNumber.ToString(),
            //                Website = i.Url?.ToString()

            //            }
            //        }));
            //    return result;
            //}
            //return null;
        }
        ///<inheritdoc/>
        public Task<PlaceDetails> GetDetails(string id)
        {
            throw new NotImplementedException("Do not use Native API on iOS");
        }
    }

    public static class Extensions
    {
        public static Position ToPosition(this CLLocationCoordinate2D self)
        {
            return new Position(self.Latitude, self.Longitude);
        }

        public static CLLocationCoordinate2D ToLocationCoordinate(this Position self)
        {
            return new CLLocationCoordinate2D(self.Latitude, self.Longitude);
        }
    }
}