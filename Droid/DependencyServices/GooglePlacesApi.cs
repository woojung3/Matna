using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Gms.Common.Apis;
using Android.Gms.Location.Places;
using Android.Gms.Maps.Model;
using Matna.Droid;
using Matna.Helpers.GooglePlacesApi;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

[assembly: Dependency(typeof(Matna.Droid.GooglePlacesApi))]

namespace Matna.Droid
{
    /// <inheritdoc />
    public class GooglePlacesApi : IGooglePlacesApi
    {
        private GoogleApiClient _apiClient;
        private AutocompletePredictionBuffer _buffer;

        ///<inheritdoc/>
        public async Task<IEnumerable<IPlaceResult>> GetPredictions(string query, MapSpan bounds)
        {
            if (this._apiClient == null || !this._apiClient.IsConnected) this.Connect();

            List<IPlaceResult> result = new List<IPlaceResult>();

            LatLngBounds latLngBounds = null;

            if (bounds != null)
            {
                double mDistanceInMeters = bounds.Radius.Meters;

                double latRadian = bounds.LatitudeDegrees;

                double degLatKm = 110.574235;
                double degLongKm = 110.572833 * Math.Cos(latRadian);
                double deltaLat = mDistanceInMeters / 1000.0 / degLatKm;
                double deltaLong = mDistanceInMeters / 1000.0 / degLongKm;

                double minLat = bounds.Center.Latitude - deltaLat;
                double minLong = bounds.Center.Longitude - deltaLong;
                double maxLat = bounds.Center.Latitude + deltaLat;
                double maxLong = bounds.Center.Longitude + deltaLong;

                latLngBounds = new LatLngBounds(new LatLng(minLat, minLong), new LatLng(maxLat, maxLong));
            }

            if (this._buffer != null)
            {
                this._buffer.Dispose();
                this._buffer = null;
            }

            this._buffer = await PlacesClass.GeoDataApi.GetAutocompletePredictionsAsync(
                this._apiClient,
                query,
                latLngBounds,
                null);

            if (this._buffer != null)
            {
                result.AddRange(this._buffer.Select(i =>
                    new NativeAndroidPlaceResult
                    {
                        Description = i.GetPrimaryText(null),
                        Subtitle = i.GetSecondaryText(null),
                        PlaceId = i.PlaceId,
                    }));
            }
            return result;
        }
        ///<inheritdoc/>
        public void Connect()
        {
            if (this._apiClient == null)
            {
                this._apiClient = new GoogleApiClient.Builder(Forms.Context)
                    .AddApi(PlacesClass.GEO_DATA_API)
                    .Build();
            }
            if (!this._apiClient.IsConnected && !this._apiClient.IsConnecting)
            {
                this._apiClient.Connect();
            }
        }
        ///<inheritdoc/>
        public void DisconnectAndRelease()
        {
            if (this._apiClient == null) return;

            if (this._apiClient.IsConnected)
                this._apiClient.Disconnect();

            this._apiClient.Dispose();
            this._apiClient = null;

            if (this._buffer != null)
            {
                this._buffer.Dispose();
                this._buffer = null;
            }
        }
        /// <inheritdoc/>
        public async Task<PlaceDetails> GetDetails(string id)
        {
            if (this._apiClient == null || !this._apiClient.IsConnected) this.Connect();

            var nativeResult = await PlacesClass.GeoDataApi.GetPlaceByIdAsync(this._apiClient, id);

            if (nativeResult == null || !nativeResult.Any()) return null;

            var nativeDetails = nativeResult.First();

            var types = new List<string>();
            if (nativeDetails.PlaceTypes.Contains((Java.Lang.Integer)79))
                types.Add("restaurant");

            return new PlaceDetails
            {
                PlaceId = nativeDetails.Id,
                Types = types,
                Coordinate = nativeDetails.LatLng.ToPosition(),
                FormattedAddress = nativeDetails.AddressFormatted.ToString(),
                InternationalPhoneNumber = nativeDetails.PhoneNumberFormatted?.ToString(),
                Website = nativeDetails.WebsiteUri?.ToString(),
                Name = nativeDetails.NameFormatted.ToString(),
                Rating = nativeDetails.Rating
            };
        }
    }

    public static class Extensions
    {

        public static Position ToPosition(this LatLng self)
        {
            return new Position(self.Latitude, self.Longitude);
        }
    }
}