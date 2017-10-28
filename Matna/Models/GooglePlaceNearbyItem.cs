using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using Matna.Helpers;

namespace Matna.Models
{
    public class GooglePlaceNearbys
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("results")]
        public List<GooglePlaceNearbyItem> Results { get; set; }

        [JsonProperty("next_page_token")]
        public string NextPageToken { get; set; }
    }

    public class GooglePlaceNearbyItem
    {
        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
        public string IconUrl
        {
            get
            {
                if (Icon != null)
                    return Icon;
                else return PropertiesDictionary.NotFoundImage;
            }
        }

        [PrimaryKey, AutoIncrement, JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rating")]
        public double? Rating { get; set; }
        public double RatingD
        {
            get
            {
                if (Rating != null)
                    return (double)Rating;
                else
                    return 0.0;
            }
        }

        [JsonProperty("opening_hours")]
        public OpeningHours OpeningHours { get; set; }

        [JsonProperty("photos")]
        public List<Photo> Photos { get; set; }
        public string ImageUrl
        {
            get
            {
                if (Photos != null && Photos.Any())
                {
                    if (Photos[0].PhotoReference != null)
                        return $"https://maps.googleapis.com/maps/api/place/photo?maxwidth=80&photoreference={Photos[0].PhotoReference}&key={Keys.GooglePlacesApiKey}";
                }
                return PropertiesDictionary.NotFoundImage;
            }
        }
    }

    public class Geometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }
    }

    public class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lon { get; set; }
    }

    public class Photo
    {
        [JsonProperty("photo_reference")]
        public string PhotoReference { get; set; }
    }

    public class OpeningHours
    {
        [JsonProperty("open_now")]
        public bool OpenNow { get; set; }
    }
}
